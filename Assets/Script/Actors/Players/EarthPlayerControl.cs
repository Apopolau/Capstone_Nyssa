
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class EarthPlayerControl : MonoBehaviour
{
    private EarthPlayer earthPlayer;
    public PlayerInputActions controls;
    public InputAction pickTreeAction;
    public MainMenu mainMenu;
    public EventManager eventManager;
    public CutsceneManager cutsceneManager;
    public DialogueManager dialogueManager;
    public UserSettingsManager userSettingsManager;
    public VirtualMouseInput virtualMouseInput;
    public Vector2 virtualMousePosition;

    //public enum DeviceUsed { KEYBOARD, CONTROLLER};
    //public DeviceUsed thisDevice;

    int playerIndex = 1; // can only be 0 (for player 1) or 1 (for player 2)
    public int myDeviceID = -1; // used to store the ID of the controller that controls this particular player
    public int myDeviceID2 = -1;

    public float moveSpeed;

    bool takinginput = false;

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

    private void OnEnable()
    {
        ResetControls();
    }

    private void OnDisable()
    {
        ShutOffControls();
    }

    void Start()
    {
        // Gamepad.all gives us a list of all connected gamepads.
        // We need the deviceId of the gamepad that will control this character (the one this script is attached to), so that we can check it later when actions happen.
        // P1's deviceId is at index 0, P2's deviceID is at index 1...
        // For a two player game, we set the playerIndex via the inspector as either 0 or 1 
        if (Gamepad.all.Count == 0)
        {
            myDeviceID = Keyboard.current.deviceId;
            myDeviceID2 = Mouse.current.deviceId;
            userSettingsManager.earthControlType = UserSettingsManager.ControlType.KEYBOARD;
        }
        else if (Gamepad.all.Count == 1)
        {
            myDeviceID = Keyboard.current.deviceId;
            myDeviceID2 = Mouse.current.deviceId;
            userSettingsManager.earthControlType = UserSettingsManager.ControlType.KEYBOARD;
        }
        else
        {
            myDeviceID = Gamepad.all[playerIndex].deviceId;
            userSettingsManager.earthControlType = UserSettingsManager.ControlType.CONTROLLER;
        }

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
        //controls.EarthPlayerDefault.DebugTileflip.performed += OnTileFlipped;
        controls.EarthPlayerDefault.Heal.started += OnHealPerformed;
        controls.EarthPlayerDefault.ThornShield.started += OnThornShieldPerformed;

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

        controls.HealSelect.Disable();
        controls.HealSelect.SelectTarget.started += OnTargetSelected;
        controls.HealSelect.CancelHeal.started += OnHealCancelled;
        controls.HealSelect.CycleTarget.started += OnTargetCycled;

        controls.BarrierSelect.Disable();
        controls.BarrierSelect.SelectTarget.started += OnTargetSelected;
        controls.BarrierSelect.CancelBarrier.started += OnBarrierCancelled;
        controls.BarrierSelect.CycleTarget.started += OnTargetCycled;

        controls.HoldingNyssa.Disable();
        controls.HoldingNyssa.EarthWalk.started += OnEarthMovePerformed;
        controls.HoldingNyssa.EarthWalk.canceled += OnEarthMoveCancelled;
        controls.HoldingNyssa.PutNyssaDown.started += OnPutNyssaDown;

        //When in the menus
        //We may want to switch this one to be active when we start up the game instead of the default
        controls.MenuControls.Disable();
        //controls.MenuControls.Submit.started += OnMenuSubmitPerformed;

        controls.CutsceneControls.Disable();
        controls.CutsceneControls.NextSlide.started += OnNextSlideSelected;

        //When in dialogue
        controls.DialogueControls.Disable();
        controls.DialogueControls.Continue.started += OnContinuePerformed;
        controls.DialogueControls.Skip.started += OnSkipPerformed;

        ResetControls();
    }


    /// <summary>
    /// NON-PLAYER BUTTON FUNCTIONS
    /// </summary>
    public void ResetControls()
    {
        //Set our starting controls based on context
        if (mainMenu != null)
        {
            controls.MenuControls.Enable();
            controls.CutsceneControls.Disable();
            controls.EarthPlayerDefault.Disable();
        }
        else if (cutsceneManager != null)
        {
            controls.CutsceneControls.Enable();
            controls.MenuControls.Disable();
            controls.EarthPlayerDefault.Disable();
        }
        else if (dialogueManager != null)
        {
            controls.CutsceneControls.Disable();
            controls.MenuControls.Disable();
            controls.EarthPlayerDefault.Enable();
        }
    }

    public void ShutOffControls()
    {
        controls.CutsceneControls.Disable();
        controls.MenuControls.Disable();
        controls.EarthPlayerDefault.Disable();
    }


    /// <summary>
    /// DEFAULT CONTROLS
    /// </summary>
    /// <param name="context"></param>
    private void OnEarthMovePerformed(InputAction.CallbackContext context)
    {
        if (context.control.device.deviceId == myDeviceID)
        {
            if (takinginput == false)
            {
                takinginput = true;

                Vector2 input;
                if (userSettingsManager.earthControlType == UserSettingsManager.ControlType.KEYBOARD)
                {
                    input = new Vector2(Keyboard.current.leftArrowKey.ReadValue() - Keyboard.current.rightArrowKey.ReadValue(),
                        Keyboard.current.upArrowKey.ReadValue() - Keyboard.current.downArrowKey.ReadValue());
                }
                else
                {
                    input = Gamepad.all[playerIndex].leftStick.ReadValue();
                    input.x *= -1;
                }
                this.GetComponent<playerMovement>().MovePlayer(input);
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
            this.GetComponent<EarthPlayer>().OnTreeSelected(context);
        }

    }

    private void OnPickFlower(InputAction.CallbackContext context)
    {
        // Before doing anything, we check to make sure that the current message came from the correct controller (i.e., that the sender's ID matches our saved ID)
        if (context.control.device.deviceId == myDeviceID)
        {
            this.GetComponent<EarthPlayer>().OnFlowerSelected(context);
        }

    }

    private void OnPickGrass(InputAction.CallbackContext context)
    {
        // Before doing anything, we check to make sure that the current message came from the correct controller (i.e., that the sender's ID matches our saved ID)
        if (context.control.device.deviceId == myDeviceID)
        {
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
            this.GetComponent<EarthPlayer>().OnInteract(context);
        }
    }

    private void OnHealPerformed(InputAction.CallbackContext context)
    {
        if (context.control.device.deviceId == myDeviceID)
        {
            earthPlayer.CastHealHandler();
        }
    }

    private void OnThornShieldPerformed(InputAction.CallbackContext context)
    {
        if (context.control.device.deviceId == myDeviceID)
        {
            earthPlayer.CastThornShieldHandler();
        }
    }

    /// <summary>
    /// DEBUG CONTROLS (DEFAULT)
    /// </summary>
    /// <param name="context"></param>

    /// <summary>
    /// PLANTING PLANT CONTROLS
    /// </summary>
    /// <param name="context"></param>
    private void OnPlantPlantedPerformed(InputAction.CallbackContext context)
    {
        if (context.control.device.deviceId == myDeviceID || (userSettingsManager.earthControlType == UserSettingsManager.ControlType.KEYBOARD && Mouse.current.leftButton.wasPressedThisFrame))
        {
            earthPlayer.PlantPlantingHandler();
        }
    }

    private void OnPlantingCancelledPerformed(InputAction.CallbackContext context)
    {
        if (context.control.device.deviceId == myDeviceID || (userSettingsManager.earthControlType == UserSettingsManager.ControlType.KEYBOARD && Mouse.current.rightButton.wasPressedThisFrame))
        {
            earthPlayer.OnPlantingCancelled();
        }
    }



    /// <summary>
    /// PLANT REMOVAL CONTROLS
    /// </summary>
    /// <param name="context"></param>
    private void OnPlantRemoved(InputAction.CallbackContext context)
    {
        if (context.control.device.deviceId == myDeviceID || (userSettingsManager.earthControlType == UserSettingsManager.ControlType.KEYBOARD && Mouse.current.leftButton.wasPressedThisFrame))
        {
            earthPlayer.PlantRemovingHandler();
        }
    }

    private void OnRemovingPlantCancelled(InputAction.CallbackContext context)
    {
        if (context.control.device.deviceId == myDeviceID || (userSettingsManager.earthControlType == UserSettingsManager.ControlType.KEYBOARD && Mouse.current.rightButton.wasPressedThisFrame))
        {
            earthPlayer.OnRemovingCancelled();
        }
    }



    /// <summary>
    /// HEAL AND BARRIER SELECTION CONTROLS
    /// </summary>
    /// <param name="context"></param>
    private void OnTargetSelected(InputAction.CallbackContext context)
    {
        if (context.control.device.deviceId == myDeviceID || (userSettingsManager.earthControlType == UserSettingsManager.ControlType.KEYBOARD && Mouse.current.leftButton.wasPressedThisFrame))
        {
            if (controls.HealSelect.enabled)
            {
                earthPlayer.InitiateHealing();
            }
            else
            {
                earthPlayer.InitiateBarrier();
            }
        }

    }


    private void OnTargetCycled(InputAction.CallbackContext context)
    {
        if (context.control.device.deviceId == myDeviceID)
        {
            float input;
            if (userSettingsManager.earthControlType == UserSettingsManager.ControlType.CONTROLLER)
            {
                input = Gamepad.all[playerIndex].leftTrigger.ReadValue();
                input += Gamepad.all[playerIndex].rightTrigger.ReadValue() * -2;
                if (input < 0)
                {
                    earthPlayer.OnCycleTargets(true);
                }
                else if (input > 0)
                {
                    earthPlayer.OnCycleTargets(false);
                }
            }
            else
            {
                input = Keyboard.current.leftArrowKey.ReadValue();
                input += Keyboard.current.rightArrowKey.ReadValue() * -2;
                if (input < 0)
                {
                    earthPlayer.OnCycleTargets(true);
                }
                else if (input > 0)
                {
                    earthPlayer.OnCycleTargets(false);
                }
            }

        }
    }


    private void OnHealCancelled(InputAction.CallbackContext context)
    {
        if (context.control.device.deviceId == myDeviceID || (userSettingsManager.earthControlType == UserSettingsManager.ControlType.KEYBOARD && Mouse.current.rightButton.wasPressedThisFrame))
        {
            earthPlayer.OnHealingCancelled();
        }
    }


    private void OnBarrierCancelled(InputAction.CallbackContext context)
    {
        if (context.control.device.deviceId == myDeviceID || (userSettingsManager.earthControlType == UserSettingsManager.ControlType.KEYBOARD && Mouse.current.rightButton.wasPressedThisFrame))
        {
            earthPlayer.OnBarrierCancelled();
        }
    }

    ///
    /// MISC
    ///
    private void OnPutNyssaDown(InputAction.CallbackContext context)
    {
        if (context.control.device.deviceId == myDeviceID)
        {
            earthPlayer.PutDownNyssa();
        }
    }

    /// <summary>
    /// MENU CONTROLS
    /// </summary>
    private void OnMenuSubmitPerformed(InputAction.CallbackContext context)
    {
        if(context.control.device.deviceId == myDeviceID)
        {
            if (userSettingsManager.earthControlType == UserSettingsManager.ControlType.CONTROLLER)
            {
                
            }
        }
    }

    ///
    ///
    ///
    private void OnNextSlideSelected(InputAction.CallbackContext context)
    {
        if (context.control.device.deviceId == myDeviceID)
        {
            cutsceneManager.NextImage();
        }
    }

    /// <summary>
    /// DIALOGUE CONTROLS
    /// </summary>
    /// <param name="context"></param>
    private void OnContinuePerformed(InputAction.CallbackContext context)
    {
        if (context.control.device.deviceId == myDeviceID)
        {
            float input;
            if (userSettingsManager.earthControlType == UserSettingsManager.ControlType.CONTROLLER)
            {
                input = Gamepad.all[playerIndex].buttonSouth.ReadValue();
            }
            else
            {
                input = Keyboard.current.enterKey.ReadValue();
            }
            if (input > 0)
            {
                dialogueManager.HandleDialogueContinue();
            }
        }
    }

    private void OnSkipPerformed(InputAction.CallbackContext context)
    {
        if (context.control.device.deviceId == myDeviceID)
        {
            dialogueManager.EndDialogue();
        }
    }

    
}
