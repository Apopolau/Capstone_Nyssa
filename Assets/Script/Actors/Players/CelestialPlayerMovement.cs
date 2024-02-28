using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;


//Resources and tutorials that were used to help create this quick tester
// they can come in handy for future developpment, and a guide when it comes to creating our final product

//

public class CelestialPlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    //Rotation of the player
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public float rotationSpeed;

    //Movement of the player
    public float moveSpeed;
    //if the player is on the ground don't let them slip and slide let them have drag
    public float groundDrag;

    //Jumping
    public float jumpForce;
    public float jumpCoolDown;
    public float airMultiplier;
    bool readyToJump = true;

    //Keys Based on player
    public KeyCode jumpKeyP1 = KeyCode.Space;
    public KeyCode jumpKeyP2 = KeyCode.KeypadEnter;


    //Check if player is on the ground
    public float playerHeight;
    public LayerMask groundMask;
    bool grounded;

    float horInput;
    float vertInput;
    Vector3 moveDirection;
    bool isMoveKeyHeld;
    Vector2 inputVector;
    Matrix4x4 isometricIdentity = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    Vector3 isometricInput;
    int CelestialDeviceID;
    private PlayerInput playerInput;
    private CelestialPlayerInputActions celestialPlayerInputActions;
    //private CelestialPlayerInput celestialPlayerInput;

    private void Awake()
    {
        player = this.GetComponent<Transform>();
        celestialPlayerInputActions = GetComponent<CelestialPlayerControls>().controls;
        /* playerInput = GetComponent<PlayerInput>();
         celestialPlayerInputActions = new CelestialPlayerInputActions();
           if (this.GetComponent<CelestialPlayer>())
          {
              celestialPlayerInputActions.CelestialPlayerDefault.Enable();
              celestialPlayerInputActions.CelestialPlayerDefault.Walk.performed += MovePlayer;
          }*/

    }

   void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        if (GetComponent<CelestialPlayerControls>().thisDevice == CelestialPlayerControls.DeviceUsed.KEYBOARD)
        {
            CelestialDeviceID = Keyboard.current.deviceId;
        }
        else
        {
            CelestialDeviceID = Gamepad.all[0].deviceId;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //takes input of the keys for movement
        //MyInput();
        SpeedControl();
        OrientPlayer();

        //check if grounded
        if (grounded)
        {
            rb.drag= groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        //Vector2 inputVector;
        if (this.gameObject.tag == "Player2")
        {


            //inputVector = GetComponent<CelestialPlayerControls>().controls.CelestialPlayerDefault.Walk.ReadValue<Vector2>();
            rb.AddForce(new Vector3(inputVector.x - isometricInput.x + (inputVector.x / 2),
                0, inputVector.y - isometricInput.z + (inputVector.y / 2)).normalized * moveSpeed * 10f, ForceMode.Force);
            // Debug.Log("celestial FIXED UPDATE CHECK");
            // Debug.Log("celestial" + inputVector);
        }

        if (rb.velocity != Vector3.zero)
        {
            GetComponent<CelestialPlayerAnimator>().animator.SetBool(GetComponent<CelestialPlayerAnimator>().IfWalkingHash, true);
        }
        else
        {
            GetComponent<CelestialPlayerAnimator>().animator.SetBool(GetComponent<CelestialPlayerAnimator>().IfWalkingHash, false);
        }
        // rb.AddForce(new Vector3(inputVector.x, 0, inputVector.y).normalized * moveSpeed * 10f, ForceMode.Force);

        //ground check, send a raycast to check if the ground is present half way down the players body+0.2
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);
    }

    private void MyInput()
    {
       

        if (player.gameObject.tag == "Player2")
        {
            //horInput = Input.GetAxisRaw("Horizontal_P2");
            //vertInput = Input.GetAxisRaw("Vertical_P2");

            if (Input.GetKey(jumpKeyP2) && readyToJump && grounded)
            {
                //print("I should be jumping");
                readyToJump = false;
                Jump();
                // makes sure you can't double jump, but allow you to continuously jump iff you hodl down on the ke
                Invoke(nameof(ResetJump), jumpCoolDown);
            }
        }
    }

    public void MovePlayer(InputAction.CallbackContext context, Vector2 input)
    {
        //FixedUpdate();
        //calculate movment direction
        //move in dirction you are looking
        //Debug.Log("celestial walking");



        if (context.control.device.deviceId == CelestialDeviceID)

        {

            //inputVector = context.ReadValue<Vector2>();
            inputVector = input;
            // Debug.Log("celestialPLAYER" + inputVector);
            horInput = inputVector.x;
            vertInput = inputVector.y;
            isometricInput = isometricIdentity.MultiplyPoint3x4(new Vector3(horInput, 0, vertInput));

            isMoveKeyHeld = true;
            if (horInput < 0.1 && horInput > -0.1 && vertInput < 0.1 && vertInput > 0.1)
            {
                //Debug.Log("Stopping player movement");
                isometricInput = Vector3.zero;
                inputVector = Vector2.zero;
                return;
            }
            if (this.GetComponent<NavMeshAgent>())
            {
                if (this.GetComponent<NavMeshAgent>().enabled)
                {
                    //  this.GetComponent<CelestialPlayer>().enrouteToPlant = false;
                    this.GetComponent<NavMeshAgent>().ResetPath();
                    this.GetComponent<NavMeshAgent>().enabled = false;
                }
            }
            //if (this.GetComponent<NavMeshAgent>())
            //{
            //Debug.Log("Continuing Celestial movement");
            rb.AddForce(new Vector3(inputVector.x - isometricInput.x + (inputVector.x / 2),
                0, inputVector.y - isometricInput.z + (inputVector.y / 2)).normalized * moveSpeed * 10f, ForceMode.Force);

            //}
        }
    }
    public void EndMovement(InputAction.CallbackContext context)
    {
        if (context.control.device.deviceId == CelestialDeviceID)
        {
            if (context.canceled)
            {
                //Debug.Log("Cancelling Celestial movement");
                inputVector = Vector2.zero;
                isometricInput = Vector3.zero;
            }
        }
    }

    public void StopPlayer()
    {
        /*
        inputVector = new Vector2(0 , 0);
        horInput = inputVector.x;
        vertInput = inputVector.y;
        */
        //isMoveKeyHeld = true;
        if (this.GetComponent<NavMeshAgent>())
        {
            if (this.GetComponent<NavMeshAgent>().enabled)
            {
                //  this.GetComponent<CelestialPlayer>().enrouteToPlant = false;
                ResetNavAgent();
            }
        }
        //if (this.GetComponent<NavMeshAgent>())
        //{
            //rb.AddRelativeForce(new Vector3(inputVector.x, 0, inputVector.y).normalized * moveSpeed * 10f, ForceMode.Force);

        //}
    }

    private void OrientPlayer()
    {
        //If we've activated the nav mesh agent, we want to turn in the direction it is moving
        if (!this.GetComponent<NavMeshAgent>().enabled)
        {
            //rotate orientation
            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            if (viewDir != Vector3.zero)
            {
                orientation.forward = viewDir.normalized;
            }

            // rotate the player object
            Vector3 inputDir = orientation.forward * isometricInput.x + orientation.right * -isometricInput.z;

            //if input direction isnt 0 smoothly change the direction using the rotation speed
            if (inputDir != Vector3.zero)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
            }

        }
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

    public void ResetNavAgent()
    {
        
        //this.GetComponent<EarthPlayer>().enrouteToPlant = false;
        this.GetComponent<NavMeshAgent>().ResetPath();
        this.GetComponent<NavMeshAgent>().enabled = false;
        orientation.localRotation = new Quaternion(0, 0, 0, 1);
        this.gameObject.transform.rotation.Set(0, 0, 0, 1);
    }

    private void Jump()
    {
        //makes sure y velocity is set to 0
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        //set the forceMode to impulse since you are only jumping once
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

    }

    private void ResetJump()
    {
        readyToJump = true;
    }

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
}

