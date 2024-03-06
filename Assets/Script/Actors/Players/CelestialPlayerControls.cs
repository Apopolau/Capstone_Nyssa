
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CelestialPlayerControls : MonoBehaviour
{


    public CelestialPlayerInputActions controls;
    public InputActionReference rainAction;
    public InputAction celestialActions;
    public DialogueManager dialogueManager;
    public UserSettingsManager userSettingsManager;
    // public CelestialPlayerInputActions cele;

    int playerIndex = 0; // can only be 0 (for player 1) or 1 (for player 2)
    public int myDeviceID = 0; // used to store the ID of the controller that controls this particular player

    public float moveSpeed;

    bool takinginput = false;


    private Keyboard kb;
    private Mouse ms;
    private Gamepad gp;
    private bool initialized;
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



    private void Start()
    {
        // Gamepad.all gives us a list of all connected gamepads.
        // We need the deviceId of the gamepad that will control this character (the one this script is attached to), so that we can check it later when actions happen.
        // P1's deviceId is at index 0, P2's deviceID is at index 1...
        // For a two player game, we set the playerIndex via the inspector as either 0 or 1 

        if (Gamepad.all.Count == 0)
        {
            myDeviceID = Keyboard.current.deviceId;
            userSettingsManager.celestialControlType = UserSettingsManager.ControlType.KEYBOARD;
        }
        else if (Gamepad.all.Count == 1)
        {
            myDeviceID = Gamepad.all[playerIndex].deviceId;
            userSettingsManager.celestialControlType = UserSettingsManager.ControlType.CONTROLLER;
        }
        else
        {
            myDeviceID = Gamepad.all[playerIndex].deviceId;
            userSettingsManager.celestialControlType = UserSettingsManager.ControlType.CONTROLLER;
        }

        Debug.Log(myDeviceID);
        controls.CelestialPlayerDefault.Disable();
        controls.CelestialPlayerDefault.CelestialWalk.performed += OnCelestialMovePerformed;
        controls.CelestialPlayerDefault.CelestialWalk.canceled += OnCelestialMoveCancelled;
        controls.CelestialPlayerDefault.MakeRain.performed += OnMakeRain; // <- we can talk about Attack here because P1Controls has an Attack action
        controls.CelestialPlayerDefault.MakeColdSnap.performed += OnColdSnapPerformed; // <- we can talk about Attack here because P1Controls has an Attack action
        controls.CelestialPlayerDefault.MakeInteract.started += OnInteract;
        controls.CelestialPlayerDefault.MakeInteract.canceled += OnInteract;

        //When in the menus
        //We may want to switch this one to be active when we start up the game instead of the default
        controls.MenuControls.Disable();

        //When in dialogue
        controls.DialogueControls.Enable();
        controls.DialogueControls.Continue.started += OnContinuePerformed;
        controls.DialogueControls.Skip.started += OnSkipPerformed;
    }

    private void OnCelestialMovePerformed(InputAction.CallbackContext context)
    {
        if (context.control.device.deviceId == myDeviceID)
        {
            if (takinginput == false)
            {
                takinginput = true;

                Vector2 input;
                if (userSettingsManager.celestialControlType == UserSettingsManager.ControlType.KEYBOARD)
                {
                    input = new Vector2(Keyboard.current.wKey.ReadValue() - Keyboard.current.sKey.ReadValue(),
                        Keyboard.current.aKey.ReadValue() - Keyboard.current.dKey.ReadValue());
                }
                else
                {
                    input = Gamepad.all[playerIndex].leftStick.ReadValue();
                }

                this.GetComponent<CelestialPlayerMovement>().MovePlayer(input);
                takinginput = false;
            }
        }
    }

    private void OnCelestialMoveCancelled(InputAction.CallbackContext context)
    {
        // Before doing anything, we check to make sure that the current message came from the correct controller (i.e., that the sender's ID matches our saved ID)
        //if (context.control.device.deviceId != myDeviceID) return;

        if (context.control.device.deviceId == myDeviceID)
        {
            this.GetComponent<CelestialPlayerMovement>().EndMovement();
        }
    }

    private void OnMakeRain(InputAction.CallbackContext context)
    {
        // Before doing anything, we check to make sure that the current message came from the correct controller (i.e., that the sender's ID matches our saved ID)
        if (context.control.device.deviceId != myDeviceID) return;

        Debug.Log("call start rain");


        //<CelestialPlayer>().isRaining = true;
        this.GetComponent<CelestialPlayer>().OnRainDropSelected();
        //rainAction.ToInputAction(BaseStateMachine.)
        //https://gamedevbeginner.com/input-in-unity-made-easy-complete-guide-to-the-new-system/#:~:text=To%20do%20that%2C%20right%2Dclick,Input%20Actions%20from%20the%20menu.&text=To%20create%20an%20Input%20Actions,Input%20Actions%20in%20the%20menu.
        //PlayerInput.switchcurrentactionMap("Menu");


    }

    private void OnColdSnapPerformed(InputAction.CallbackContext context)
    {
        // Before doing anything, we check to make sure that the current message came from the correct controller (i.e., that the sender's ID matches our saved ID)
        if (context.control.device.deviceId != myDeviceID) return;
        this.GetComponent<CelestialPlayer>().OnSnowFlakeSelected();

    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        // Before doing anything, we check to make sure that the current message came from the correct controller (i.e., that the sender's ID matches our saved ID)
        if (context.control.device.deviceId == myDeviceID)
        {

            Debug.Log("call pickup tree");
            this.GetComponent<CelestialPlayer>().OnInteract(context);
        }
    }

    //UI calls
    private void OnContinuePerformed(InputAction.CallbackContext context)
    {

        if (context.control.device.deviceId == myDeviceID)
        {
            float input;
            if (userSettingsManager.celestialControlType == UserSettingsManager.ControlType.CONTROLLER)
            {
                input = Gamepad.all[playerIndex].buttonSouth.ReadValue();
            }
            else
            {
                input = 0;
            }
            if (input > 0)
            {
                dialogueManager.DisplayNextDialogueLine();
            }
        }
    }

    private void OnSkipPerformed(InputAction.CallbackContext context)
    {
        if (context.control.device.deviceId == myDeviceID)
        {
            Debug.Log("skipping dialogue");
            dialogueManager.EndDialogue();
        }
    }
    // Update is called once per frame

}
