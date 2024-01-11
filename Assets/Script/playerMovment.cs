using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


//Resources and tutorials that were used to help create this quick tester
// they can come in handy for future developpment, and a guide when it comes to creating our final product

//

public class playerMovement : MonoBehaviour
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
    bool readyToJump=true;

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

    private InputAction.CallbackContext playerInputActions;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

    }

    // Update is called once per frame
    void Update()
    {
        

        //print(grounded);
        // print("I'm ready:");
        // print(readyToJump) ;


        //rotate orientation

        
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        if (viewDir != Vector3.zero)
        {
            orientation.forward = viewDir.normalized;
        }
        // rotate the player object
        Vector3 inputDir = orientation.forward * vertInput + orientation.right * horInput;

        //if input direction isnt 0 smoothly change the direction using the rotation speed
        if (inputDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }

        //takes input of the keys for movement
        MyInput();
        SpeedControl();
        OrientPlayer();

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
        //MovePlayer();
        Vector2 inputVector = playerInputActions.ReadValue<Vector2>();
        rb.AddForce(new Vector3(inputVector.x, 0, inputVector.y).normalized * moveSpeed * 10f, ForceMode.Force);

        //ground check, send a raycast to check if the ground is present half way down the players body+0.2
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);
    }

    private void MyInput()
    {
       
        if(player.gameObject.tag == "Player1")
        {
            //horInput = Input.GetAxisRaw("Horizontal_P1");
            //vertInput = Input.GetAxisRaw("Vertical_P1");

            if (Input.GetKey(jumpKeyP1) && readyToJump && grounded)
            {
                //print("I should be jumping");
                readyToJump = false;
                Jump();
                // makes sure you can't double jump, but allow you to continuously jump iff you hodl down on the ke
                Invoke(nameof(ResetJump), jumpCoolDown);
            }
        }


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

    public void MovePlayer(InputAction.CallbackContext context)
    {//calculate movment direction
        //move in dirction you are looking

        Vector2 inputVector = context.ReadValue<Vector2>();
        horInput = inputVector.x;
        vertInput = inputVector.y;

        rb.AddForce(new Vector3(inputVector.x, 0, inputVector.y).normalized * moveSpeed * 10f, ForceMode.Force);
        /*
        moveDirection = orientation.forward * vertInput + orientation.right * horInput;

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            
        }
        // if you are in the air flying 
        else if (!grounded) 
        {
            rb.AddForce(moveDirection.normalized * moveSpeed  *10f* airMultiplier, ForceMode.Force);
        }
        */


    }

    private void OrientPlayer()
    {
        //rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        if (viewDir != Vector3.zero)
        {
            orientation.forward = viewDir.normalized;
        }

        // rotate the player object
        Vector3 inputDir = orientation.forward * vertInput + orientation.right * horInput;

        //if input direction isnt 0 smoothly change the direction using the rotation speed
        if (inputDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
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
}

