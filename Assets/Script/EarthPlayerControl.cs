
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class EarthPlayerControl : MonoBehaviour
{


    public PlayerInputActions controls;
    public InputAction pickTreeAction;

    int playerIndex = 1; // can only be 0 (for player 1) or 1 (for player 2)
    int myDeviceID = 0; // used to store the ID of the controller that controls this particular player

    public float moveSpeed;

    // Whenever either of two gamepads are used, input event will be sent to *all* player objects.
    // So if controller C1 sends an input, both Player 1 and Player 2 will receive it. Same for controller C2.
    // To have C1's inputs affect only P1, and C2's inputs affect only P2, each Player needs to ignore the messages that it receives from the "wrong" controller.
    // To do that, we need to know two things:
    //  * which controller each Player is supposed to be paying attenion to
    //  * which controller sent each message that a Player receives

    // The Start function shows a way to decide which controller the current player object should attention to.
    // We save the information we need as the deviceID of the controller

    // The On___ functions below shows a way to identify which controller sent the current input message and filter on that basis:
    // - we use the context to check the deviceID of the sender, and we check that against our saved deviceID from the Start function.


    private void Awake()
    {
        controls = new PlayerInputActions();
    }

    void Start()
    {
        // Gamepad.all gives us a list of all connected gamepads.
        // We need the deviceId of the gamepad that will control this character (the one this script is attached to), so that we can check it later when actions happen.
        // P1's deviceId is at index 0, P2's deviceID is at index 1...
        // For a two player game, we set the playerIndex via the inspector as either 0 or 1 

        myDeviceID = Gamepad.all[playerIndex].deviceId;

        controls.EarthPlayerDefault.Enable();
        controls.EarthPlayerDefault.Walk.performed += OnMovePerformed;
        controls.EarthPlayerDefault.Walk.canceled += OnMoveCancelled;
        controls.EarthPlayerDefault.PickTree.performed+= OnPickTree; // <- we can talk about Attack here because P1Controls has an Attack action
        controls.EarthPlayerDefault.PickFlower.performed += OnPickFlower; // <- we can talk about Attack here because P1Controls has an Attack action
        controls.EarthPlayerDefault.PickGrass.performed += OnPickGrass; // <- we can talk about Attack here because P1Controls has an Attack action
        controls.EarthPlayerDefault.RemoveBuilding.performed += OnRemovePlant;
        controls.EarthPlayerDefault.Interact.performed += OnInteract;

        // controls.CelestialPlayerDefault.MakeRain.performed += OnAttackPerformed; // <- we can talk about Attack here because P1Controls has an Attack action

    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        // Before doing anything, we check to make sure that the current message came from the correct controller (i.e., that the sender's ID matches our saved ID)
     if (context.control.device.deviceId != myDeviceID) return;
       this.GetComponent<playerMovement>().MovePlayer(context);
       
       

    }

    private void OnMoveCancelled(InputAction.CallbackContext context)
    {
        // Before doing anything, we check to make sure that the current message came from the correct controller (i.e., that the sender's ID matches our saved ID)
        if (context.control.device.deviceId != myDeviceID) return;
        this.GetComponent<playerMovement>().StopPlayer();


    }
    private void OnPickTree(InputAction.CallbackContext context)
    {
        // Before doing anything, we check to make sure that the current message came from the correct controller (i.e., that the sender's ID matches our saved ID)
        if (context.control.device.deviceId != myDeviceID) return;

        Debug.Log("call pickup tree");
        this.GetComponent<EarthPlayer>().OnTreeSelected(context);
        //pickTreeAction.(context);


    }
    private void OnPickFlower(InputAction.CallbackContext context)
    {
        // Before doing anything, we check to make sure that the current message came from the correct controller (i.e., that the sender's ID matches our saved ID)
        if (context.control.device.deviceId != myDeviceID) return;

        Debug.Log("call pickup tree");
        this.GetComponent<EarthPlayer>().OnFlowerSelected(context);
        //pickTreeAction.(context);


    }
    private void OnPickGrass(InputAction.CallbackContext context)
    {
        // Before doing anything, we check to make sure that the current message came from the correct controller (i.e., that the sender's ID matches our saved ID)
        if (context.control.device.deviceId != myDeviceID) return;

        Debug.Log("call pickup tree");
        this.GetComponent<EarthPlayer>().OnFlowerSelected(context);
        //pickTreeAction.(context);


    }
    private void OnRemovePlant(InputAction.CallbackContext context)
    {
        // Before doing anything, we check to make sure that the current message came from the correct controller (i.e., that the sender's ID matches our saved ID)
        if (context.control.device.deviceId != myDeviceID) return;

        Debug.Log("call pickup tree");
        this.GetComponent<EarthPlayer>().RemovePlant();
        //pickTreeAction.(context);


    }
    private void OnInteract(InputAction.CallbackContext context)
    {
        // Before doing anything, we check to make sure that the current message came from the correct controller (i.e., that the sender's ID matches our saved ID)
        if (context.control.device.deviceId != myDeviceID) return;

        Debug.Log("call pickup tree");
        this.GetComponent<EarthPlayer>().OnInteract(context);
        //pickTreeAction.(context);


    }
    /*
        private void FixedUpdate()
        {

            Vector2 inputVector;
            if (this.GetComponent<EarthPlayer>())
            {
                inputVector =controls.EarthPlayerDefault.Walk.ReadValue<Vector2>();
            }
            else
            {
                inputVector = new Vector2(0, 0);
            }

           rb.AddForce(new Vector3(inputVector.x, 0, inputVector.y).normalized * moveSpeed * 10f, ForceMode.Force);

            //ground check, send a raycast to check if the ground is present half way down the players body+0.2
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);
        }
    */

}
