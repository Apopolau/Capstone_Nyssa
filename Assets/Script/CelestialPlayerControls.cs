
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CelestialPlayerControls : MonoBehaviour
{


    public CelestialPlayerInputActions controls;
    public InputActionReference rainAction;
   // public CelestialPlayerInputActions cele;

    int playerIndex = 0; // can only be 0 (for player 1) or 1 (for player 2)
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
        controls = new CelestialPlayerInputActions();
    }

    void Start()
    {
        // Gamepad.all gives us a list of all connected gamepads.
        // We need the deviceId of the gamepad that will control this character (the one this script is attached to), so that we can check it later when actions happen.
        // P1's deviceId is at index 0, P2's deviceID is at index 1...
        // For a two player game, we set the playerIndex via the inspector as either 0 or 1 

        myDeviceID = Gamepad.all[playerIndex].deviceId;

        controls.CelestialPlayerDefault.Enable();
        controls.CelestialPlayerDefault.Walk.performed += OnMovePerformed;
        controls.CelestialPlayerDefault.Walk.canceled += OnMoveCancelled;
        controls.CelestialPlayerDefault.MakeRain.performed += OnMakeRain; // <- we can talk about Attack here because P1Controls has an Attack action

       // controls.CelestialPlayerDefault.MakeRain.performed += OnAttackPerformed; // <- we can talk about Attack here because P1Controls has an Attack action

    }

   private void OnMovePerformed(InputAction.CallbackContext context)
    {
        // Before doing anything, we check to make sure that the current message came from the correct controller (i.e., that the sender's ID matches our saved ID)
        if (context.control.device.deviceId != myDeviceID) return;

       this.GetComponent<CelestialPlayerMovement>().MovePlayer(context);
      

    }

    private void OnMoveCancelled(InputAction.CallbackContext context)
    {
        // Before doing anything, we check to make sure that the current message came from the correct controller (i.e., that the sender's ID matches our saved ID)
        if (context.control.device.deviceId != myDeviceID) return;
        this.GetComponent<CelestialPlayerMovement>().StopPlayer();


    }
    private void OnMakeRain(InputAction.CallbackContext context)
    {
        // Before doing anything, we check to make sure that the current message came from the correct controller (i.e., that the sender's ID matches our saved ID)
        if (context.control.device.deviceId != myDeviceID) return;
        
        Debug.Log("call start rain");
      
       // rainAction.ToInputAction(BaseStateMachine.)
        //https://gamedevbeginner.com/input-in-unity-made-easy-complete-guide-to-the-new-system/#:~:text=To%20do%20that%2C%20right%2Dclick,Input%20Actions%20from%20the%20menu.&text=To%20create%20an%20Input%20Actions,Input%20Actions%20in%20the%20menu.
        //PlayerInput.switchcurrentactionMap("Menu");
      

    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        // Before doing anything, we check to make sure that the current message came from the correct controller (i.e., that the sender's ID matches our saved ID)
        if (context.control.device.deviceId != myDeviceID) return;

        Debug.Log("P1 A button means Attack!");
    }


    // Update is called once per frame
 
}
