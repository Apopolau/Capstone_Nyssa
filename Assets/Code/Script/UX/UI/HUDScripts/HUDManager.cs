using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class HUDManager : MonoBehaviour
{
    //Contains all of our UI element data
    [SerializeField] HUDModel model;

    //Global settings manager
    [SerializeField] private UserSettingsManager userSettingsManager;
    [Tooltip("Put the weather state data object here")]
    [SerializeField] private WeatherState weatherState;
    LevelEventManager levelEvents;

    public enum MouseCanvasType { MAIN, SPROUT };
    private MouseCanvasType currentCanvas = MouseCanvasType.MAIN;

    [SerializeField] private Canvas hudCanvas;

    [SerializeField] private GameObjectRuntimeSet playerSet;
    [SerializeField] private GameObjectRuntimeSet animalSet;
    [SerializeField] private GameObjectRuntimeSet buildSet;
    private CelestialPlayer celestialPlayer;
    private EarthPlayer earthPlayer;

    [SerializeField] private PowerStats coldSnapStats;
    [SerializeField] private PowerStats lightningStats;
    [SerializeField] private PowerStats basicAttackStats;
    [SerializeField] private PowerStats moonTideAttackStats;

    //These bools help track the state of the UI so they can be turned off and restored properly
    private bool inventoryIsOn = true;
    //private bool dialogueIsOn = false;
    private bool objectivesIsOn = false;
    //private bool menuIsOn = false;
    private bool pollutionIsOn = true;
    //private bool dayNightCycleIsOn = true;
    private bool popUpTextIsOn = false;

    private WaitForSeconds textTimer = new WaitForSeconds(3);
    private WaitForSeconds refreshTimer = new WaitForSeconds(0.5f);

    private Coroutine textDisplay = null;

    private bool celesteOccupied = false;
    private bool sproutOccupied = false;

    private void Awake()
    {
        model.SetManager(this);
        levelEvents = GetComponent<LevelEventManager>();
        PopulateUI();
        model.GetInventory().OnItemChange += OnPowerStateChange;
        levelEvents.SetInventorySlotManager(model.GetInventoryPanel().transform.GetChild(1).gameObject);
        model.GetInventoryPanel().transform.GetChild(1).gameObject.SetActive(true);
        levelEvents.SetObjectives(model.GetObjectives());
        //SetUI();
    }

    private void Start()
    {
        foreach (GameObject player in playerSet.Items)
        {
            if (player.GetComponent<CelestialPlayer>())
            {
                celestialPlayer = player.GetComponent<CelestialPlayer>();
                celestialPlayer.OnEnergyChanged += OnEnergyChanged;
                celestialPlayer.OnCooldownStarted += OnCooldownStarted;
                celestialPlayer.OnPowerStateChange += OnPowerStateChange;
                celestialPlayer.GetComponent<PowerBehaviour>().OnPowerStateChange += OnPowerStateChange;
            }
            else if (player.GetComponent<EarthPlayer>())
            {
                earthPlayer = player.GetComponent<EarthPlayer>();
                earthPlayer.OnCooldownStarted += OnCooldownStarted;
                earthPlayer.OnPowerStateChange += OnPowerStateChange;
            }
        }
        model.InitializeKidnapIcons();
        SetDialogueManagerControls(model.GetDialogueManager().GetComponent<DialogueManager>());
        SetMovementKeyIndicators();
        //StartCoroutine(UpdateCelestePowerOverlay());
        //StartCoroutine(UpdateEnergyBar());
        //StartCoroutine(UpdateSeedQuantityText());
        OnPowerStateChange();
    }

    private void Update()
    {
        UpdateCooldowns();
    }

    /// <summary>
    /// STARTUP FUNCTIONS FOR INITIAL SETTINGS
    /// </summary>

    //Called on level start in order to create all UI elements
    void PopulateUI()
    {
        //INSTANTIATE ELEMENTS

        model.SetCanvas(hudCanvas);

        //Create the UI assets on the HUD
        model.InitializeObjectives(levelEvents);
        model.InitializeInventory(levelEvents.GetProgress().GetInventory());
        model.InitializeVirtualMouseCanvas();
        model.InitializeDialogueManager();
        model.InitializePollutionBar();
        model.InitializeDayNightCycle();
        model.InitializeEnergyBar();
        model.InitializePopUpText();
        model.InitializeMenuKeyGuide();
        

        //Create the UI assets for player controls
        model.InitializePowers();
        //model.InitializeCelesteControls();
        //model.InitializeSproutControls();
    }

    //Checks the user's language settings and switches the active UI accordingly
    public void ActivateUIBasedOnLanguage()
    {
        if (userSettingsManager.chosenLanguage == UserSettingsManager.GameLanguage.ENGLISH)
        {
            foreach (GameObject go in model.GetEnglishUIElements())
            {
                go.SetActive(true);
            }
            foreach (GameObject go in model.GetFrenchUIElements())
            {
                go.SetActive(false);
            }

        }
        else if (userSettingsManager.chosenLanguage == UserSettingsManager.GameLanguage.FRENCH)
        {
            foreach (GameObject go in model.GetFrenchUIElements())
            {
                go.SetActive(true);
            }
            foreach (GameObject go in model.GetEnglishUIElements())
            {
                go.SetActive(false);
            }
        }
    }

    //Sets whether the controller or keyboard movement keys appear above the characters' heads
    private void SetMovementKeyIndicators()
    {
        if (userSettingsManager.celestialControlType == UserSettingsManager.ControlType.KEYBOARD)
        {
            celestialPlayer.GetKeyboardMovementCtrls().SetActive(true);
            celestialPlayer.GetControllerMovementCtrls().SetActive(false);
        }
        else
        {
            celestialPlayer.GetKeyboardMovementCtrls().SetActive(false);
            celestialPlayer.GetControllerMovementCtrls().SetActive(true);
        }
        if (userSettingsManager.earthControlType == UserSettingsManager.ControlType.KEYBOARD)
        {
            earthPlayer.GetKeyboardMovementCtrls().SetActive(true);
            earthPlayer.GetControllerMovementCtrls().SetActive(false);
        }
        else
        {
            earthPlayer.GetKeyboardMovementCtrls().SetActive(false);
            earthPlayer.GetControllerMovementCtrls().SetActive(true);
        }
    }

    //Set Celeste's powers based on what she's unlocked so far


    /// <summary>
    /// FUNCTIONS FOR TURNING VARIOUS UI ELEMENTS ON OR OFF
    /// </summary>

    //Hides or restores all UI elements for dialogue. If active, dialogue is on
    public void ToggleUIForDialogue(bool isActive)
    {
        if (isActive)
        {
            //If inventory is on, turn it off
            if (model.GetInventoryPanel().activeSelf)
            {
                ToggleInventoryPanel(false);
            }
            //If objectives are on, turn them off
            if (model.GetObjectives().activeSelf)
            {
                ToggleObjectivePanel(false);
            }
            //If Sprout's UI is on, turn it off
            if (model.GetActiveSproutUI().activeSelf)
            {
                model.GetActiveSproutUI().SetActive(false);
            }
            //If Celeste's UI is on, turn it off
            if (model.GetActiveCelesteUI().activeSelf)
            {
                model.GetEnergyBar().SetActive(false);
                model.GetActiveCelesteUI().SetActive(false);
            }
            //If the pollution bar is on, turn it off
            if (model.GetPollutionBar().activeSelf)
            {
                model.GetPollutionBar().SetActive(false);
            }
            //Make sure there's no popup text appearing over the dialogue
            if (model.GetPopUpTextBox().activeSelf)
            {
                model.GetPopUpTextBox().SetActive(false);
            }
            //UNCOMMENT THIS WHEN WE HAVE A DAY NIGHT CYCLE UI
            /*
            //If the day night cycle UI is on, turn it off
            if (model.GetDayNightCycle().activeSelf)
            {
                model.GetDayNightCycle().SetActive(false);
            }
            */
        }
        //We want to restore UI elements that were on before dialogue started
        if (!isActive)
        {
            //If for whatever reason these elements were supposed to be disabled, we don't want to turn them back on after
            if (inventoryIsOn)
            {
                ToggleInventoryPanel(true);
            }
            if (objectivesIsOn)
            {
                ToggleObjectivePanel(true);
            }
            if (pollutionIsOn)
            {
                model.GetPollutionBar().SetActive(true);
            }
            /*
            if (dayNightCycleIsOn)
            {
                model.GetDayNightCycle().SetActive(true);
            }
            */
            if (popUpTextIsOn)
            {
                model.GetPopUpTextBox().SetActive(true);
            }
            model.GetActiveCelesteUI().SetActive(true);
            model.GetEnergyBar().SetActive(true);
            model.GetActiveSproutUI().SetActive(true);
        }
        //dialogueIsOn = isActive;

    }

    //Hides or restores the inventory panel
    private void ToggleInventoryPanel(bool isActive)
    {
        if (isActive)
        {
            Image[] inventoryImages = model.GetInventoryPanel().GetComponentsInChildren<Image>();

            foreach (Image image in inventoryImages)
            {
                if (image.GetComponent<ItemSlot>())
                {
                    if (image.GetComponent<ItemSlot>().Item != null)
                    {
                        image.enabled = true;

                    }
                }
                else
                {
                    image.enabled = true;
                }
            }
            TextMeshProUGUI[] text = model.GetInventoryPanel().GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI txt in text)
            {
                if (txt.text != "")
                {
                    txt.enabled = true;
                    //txt.gameObject.SetActive(true);
                }
            }

        }
        else if (!isActive)
        {
            Image[] inventoryImages = model.GetInventoryPanel().GetComponentsInChildren<Image>();

            foreach (Image image in inventoryImages)
            {
                if (image.GetComponent<ItemSlot>())
                {
                    if (image.GetComponent<ItemSlot>().Item != null)
                    {
                        image.enabled = false;

                    }
                }
                else
                {
                    image.enabled = false;
                }
            }
            TextMeshProUGUI[] text = model.GetInventoryPanel().GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI txt in text)
            {
                if (txt.text != "")
                {
                    txt.enabled = false;
                    //txt.gameObject.SetActive(false);
                }
            }

        }
    }

    //Hides or restores the objective panel
    private void ToggleObjectivePanel(bool isActive)
    {
        if (isActive)
        {
            model.GetObjectives().SetActive(true);
            //objectivesIsOn = true;
        }
        else if (!isActive)
        {
            model.GetObjectives().SetActive(false);
            //objectivesIsOn = false;
        }
    }

    //Makes a box of text appear on-screen to inform the player of something for a set period of time
    public void ThrowPlayerWarning(string incText)
    {
        model.SetPopUpText(incText);
        model.GetPopUpTextBox().SetActive(true);
        popUpTextIsOn = true;
        if (textDisplay != null)
        {
            StopCoroutine(textDisplay);
        }
        textDisplay = StartCoroutine(TimedTurnOffPopUpText());
    }

    //Makes a box of text appear on-screen to inform the player of something for no set period of time
    public void TurnOnPopUpText(string incText)
    {
        model.SetPopUpText(incText);
        model.GetPopUpTextBox().SetActive(true);
        popUpTextIsOn = true;
    }

    //Makes the popup text box disappear
    public void TurnOffPopUpText()
    {
        model.GetPopUpTextBox().SetActive(false);
        model.SetPopUpText("");
        popUpTextIsOn = false;
    }

    //Turns the text box off after the time has passed
    private IEnumerator TimedTurnOffPopUpText()
    {
        yield return textTimer;
        model.GetPopUpTextBox().SetActive(false);
        model.SetPopUpText("");
        popUpTextIsOn = false;
    }

    //Switches which cursor is activated, used when switching between split screen and single camera
    public void SwitchCursorCanvas(MouseCanvasType mouseCanvasType, bool isActive)
    {
        VirtualMouseInput inputToUse;
        GameObject uiToUse;
        ToggleVirtualMouseSprite(false);
        if (mouseCanvasType == MouseCanvasType.SPROUT)
        {
            inputToUse = model.GetSproutVirtualMouseInput();
            uiToUse = model.GetSproutVirtualMouseUI();
        }
        else //(mouseCanvasType == MouseCanvasType.MAIN && currentCanvas == MouseCanvasType.SPROUT)
        {
            inputToUse = model.GetMainVirtualMouseInput();
            uiToUse = model.GetMainVirtualMouseUI();
            //inputToUse.cursorTransform.position = GetVirtualMousePosition();
        }

        inputToUse.cursorTransform.position = GetVirtualMousePosition();
        model.SetActiveVirtualMouseUI(uiToUse);
        model.SetVirtualMouseInput(inputToUse);
        //SetVirtualMouseImage(model.GetVirtualMouseImage().sprite);

        if (isActive)
        {
            ToggleVirtualMouseSprite(true);
        }

        currentCanvas = mouseCanvasType;
    }



    /// <summary>
    /// FUNCTIONS FOR DARKENING OR RESTORING CERTAIN UI ELEMENTS
    /// </summary>

    //Turns the overlay on or off all of Sprout's powers
    public void ToggleSproutPanel(bool on)
    {
        /*
        foreach (GameObject overlay in model.GetSproutOverlays())
        {
            overlay.transform.GetChild(0).gameObject.SetActive(on);
        }
        */
        OnPowerStateChange();
    }

    //Turns the overlay on or off all of Celeste's powers
    public void ToggleCelestePanel(bool on)
    {
        /*
        foreach (GameObject overlay in model.GetCelesteOverlays())
        {
            overlay.transform.GetChild(0).gameObject.SetActive(on);
        }
        */
        OnPowerStateChange();
    }

    //Turn off the overlay on a single power display
    public void TurnOnPowerOverlay(string hashName)
    {
        model.GetOverlayTable()[hashName].GetComponent<PowerButtonHandler>().ToggleOverlay(true);
    }

    /*
    private void TurnOnPowerOverlay(string hashName, float timer)
    {
        model.GetOverlayTable()[hashName].GetComponent<PowerButtonHandler>().ToggleOverlay(timer);
    }
    */

    //Turns off the overlay to restore ability brightness
    public void TurnOffPowerOverlay(string hashName)
    {
        model.GetOverlayTable()[hashName].GetComponent<PowerButtonHandler>().ToggleOverlay(false);
    }



    /// <summary>
    /// FUNCTIONS FOR RUNNING COOLDOWN ANIMATIONS
    /// </summary>


    //Initiates the cooldown un-fill for a particular power using its hash code
    public void OnCooldownStarted(string hashCode, float length)
    {
        Debug.Log("Started cooldown for " + hashCode + "power for " + length + " seconds.");
        StartCoroutine(RunCooldown(hashCode, length));
    }

    //Lets the overlay un-fill in a radial pattern to indicate a cooldown timer
    public IEnumerator RunCooldown(string hashcode, float cooldown)
    {
        //Set our fill to full to start with, just to be sure
        Image fillImage = model.GetOverlayTable()[hashcode].transform.GetChild(0).GetComponent<Image>();
        fillImage.fillAmount = 1;

        //Set the appropriate cooldown overlay's time stamp and cooldown state
        model.GetCooldownStateTable()[hashcode] = true;
        model.GetCooldownTimeTable()[hashcode] = cooldown;
        model.GetCooldownProgress()[hashcode] = 0;

        //Debug.Log("Starting cooldown for " + hashcode);

        yield return new WaitForSeconds(cooldown);

        // Reset fill back to full amount
        fillImage.fillAmount = 1;

        //Reset our cooldown data
        model.GetCooldownStateTable()[hashcode] = false;
        model.GetCooldownTimeTable()[hashcode] = -1;
        model.GetCooldownProgress()[hashcode] = -1;
    }

    private void CooldownImageFill(string powerName)
    {
        Image fillImage = model.GetOverlayTable()[powerName].transform.GetChild(0).GetComponent<Image>();
        float length = model.GetCooldownTimeTable()[powerName];
        float timer = model.GetCooldownProgress()[powerName];

        //Put all of the fill data here and run it from an update function for each power that is on cooldown
        if (timer < length)
        {
            float fillAmount = Mathf.Clamp(1 - (1 / (length / timer)), 0, 1);
            fillImage.fillAmount = fillAmount;
            model.GetCooldownProgress()[powerName] += Time.deltaTime;
        }
        else
        {
            fillImage.fillAmount = 0;
        }

    }

    private void UpdateCooldowns()
    {
        foreach(string powerName in model.GetOverlayTable().Keys)
        {
            if(model.GetCooldownStateTable()[powerName] == true)
            {
                CooldownImageFill(powerName);
            }
        }
    }

    /// <summary>
    /// FUNCTIONS FOR UPDATING THE UI
    /// </summary>

    //////////GENERAL

    //Update pollution meter
    public void UpdatePollutionDisplay(float pollutionPercent)
    {
        if (model.GetPollutionBar() != null)
        {
            model.GetPollutionBar().transform.GetChild(0).GetComponent<Image>().fillAmount = pollutionPercent;

            pollutionPercent *= 100;

            if (pollutionPercent >= model.GetAcidRainThreshold() && pollutionPercent < model.GetAcidRainHardThreshold())
            {
                weatherState.SetAcidState(WeatherState.AcidRainState.LIGHT);
                model.SetEarthIcon(model.GetSadEarthIcon());
            }
            else if (pollutionPercent >= model.GetAcidRainHardThreshold())
            {
                weatherState.SetAcidState(WeatherState.AcidRainState.HEAVY);
                model.SetEarthIcon(model.GetDesperateEarthIcon());
            }
            else
            {
                weatherState.SetAcidState(WeatherState.AcidRainState.NONE);
                model.SetEarthIcon(model.GetHappyEarthIcon());
            }
        }

    }

    //NEEDS SETUP
    //Update Day/Night circle
    public void UpdateDayNightDisplay()
    {
        //This will need to spin the tab on the day night cycle once it's created
    }

    //NEEDS HEAVILY DEBUGGED
    //Moves a kidnap icon to try to get as close to an animal that's being kidnapped as possible while staying on-screen
    public void MoveKidnapIcon(GameObject kidnapIcon, GameObject uiTarget, GameObject animal)
    {
        
        //float xPos = Mathf.Clamp(120, 0f, Screen.width - 120);
        //float yPos = Mathf.Clamp(120, 0f, Screen.height - 120);
        //kidnapIcon.transform.position = new Vector3(xPos, yPos, 0);

        //Calculate the direction between the camera and the animal
        Vector3 toPosition = animal.transform.position;
        Vector3 fromPosition = Camera.main.transform.position;
        fromPosition.z = 0f;

        Vector3 dir = (toPosition - fromPosition).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (angle < 0)
            angle += 360;

        kidnapIcon.transform.GetChild(0).gameObject.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, angle);

        //Move the kidnap icon to the closest point in the screen space to the animal's UI target
        Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(uiTarget.transform.position);

        float borderSize = 100f;

        bool isOffScreen = targetPositionScreenPoint.x <= borderSize || targetPositionScreenPoint.x >= Screen.width - borderSize || targetPositionScreenPoint.y <= borderSize || targetPositionScreenPoint.y >= Screen.height - borderSize;

        //If the position it would be at falls off of the edge of the visible screen
        if (isOffScreen)
        {
            

            Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;
            if (cappedTargetScreenPosition.x <= 0) cappedTargetScreenPosition.x = borderSize;
            if (cappedTargetScreenPosition.x >= Screen.width) cappedTargetScreenPosition.x = Screen.width - borderSize;
            if (cappedTargetScreenPosition.y <= 0) cappedTargetScreenPosition.y = borderSize;
            if (cappedTargetScreenPosition.y >= Screen.height) cappedTargetScreenPosition.y = Screen.height - borderSize;

            Vector3 pointerWorldPosition = Camera.main.ScreenToWorldPoint(cappedTargetScreenPosition);
            kidnapIcon.transform.position = pointerWorldPosition;
            kidnapIcon.transform.localPosition = new Vector3(kidnapIcon.transform.localPosition.x, kidnapIcon.transform.localPosition.y, 0);
        }
        //Vector3 pointToRotateTowards = new Vector3(kidnapIcon.transform.position.x, kidnapIcon.transform.position.y, animal.transform.position.x);

        //kidnapIcon.transform.GetChild(0).LookAt(pointToRotateTowards);

    }

    private void OnPowerStateChange()
    {
        UpdateCelestePowerOverlay();
        UpdateSproutPowerOverlay();
    }

    //////////CELESTE

    //Sets whether Celeste's powers should be lit up or not based on circumstance
    public void UpdateCelestePowerOverlay()
    {
        //Checks that Celeste's UI isn't disabled on account of being in the middle of something
        if (!celesteOccupied)
        {
            //We can cast rain as long as we have energy
            if(celestialPlayer.GetEnergy() > 0)
            {
                TurnOffPowerOverlay("CastRain");
            }
            else
            {
                TurnOnPowerOverlay("CastRain");
            }
            //Check if basic attack is available, turn overlay on or off as needed
            if (celestialPlayer.CheckIfCastable(basicAttackStats, true))
            {
                TurnOffPowerOverlay("BasicAttack");
            }
            else
            {
                TurnOnPowerOverlay("BasicAttack");
            }

            if (celestialPlayer.CheckIfCastable(coldSnapStats, true))
            {
                TurnOffPowerOverlay("CastCold");
            }
            else
            {
                TurnOnPowerOverlay("CastCold");
            }

            if (celestialPlayer.CheckIfCastable(lightningStats, true))
            {
                TurnOffPowerOverlay("CastThunder");
            }
            else
            {
                TurnOnPowerOverlay("CastThunder");
            }

            if (celestialPlayer.CheckIfCastable(moonTideAttackStats, true))
            {
                TurnOffPowerOverlay("CastMoontide");
            }
            else
            {
                TurnOnPowerOverlay("CastMoontide");
            }
        }
        else
        {
            TurnOnPowerOverlay("CastRain");
            TurnOnPowerOverlay("BasicAttack");
            TurnOnPowerOverlay("CastCold");
            TurnOnPowerOverlay("CastThunder");
            TurnOnPowerOverlay("CastMoontide");
        }
    }

    private void OnEnergyChanged(int currentEnergy, int maxEnergy)
    {
        UpdateEnergyBar(currentEnergy, maxEnergy);
    }

    //Keeps Celeste's energy bar fill up-to-date
    private void UpdateEnergyBar(int currentEnergy, int maxEnergy)
    {

        //Debug.Log("Current energy: " + celestialPlayer.energy.current);

        // Find the energy bar fill Image component dynamically
        if (model.GetEnergyBar() != null)
        {
            Image fillImage = model.GetEnergyBar().transform.GetChild(0).GetComponent<Image>(); //The first child of the energy bar is the fill bar
            if (fillImage != null)
            {
                // Calculate the new fill amount
                float fillAmount = (float)currentEnergy / maxEnergy; // Assuming energy bar's max value is 100
                fillImage.fillAmount = Mathf.Clamp01(fillAmount); // Clamp fill amount between 0 and 1
            }
        }


    }

    //////////SPROUT

    public void UpdateSproutPowerOverlay()
    {
        UpdateSeedQuantityText();
        if (buildSet.Items.Count == 0 || sproutOccupied)
        {
            // Activate the overlay if Build Count is 0
            TurnOnPowerOverlay("RemovePlant");
        }
        else
        {
            // Deactivate the UI if the build set count is greater than 0
            TurnOffPowerOverlay("RemovePlant");
        }
        UpdateSproutSpellOverlay();
        ChangeSproutPanelByState();
    }

    public void UpdateSproutSpellOverlay()
    {
        if (!sproutOccupied)
        {
            if (!earthPlayer.GetBarrierOnCooldown())
            {
                TurnOffPowerOverlay("CastBarrier");
            }
            else
            {
                TurnOnPowerOverlay("CastBarrier");
            }
            if (!earthPlayer.GetHealOnCooldown())
            {
                TurnOffPowerOverlay("CastHeal");
            }
            else
            {
                TurnOnPowerOverlay("CastHeal");
            }
        }
        else
        {
            TurnOnPowerOverlay("CastHeal");
            TurnOnPowerOverlay("CastBarrier");
        }
    }

    //Sets whether Sprout's Planting abilities should be lit up or not based on circumstance
    public void UpdateSeedQuantityText()
    {
        // Update the quantity text with the item's quantity.
        if (model.GetInventory() != null && model.GetInventoryPanel() != null)
        {
            for (int i = 0; i < 3; i++)
            {
                PowerButtonHandler button = model.GetSproutOverlays()[i].GetComponent<PowerButtonHandler>();
                UpdateIndividualSeedText(button);
            }
        }
        

    }

    //Updates the number value displayed on each power to show how many seeds Sprout has of a particular type
    private void UpdateIndividualSeedText(PowerButtonHandler button)
    {
        string hashName = button.GetHashName();
        int quantity = model.GetInventory().GetQuantityByItemType(button.GetItemName());
        button.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{quantity}";
        //quantityText.text = $"{quantity}";

        // Check if the quantity is above 0
        if (quantity > 0 && !sproutOccupied)
        {
            // Activate the UI element based on the itemType
            TurnOffPowerOverlay(hashName);
        }
        else if (quantity == 0 || sproutOccupied)
        {
            TurnOnPowerOverlay(hashName);
        }
    }

    private void ChangeSproutPanelByState()
    {
        if (earthPlayer.GetInBarrierSelection())
        {
            TurnOnPowerOverlay("CastHeal");
            TurnOnPowerOverlay("PlantTree");
            TurnOnPowerOverlay("PlantFlower");
            TurnOnPowerOverlay("PlantGrass");
            TurnOnPowerOverlay("RemovePlant");
        }

        if (earthPlayer.GetInHealSelection())
        {
            TurnOnPowerOverlay("CastBarrier");
            TurnOnPowerOverlay("PlantTree");
            TurnOnPowerOverlay("PlantFlower");
            TurnOnPowerOverlay("PlantGrass");
            TurnOnPowerOverlay("RemovePlant");
        }

        if (earthPlayer.GetInPlantSelection())
        {
            TurnOnPowerOverlay("CastHeal");
            TurnOnPowerOverlay("CastBarrier");
            if(earthPlayer.plantSelectedType == EarthPlayer.PlantSelectedType.TREE)
            {
                TurnOnPowerOverlay("PlantFlower");
                TurnOnPowerOverlay("PlantGrass");
            }
            else if(earthPlayer.plantSelectedType == EarthPlayer.PlantSelectedType.FLOWER)
            {
                TurnOnPowerOverlay("PlantTree");
                TurnOnPowerOverlay("PlantGrass");
            }
            else if(earthPlayer.plantSelectedType == EarthPlayer.PlantSelectedType.GRASS)
            {
                TurnOnPowerOverlay("PlantFlower");
                TurnOnPowerOverlay("PlantTree");
            }
        }
    }

    //Change the position of the virtual mouse based on current mouse
    public void MoveVirtualMouse(UserSettingsManager.ControlType controlType)
    {
        if (model.GetVirtualMouseInput() != null)
        {
            if (controlType == UserSettingsManager.ControlType.CONTROLLER)
            {
                model.GetVirtualMouseInput().cursorTransform.position = model.GetVirtualMouseInput().virtualMouse.position.value;
                model.SetVirtualMousePosition(model.GetVirtualMouseInput().cursorTransform.position);
            }
            else if (controlType == UserSettingsManager.ControlType.KEYBOARD)
            {
                model.GetVirtualMouseInput().cursorTransform.position = Mouse.current.position.value;
                model.SetVirtualMousePosition(Mouse.current.position.value);
            }
        }

    }



    /// <summary>
    /// CONTROL STATE CHANGERS
    /// </summary>
    /// <returns></returns>

    ////////GENERAL DATA
    
    //Used to set the current general state of objectives. Use when objectives are enabled at the start of the mission
    public void ToggleObjectivesState(bool on)
    {
        objectivesIsOn = on;
    }

    ////////SPROUT DATA

    //Used to declare that Sprout is occupied; her powers shouldn't be lit up when in this state
    public void SetSproutOccupied(bool isOccupied)
    {
        sproutOccupied = isOccupied;
    }

    ////////CELESTE DATA

    //Used to declare that Celeste is occupied; her powers shouldn't be lit up when in this state
    public void SetCelesteOccupied(bool isOccupied)
    {
        celesteOccupied = isOccupied;
    }




    /// <summary>
    /// GETTERS AND SETTERS
    /// </summary>
    /// <returns></returns>

    ////////GENERAL UI DATA

    //Returns the main Objectives UI object instance
    public GameObject GetObjectivesContainer()
    {
        return model.GetObjectives();
    }

    //Change a particular task to active or inactive
    public void ToggleTask(int index, bool active)
    {
        model.GetTaskData()[index].SetTaskActive(active);
        model.GetTask(index).SetActive(active);
    }

    //Returns the InventorySlotManager script instance that populates the inventory with inventory slots
    public GameObject GetInventorySlotManager()
    {
        return model.GetInventoryPanel().transform.GetChild(1).gameObject;
    }

    public void SetDialogueManagerControls(DialogueManager dialogueManager)
    {
        celestialPlayer.GetComponent<CelestialPlayerControls>().SetDialogueManager(dialogueManager);
        earthPlayer.GetComponent<EarthPlayerControl>().SetDialogueManager(dialogueManager);
    }

    //Returns the hudManager's level manager for this level
    public LevelEventManager GetLevelManager()
    {
        return levelEvents;
    }

    ////////SPROUT DATA

    //Turns the virtual mouse sprite on and off
    public void ToggleVirtualMouseSprite(bool isActive)
    {
        //Debug.Log("Toggling virtual mouse sprite to " + isActive);
        model.GetActiveVirtualMouseUI().transform.GetChild(0).gameObject.GetComponent<Image>().enabled = isActive;
    }

    //Get the current location of the virtual mouse
    public Vector3 GetVirtualMousePosition()
    {
        return model.GetVirtualMousePosition();
    }

    //Move the virtual mouse to a specific position on the canvas
    public void SetVirtualMousePosition(Vector3 incPos)
    {
        model.SetVirtualMousePosition(incPos);
    }

    //Set the image being used by the virtual cursor
    public void SetVirtualMouseImage(Sprite sprite)
    {
        model.SetVirtualMouseImage(sprite);
    }

    //Get our currently used virtual mouse input
    public VirtualMouseInput GetVirtualMouseInput()
    {
        return model.GetVirtualMouseInput();
    }

    //Sets the canvas object from Sprout's object
    public void SetSproutVirtualMouseUI(GameObject incMouseUI)
    {
        model.SetSproutVirtualMouseUI(incMouseUI);
    }

    

    public List<GameObject> GetAnimalSet()
    {
        return animalSet.Items;
    }
}
