
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class EarthPlayerControl : MonoBehaviour
{

    private EarthPlayer earthPlayer;
    public PlayerInputActions controls;
    public InputAction pickTreeAction;
    public LevelOneEvents levelOneEvents;
    public DialogueManager dialogueManager;

    public enum DeviceUsed { KEYBOARD, CONTROLLER};
    public DeviceUsed thisDevice;

    int playerIndex = 1; // can only be 0 (for player 1) or 1 (for player 2)
    public int myDeviceID = -1; // used to store the ID of the controller that controls this particular player
    public int myDeviceID2 = -1;

    public float moveSpeed;

    bool takinginput=false;

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
        earthPlayer = GetComponent<EarthPlayer>();
    }

    void Start()
    {
        // Gamepad.all gives us a list of all connected gamepads.
        // We need the deviceId of the gamepad that will control this character (the one this script is attached to), so that we can check it later when actions happen.
        // P1's deviceId is at index 0, P2's deviceID is at index 1...
        // For a two player game, we set the playerIndex via the inspector as either 0 or 1 
        if(Gamepad.all.Count == 0)
        {
            myDeviceID = Keyboard.current.deviceId;
            myDeviceID2 = Mouse.current.deviceId;
            thisDevice = DeviceUsed.KEYBOARD;
        }
        else if(Gamepad.all.Count == 1)
        {
            myDeviceID = Keyboard.current.deviceId;
            myDeviceID2 = Mouse.current.deviceId;
            thisDevice = DeviceUsed.KEYBOARD;
        }
        else
        {
            myDeviceID = Gamepad.all[playerIndex].deviceId;
            thisDevice = DeviceUsed.CONTROLLER;
        }

        //Test
        controls.EarthPlayerDefault.Enable();
        controls.PlantIsSelected.Enable();
        controls.RemovingPlant.Enable();
        controls.MenuControls.Enable();
        controls.DialogueControls.Enable();

        Debug.Log(myDeviceID);
        //EarthPlayerDefault
        controls.EarthPlayerDefault.Disable();
        controls.EarthPlayerDefault.EarthWalk.performed += OnEarthMovePerformed;
        controls.EarthPlayerDefault.EarthWalk.canceled += OnEarthMoveCancelled;
        controls.EarthPlayerDefault.PickTree.performed += OnPickTree; // <- we can talk about Attack here because P1Controls has an Attack action
        controls.EarthPlayerDefault.PickFlower.performed += OnPickFlower; // <- we can talk about Attack here because P1Controls has an Attack action
        controls.EarthPlayerDefault.PickGrass.performed += OnPickGrass; // <- we can talk about Attack here because P1Controls has an Attack action
        controls.EarthPlayerDefault.RemoveBuilding.performed += OnRemovePlant;
        controls.EarthPlayerDefault.Interact.started += OnInteract;
        controls.EarthPlayerDefault.Interact.canceled += OnInteract;
        controls.EarthPlayerDefault.DebugTileflip.performed += OnTileFlipped;

        //When planting
        controls.PlantIsSelected.Disable();
        controls.PlantIsSelected.Plantplant.performed += OnPlantPlantedPerformed;
        controls.PlantIsSelected.Cancelplanting.performed += OnPlantingCancelledPerformed;
        controls.PlantIsSelected.EarthWalk.performed += OnEarthMovePerformed;
        controls.PlantIsSelected.EarthWalk.canceled += OnEarthMoveCancelled;

        //When removing plant
        controls.RemovingPlant.Disable();
        controls.RemovingPlant.RemovePlant.performed += OnPlantRemoved;
        controls.RemovingPlant.CancelRemoval.performed += OnRemovingPlantCancelled;
        controls.RemovingPlant.EarthWalk.performed += OnEarthMovePerformed;
        controls.RemovingPlant.EarthWalk.canceled += OnEarthMoveCancelled;

        //When in the menus
        //We may want to switch this one to be active when we start up the game instead of the default
        controls.MenuControls.Disable();

        //When in dialogue
        controls.DialogueControls.Enable();
        controls.DialogueControls.Continue.started += OnContinuePerformed;
        controls.DialogueControls.Skip.started += OnSkipPerformed;
    }


    /// <summary>
    /// DEFAULT CONTROLS
    /// </summary>
    /// <param name="context"></param>
    private void OnEarthMovePerformed(InputAction.CallbackContext context)
    {
        // Before doing anything, we check to make sure that the current message came from the correct controller (i.e., that the sender's ID matches our saved ID)
        // if (context.control.device.deviceId != myDeviceID && context.control.device.deviceId == myDeviceID) return;
        //if (context.control.device.deviceId != myDeviceID) 
        //{ return; 
        //}
     
        if(context.control.device.deviceId == myDeviceID)
        {
            if (takinginput == false)
            {
                takinginput = true;
                //Debug.Log("Earth:" + context.control.device.deviceId);


                Vector2 input;
                if(thisDevice == DeviceUsed.KEYBOARD)
                {
                    input = new Vector2(Keyboard.current.upArrowKey.ReadValue() - Keyboard.current.downArrowKey.ReadValue(),
                        Keyboard.current.leftArrowKey.ReadValue() - Keyboard.current.rightArrowKey.ReadValue());
                }
                else
                {
                    input = Gamepad.all[playerIndex].leftStick.ReadValue();
                }
                this.GetComponent<playerMovement>().MovePlayer(context, input);
                takinginput = false;
            }
        }
    }

    private void OnEarthMoveCancelled(InputAction.CallbackContext context)
    {
        // Before doing anything, we check to make sure that the current message came from the correct controller (i.e., that the sender's ID matches our saved ID)
        if (context.control.device.deviceId == myDeviceID)
        {
            this.GetComponent<playerMovement>().EndMovement(context);
        }
    }
    private void OnPickTree(InputAction.CallbackContext context)
    {
        if (context.control.device.deviceId == myDeviceID)
        {
            Debug.Log("call pick tree");
            this.GetComponent<EarthPlayer>().OnTreeSelected(context);
        }

    }
    private void OnPickFlower(InputAction.CallbackContext context)
    {
        // Before doing anything, we check to make sure that the current message came from the correct controller (i.e., that the sender's ID matches our saved ID)
        if (context.control.device.deviceId == myDeviceID)
        {
            Debug.Log("call pick flower");
            this.GetComponent<EarthPlayer>().OnFlowerSelected(context);
        }

    }
    private void OnPickGrass(InputAction.CallbackContext context)
    {
        // Before doing anything, we check to make sure that the current message came from the correct controller (i.e., that the sender's ID matches our saved ID)
        if (context.control.device.deviceId == myDeviceID)
        {
            Debug.Log("call pick grass");
            this.GetComponent<EarthPlayer>().OnGrassSelected(context);
        }

    }
    private void OnRemovePlant(InputAction.CallbackContext context)
    {
        // Before doing anything, we check to make sure that the current message came from the correct controller (i.e., that the sender's ID matches our saved ID)
        if (context.control.device.deviceId == myDeviceID)
        {
            this.GetComponent<EarthPlayer>().OnRemovePlant();
        }

    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        // Before doing anything, we check to make sure that the current message came from the correct controller (i.e., that the sender's ID matches our saved ID)
        if (context.control.device.deviceId == myDeviceID)
        {

            Debug.Log("call pickup tree");
            this.GetComponent<EarthPlayer>().OnInteract(context);
        }
    }

    /// <summary>
    /// DEBUG CONTROLS (DEFAULT)
    /// </summary>
    /// <param name="context"></param>
    private void OnTileFlipped(InputAction.CallbackContext context)
    {
            Debug.Log("flipping tiles");
            levelOneEvents.DebugTileFlip();
    }



    /// <summary>
    /// PLANTING PLANT CONTROLS
    /// </summary>
    /// <param name="context"></param>
    private void OnPlantPlantedPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Tried to plant a plant");
        if (context.control.device.deviceId == myDeviceID || (thisDevice == DeviceUsed.KEYBOARD && Mouse.current.leftButton.wasPressedThisFrame))
        {
            Debug.Log("planting plant");
            earthPlayer.PlantPlantingHandler();
        }
    }

    private void OnPlantingCancelledPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Tried to cancel planting a plant");
        if (context.control.device.deviceId == myDeviceID || (thisDevice == DeviceUsed.KEYBOARD && Mouse.current.rightButton.wasPressedThisFrame))
        {
            Debug.Log("cancelling plant");
            earthPlayer.OnPlantingCancelled();
        }
    }



    /// <summary>
    /// PLANT REMOVAL CONTROLS
    /// </summary>
    /// <param name="context"></param>
    private void OnPlantRemoved(InputAction.CallbackContext context)
    {
        if (context.control.device.deviceId == myDeviceID || (thisDevice == DeviceUsed.KEYBOARD && Mouse.current.leftButton.wasPressedThisFrame))
        {
            Debug.Log("removing plant");
            earthPlayer.PlantRemovingHandler();
        }
    }

    private void OnRemovingPlantCancelled(InputAction.CallbackContext context)
    {
        if (context.control.device.deviceId == myDeviceID || (thisDevice == DeviceUsed.KEYBOARD && Mouse.current.rightButton.wasPressedThisFrame))
        {
            Debug.Log("cancelled removing plant");
            earthPlayer.OnRemovingCancelled();
        }
    }



    /// <summary>
    /// MENU CONTROLS
    /// </summary>



    /// <summary>
    /// DIALOGUE CONTROLS
    /// </summary>
    /// <param name="context"></param>
    private void OnContinuePerformed(InputAction.CallbackContext context)
    {
        if (context.control.device.deviceId == myDeviceID)
        {
            float input;
            if (thisDevice == DeviceUsed.CONTROLLER)
            {
                input = Gamepad.all[playerIndex].buttonSouth.ReadValue();
            }
            else
            {
                input = Keyboard.current.enterKey.ReadValue();
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
}
