using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;


//Resources and tutorials that were used to help create this quick tester
// they can come in handy for future developpment, and a guide when it comes to creating our final product

//

public class playerMovement : MonoBehaviour
{
    [Header("Key references")]
    private Rigidbody rb;
    private Transform player;
    [SerializeField] private Transform playerObj;
    [SerializeField] private Transform orientation;
    [SerializeField] private GameObject movementControlsUI;

    [Header("Movement Variables")]
    private float horInput;
    private float vertInput;
    //private Vector3 moveDirection;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float groundDrag; //prevents player from sliding
    [SerializeField] private float gravityScale;
    //Check if player is on the ground
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private bool grounded;
    [SerializeField] private bool playerHasMoved = false;

    [Header("Rotation Variables")]
    [SerializeField] private float rotationSpeed;
    //Serialized for testing
    [SerializeField] private bool isTurning = false;
    private GameObject turnToTarget;
    //Rotation of the player

    //Variables for handling isometric view vs 2D input map
    private Vector2 inputVector;
    private Matrix4x4 isometricIdentity = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    private Matrix4x4 isometricRotIdentity = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    private Vector3 isometricInput;
    private Vector3 isometricRotInput;

    //Jumping was cut from the game but if we ever chose to re-implement it, the variables are here
    //Jumping
    //private float jumpForce;
    //private float jumpCoolDown;
    //private float airMultiplier;
    //bool readyToJump = true;

    private void Awake()
    {
        player = this.GetComponent<Transform>();
        //playerInputActions = GetComponent<EarthPlayerControl>().controls;
        isometricInput = isometricIdentity.MultiplyPoint3x4(new Vector3(horInput, 0, vertInput));
        isometricRotInput = isometricRotIdentity.MultiplyPoint3x4(new Vector3(horInput, 0, vertInput));
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        /*
        if (GetComponent<EarthPlayerControl>().userSettingsManager.earthControlType == UserSettingsManager.ControlType.KEYBOARD)
        {
            EarthDeviceID = Keyboard.current.deviceId;
        }
        else
        {
            EarthDeviceID = Gamepad.all[1].deviceId;
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        SpeedControl();

        //check if grounded
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        if (this.gameObject.tag == "Player1")
        {
            //isometricInput = isometricIdentity.MultiplyPoint3x4(new Vector3(horInput, 0, vertInput));
            rb.AddForce(new Vector3(isometricInput.z, 0, isometricInput.x).normalized * moveSpeed * 10f, ForceMode.Force);
        }
        //ground check, send a raycast to check if the ground is present half way down the players body+0.2
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);
        if (rb.velocity.magnitude > new Vector3(0.1f, 0.1f, 0.1f).magnitude)
        {
            if(!GetComponent<EarthPlayerAnimator>().GetAnimationFlag("move"))
                GetComponent<EarthPlayerAnimator>().SetAnimationFlag("move", true);
        }
        else if(GetComponent<NavMeshAgent>().enabled && GetComponent<NavMeshAgent>().hasPath)
        {
            //GetComponent<EarthPlayer>().SetAnimationFlag("move", true);
        }
        else
        {
            if(GetComponent<EarthPlayerAnimator>().GetAnimationFlag("move"))
                GetComponent<EarthPlayerAnimator>().SetAnimationFlag("move", false);
        }
        if (!grounded)
        {
            rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);
        }

        if (isTurning)
        {
            TurnToTarget();
        }
        else
        {
            if(playerHasMoved)
                OrientPlayer();
        }
    }

    //This is the function that handles actually moving the player
    public void MovePlayer(Vector2 input)
    {
        inputVector = input;
        horInput = inputVector.x;
        vertInput = inputVector.y;
        isometricInput = isometricIdentity.MultiplyPoint3x4(new Vector3(horInput, 0, vertInput));
        isometricRotInput = isometricRotIdentity.MultiplyPoint3x4(new Vector3(horInput, 0, vertInput));

        if (horInput < 0.2 && horInput > -0.2 && vertInput < 0.2 && vertInput > -0.2)
        {
            inputVector = Vector2.zero;
            isometricInput = Vector3.zero;
            isometricRotInput = Vector3.zero;
            return;
        }
        //Call this if the player is in the middle of navigating using the nav agent
        if (this.GetComponent<NavMeshAgent>().enabled)
        {
            ResetNavAgent();
        }

        rb.AddForce(new Vector3(isometricInput.z, 0, isometricInput.x).normalized * moveSpeed * 10f, ForceMode.Force);

        // if player moved disable the UI
        if (input != Vector2.zero)
        {
            // Disable the UI element
            if (movementControlsUI != null)
            {
                playerHasMoved = true;
                movementControlsUI.SetActive(false);
            }
        }
    }

    //This is called when the player stops inputting on their movement controls
    public void EndMovement(InputAction.CallbackContext context)
    {
        inputVector = Vector2.zero;
        isometricInput = Vector3.zero;
        isometricRotInput = Vector3.zero;
    }

    //This turns the player in the direction they are moving
    private void OrientPlayer()
    {
        //If we're using player inputs, we want to face the direction they're walking
        if (!this.GetComponent<NavMeshAgent>().enabled)
        {
            //rotate orientation
            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);

            if (viewDir != Vector3.zero)
            {
                orientation.forward = viewDir.normalized;
            }

            // rotate the player object
            Vector3 inputDir = orientation.forward * isometricRotInput.x + orientation.right * isometricRotInput.z;

            //if input direction isnt 0 smoothly change the direction using the rotation speed
            if (inputDir != Vector3.zero)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
            }

        }
        //If we've activated the nav mesh agent, we want to turn in the direction it is moving
        else
        {

            //rotate orientation
            Vector3 viewDir = player.position - this.GetComponent<NavMeshAgent>().steeringTarget;
            if (viewDir != Vector3.zero)
            {
                orientation.forward = viewDir.normalized;
            }

            // rotate the player object
            Vector3 inputDir = -orientation.forward;

            //if input direction isnt 0 smoothly change the direction using the rotation speed
            if (inputDir != Vector3.zero)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
            }

        }
    }

    //When run, turns player geometry towards a defined target object
    public void TurnToTarget()
    {
        if(turnToTarget != this.gameObject)
        {
            float step;
            //float speed = 0.3f;
            step = rotationSpeed * Time.deltaTime;

            //Get the direction we should be looking in
            Vector3 lookDirection = turnToTarget.transform.position - playerObj.position;
            lookDirection.y = 0;
            //Pick a stepped direction between that direction and current direction
            Vector3 rotateVector = Vector3.RotateTowards(playerObj.forward, lookDirection, step, 0.1f);
            //Set our rotation to that
            playerObj.rotation = Quaternion.LookRotation(rotateVector);
        }
    }

    //Keeps player speed below a certain threshold
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        //if the player is surpassing the max speed, which we can edit later on, then set it to the max speed
        if (flatVel.magnitude > moveSpeed)
        {
            //a calculation of what your max velocity swould be 
            Vector3 limitVel = flatVel.normalized * moveSpeed;
            //now apply the max velotiy to the x and z axis
            rb.velocity = new Vector3(limitVel.x, rb.velocity.y, limitVel.z);
        }
    }

    //Handles setting all variables related to nav agent back to default
    public void ResetNavAgent()
    {
        this.GetComponent<EarthPlayer>().enrouteToPlant = false;
        this.GetComponent<EarthPlayer>().ResetAgentPath();
        this.GetComponent<NavMeshAgent>().enabled = false;
        orientation.localRotation = new Quaternion(0, 0, 0, 1);
    }

    /*
    private void Jump()
    {
        //makes sure y velocity is set to 0
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        //set the forceMode to impulse since you are only jumping once
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

    }
    */

    private void OnTriggerStay(Collider other)
    {
        //If we're touching the ground
        if (other.gameObject.layer == 6)
        {
            grounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //If we're not touching the ground
        if (other.gameObject.layer == 6)
        {
            grounded = false;
        }
    }

    public Transform GetPlayerGeo()
    {
        return playerObj;
    }

    public void SetTurning(bool isTurning)
    {
        this.isTurning = isTurning;
    }

    public bool GetTurning()
    {
        return isTurning;
    }

    public void SetTurnTarget(GameObject target)
    {
        turnToTarget = target;
    }
}

