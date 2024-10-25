using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

[CreateAssetMenu(fileName = "New HUD Model Object", menuName = "Manager Object/HUDModel Object")]
public class HUDModel : ScriptableObject
{
    [Header("Data management objects")]
    [Tooltip("Put the Inventory data object here")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private UserSettingsManager userSettingsManager;
    private Canvas hudCanvas; //Make sure this gets set by the HUDManager at runtime
    private HUDManager manager;

    //This data is prefabs that need to be assigned to the scriptable object
    //If needed, they follow with references to the actual instance that are assigned when initialized
    [Header("Individual UI elements")]
    [Tooltip("Drag Objectives prefab here")]
    [SerializeField] private GameObject objectivesPrefab;
    private GameObject objectives;
    [Tooltip("Drag the task prefab here")]
    [SerializeField] private GameObject taskPrefab;
    [Tooltip("Drag each task for this mission here")]
    [SerializeField] private TaskData[] taskTexts;
    private GameObject[] tasks;
    [Tooltip("Drag InventoryPanel prefab here")]
    [SerializeField] private GameObject inventoryPanelPrefab;
    private GameObject inventoryPanel;
    [Tooltip("Drag VirtualMouse Canvas prefab here")]
    [SerializeField] private GameObject virtualMouseUIPrefab;
    private GameObject virtualMouseUI;
    private GameObject virtualMouseUIMain;
    private GameObject virtualMouseUISprout;
    private VirtualMouseInput virtualMouseInput;
    private VirtualMouseInput virtualMouseInputMain;
    private VirtualMouseInput virtualMouseInputSprout;
    private Image cursorImage;
    [Tooltip("Drag Dialogue Manager prefab here")]
    [SerializeField] private GameObject dialogueManagerPrefab;
    private GameObject dialogueManager;
    private List<DialogueTrigger> dialogueTriggers;
    [Tooltip("Drag PollutionBar prefab here")]
    [SerializeField] private GameObject pollutionBarPrefab;
    private GameObject pollutionBar;
    [Tooltip("Drag DayNightCycle prefab here")]
    [SerializeField] private GameObject dayNightCyclePrefab;
    private GameObject dayNightCycle;
    [Tooltip("Drag popup text box prefab here")]
    [SerializeField] private GameObject popUpTextBoxPrefab;
    private GameObject popUpTextBox;
    [Tooltip("Drag the animal pointer prefab here")]
    [SerializeField] private GameObject animalPointerPrefab;
    [SerializeField] private GameObject[] animalPointers;
    [Tooltip("Drag the menu UI key indicator prefab here")]
    [SerializeField] private GameObject menuKeyPrefab;
    //This data is populated at runtime
    private GameObject[] englishUI;
    private GameObject[] frenchUI;
    private Dictionary<string, GameObject> powerOverlays;
    private Dictionary<string, bool> powerCooldownStates;
    private Dictionary<string, float> powerCooldownTime;
    private Dictionary<string, float> powerCooldownProgress;
    private List<GameObject> overlayComponents;

    [Header("Celeste's UI")]
    //Celeste's power buttons
    [Tooltip("Panel prefab for Celeste's powers on Keyboard")]
    [SerializeField] private GameObject celesteKeyboardPanel;
    [Tooltip("Panel prefab for Celeste's powers on Controller")]
    [SerializeField] private GameObject celesteControllerPanel;
    [Tooltip("Movement control guide prefab for Celeste on Keyboard")]
    [SerializeField] private GameObject moveCelesteKeyboard;
    [Tooltip("Movement control guide prefab for Celeste on Controller")]
    [SerializeField] private GameObject moveCelesteController;
    [Tooltip("Drag EnergyBar prefab here")]
    [SerializeField] private GameObject energyBarPrefab;
    private GameObject energyBar;
    [SerializeField] private GameObject energyNode;
    //This data is populated at runtime
    private GameObject activeCelesteUI;
    [SerializeField] private GameObject[] celesteAbilityOverlays = new GameObject[celestePowerCount];

    [Header("Sprout's UI")]
    //Celeste's power buttons
    [Tooltip("Panel for Sprout's powers on Keyboard")]
    [SerializeField] private GameObject sproutKeyboardPanel;
    [Tooltip("Panel for Sprout's powers on Controller")]
    [SerializeField] private GameObject sproutControllerPanel;
    [Tooltip("Movement control guide for Sprout on Keyboard")]
    [SerializeField] private GameObject moveSproutKeyboard;
    [Tooltip("Movement control guide for Sprout on Controller")]
    [SerializeField] private GameObject moveSproutController;
    //This data is populated at runtime
    [SerializeField] private GameObject activeSproutUI;
    [SerializeField] private GameObject[] sproutAbilityComponents = new GameObject[sproutPowerCount];

    [Header("Other Data")]
    //Colour used to tint abilities darker when they're unavailable
    private Color darkenedColour = new Color(0.5f, 0.5f, 0.5f);
    private Vector3 virtualMousePosition;
    //The total number of powers each character has
    private const int celestePowerCount = 5;
    private const int sproutPowerCount = 6;

    [Header("Pollution stats")]
    //Can turn a lot of these into consts after we test more to make sure these numbers are good
    [SerializeField] private int basePollutionLevel;
    [SerializeField] private int maxPollution;
    [SerializeField] private float acidRainThreshold;
    [SerializeField] private float acidRainHardThreshold;
    //Used to modify rain colour
    [SerializeField] private Color c_cleanRain;
    [SerializeField] private Color c_lightAcid;
    [SerializeField] private Color c_heavyAcid;

    [Header("HUD Sprites")]
    private Image earthIcon;
    //Earth icon sprites
    [SerializeField] private Sprite happyLevelImage;
    [SerializeField] private Sprite sadLevelImage;
    [SerializeField] private Sprite desperateLevelImage;

    public void OnEnable()
    {
        dialogueTriggers = new List<DialogueTrigger>();
        if(overlayComponents != null)
        {
            overlayComponents.Clear();
        }
        overlayComponents = new List<GameObject>();
        powerOverlays = new Dictionary<string, GameObject>();
        powerCooldownStates = new Dictionary<string, bool>();
        powerCooldownTime = new Dictionary<string, float>();
        powerCooldownProgress = new Dictionary<string, float>();
    }


    /// <summary>
    /// MODEL'S DATA REFERENCES
    /// </summary>
    /// <returns></returns>

    //Returns the currently assigned inventory
    public Inventory GetInventory()
    {
        return inventory;
    }

    public void SetInventory(Inventory incInventory)
    {
        inventory = incInventory;
    }

    //Sets the HUD's primary canvas at runtime so it gets updated every level
    public void SetCanvas(Canvas incCanvas)
    {
        hudCanvas = incCanvas;
    }

    /// <summary>
    /// INITIALIZATION FUNCTIONS
    /// </summary>
    /// <returns></returns>
    
    //Objectives initialization
    public void InitializeObjectives(LevelEventManager levelEvents)
    {
        objectives = Instantiate(objectivesPrefab, hudCanvas.transform);
        tasks = null;
        PopulateObjectivePanel(userSettingsManager.chosenLanguage, levelEvents);
        objectives.SetActive(false);
    }

    //Creates and sets up the inventory panel in the UI
    public void InitializeInventory(Inventory incInventory)
    {
        SetInventory(incInventory);
        inventoryPanel = Instantiate(inventoryPanelPrefab, hudCanvas.transform);
        manager.GetInventorySlotManager().GetComponent<InventorySlotManager>().SetInventory(incInventory);
        manager.GetLevelManager().PopulateInventory();
    }

    //Creates and sets up the main Virtual Mouse Canvas
    public void InitializeVirtualMouseCanvas()
    {
        virtualMouseUIMain = Instantiate(virtualMouseUIPrefab, hudCanvas.transform);
        virtualMouseUI = virtualMouseUIMain;
        cursorImage = virtualMouseUI.gameObject.GetComponentInChildren<Image>();
        cursorImage.enabled = false;
        virtualMouseInputMain = virtualMouseUI.GetComponent<VirtualMouseInput>();
        virtualMouseInput = virtualMouseInputMain;
        
        //virtualMouseUI.GetComponent<VirtualMouseUI>().SetRectTransform(hudCanvas.GetComponent<RectTransform>());
        virtualMousePosition = new Vector3();
    }

    //Creates and sets up the Dialogue Box UI
    public void InitializeDialogueManager()
    {
        dialogueManager = Instantiate(dialogueManagerPrefab, hudCanvas.transform);
        manager.GetLevelManager().SetDialogueManager(dialogueManager);
        dialogueManager.GetComponent<DialogueManager>().SetHudManager(manager);
        
        foreach(DialogueTrigger trigger in dialogueTriggers)
        {
            trigger.SetDialogueManager(dialogueManager.GetComponent<DialogueManager>());
        }
    }

    //Creates and sets up the Pollution Bar UI
    public void InitializePollutionBar()
    {
        pollutionBar = Instantiate(pollutionBarPrefab, hudCanvas.transform);
        earthIcon = pollutionBar.transform.GetChild(1).GetComponent<Image>();
    }

    //Creates and sets up the Day/Night Cycle UI
    public void InitializeDayNightCycle()
    {
        //Put the day night cycle here
    }

    //Creates and sets up Celeste's Energy Bar UI
    public void InitializeEnergyBar()
    {
        energyBar = Instantiate(energyBarPrefab, hudCanvas.transform);
        if (energyBar != null)
        {
            Image fillImage = energyBar.transform.GetChild(0).GetComponent<Image>(); //The first child of the energy bar is the fill bar
            if (fillImage != null)
            {
                // Calculate the new fill amount
                float fillAmount = (float)50 / 100; // Assuming energy bar's max value is 100
                fillImage.fillAmount = Mathf.Clamp01(fillAmount); // Clamp fill amount between 0 and 1
            }
            energyBar.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = 50.ToString();
        }
    }

    //Creates and sets up the player pop up text box
    public void InitializePopUpText()
    {
        popUpTextBox = Instantiate(popUpTextBoxPrefab, hudCanvas.transform);
        popUpTextBox.SetActive(false);
    }

    //Creates and sets up each player's power UI elements
    public void InitializePowers()
    {
        InitializeCelesteControls();
        InitializeSproutControls();
    }

    //Picks which of Celeste's UI to instantiate based on her controls
    public void InitializeCelesteControls()
    {
        if(userSettingsManager.celestialControlType == UserSettingsManager.ControlType.KEYBOARD)
        {
            activeCelesteUI = Instantiate(celesteKeyboardPanel, hudCanvas.transform);
        }
        else
        {
            activeCelesteUI = Instantiate(celesteControllerPanel, hudCanvas.transform);
        }
        SetCelesteAbilityOverlay();
    }

    //Picks which of Sprout's UI to instantiate based on her controls
    public void InitializeSproutControls()
    {
        if (userSettingsManager.earthControlType == UserSettingsManager.ControlType.KEYBOARD)
        {
            activeSproutUI = Instantiate(sproutKeyboardPanel, hudCanvas.transform);
        }
        else
        {
            activeSproutUI = Instantiate(sproutControllerPanel, hudCanvas.transform);
        }
        SetSproutAbilityOverlay();
    }

    //Creates the indicator for the button to press to open the menu
    public void InitializeMenuKeyGuide()
    {
        Instantiate(menuKeyPrefab, hudCanvas.transform);
    }

    //Creates the indicators for when an animal is being kidnapped
    public void InitializeKidnapIcons()
    {
        animalPointers = new GameObject[manager.GetAnimalSet().Count];
        int i = 0;

        foreach(GameObject animal in manager.GetAnimalSet())
        {
            animalPointers[i] = Instantiate(animalPointerPrefab, hudCanvas.transform);
            animalPointers[i].transform.GetChild(1).GetComponent<Image>().sprite = animal.GetComponent<Animal>().GetKidnapIcon();
            animal.GetComponent<Animal>().SetKidnapIconObject(animalPointers[i]);
            i++;
        }
    }

    

    /// <summary>
    /// GENERAL UI ELEMENTS
    /// </summary>
    /// <returns></returns>

    //Returns the entire objectives panel
    public GameObject GetObjectives()
    {
        return objectives;
    }

    //Get all the task data
    public TaskData[] GetTaskData()
    {
        return taskTexts;
    }

    public GameObject GetTask(int index)
    {
        return tasks[index];
    }

    //Create the list of tasks in the objective panel based on language
    public void PopulateObjectivePanel(UserSettingsManager.GameLanguage language, LevelEventManager levelEvents)
    {
        tasks = new GameObject[taskTexts.Length];
        for (int i = 0; i < taskTexts.Length; i++)
        {
            //We'll need a way to tile these properly
            tasks[i] = Instantiate(taskPrefab, objectives.transform.GetChild(1));
            if (language == UserSettingsManager.GameLanguage.ENGLISH)
            {
                tasks[i].GetComponent<TextMeshProUGUI>().text = taskTexts[i].GetEnglishText();
            }
            else
            {
                tasks[i].GetComponent<TextMeshProUGUI>().text = taskTexts[i].GetFrenchText();
            }
            levelEvents.GetProgress().SetTask(i, tasks[i]);
            taskTexts[i].SetTaskActive(taskTexts[i].GetTaskActiveOnStart());
            if (!taskTexts[i].GetTaskActive())
                tasks[i].gameObject.SetActive(false);
        }
        if (language == UserSettingsManager.GameLanguage.ENGLISH)
        {
            objectives.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Objectives";
        }
        else
        {
            objectives.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Objectifs";
        }
    }

    //Returns the entire inventory panel
    public GameObject GetInventoryPanel()
    {
        return inventoryPanel;
    }

    //Get the currently active virtualMouseUI object
    public GameObject GetActiveVirtualMouseUI()
    {
        return virtualMouseUI;
    }

    //Returns the entire pollution bar panel
    public GameObject GetPollutionBar()
    {
        return pollutionBar;
    }

    //Returns the entire day/night cycle panel
    public GameObject GetDayNightCycle()
    {
        return dayNightCycle;
    }

    //Returns the entire popup text panel
    public GameObject GetPopUpTextBox()
    {
        return popUpTextBox;
    }

    public void SetPopUpText(string incText)
    {
        popUpTextBox.GetComponentInChildren<TextMeshProUGUI>().text = incText;
    }

    //Returns the list of UI elements that are English language-specific
    public GameObject[] GetEnglishUIElements()
    {
        return englishUI;
    }

    //Returns the list of UI elements that are French language-specific
    public GameObject[] GetFrenchUIElements()
    {
        return frenchUI;
    }


    /// <summary>
    /// CELESTE'S UI ELEMENTS
    /// </summary>
    /// <returns></returns>

    //Returns the elements of Celeste's UI that are active
    public GameObject GetActiveCelesteUI()
    {
        return activeCelesteUI;
    }

    public void SetActiveCelesteUI(GameObject activeUI)
    {
        activeCelesteUI = activeUI;
    }

    //Get Celeste's keyboard HUD main panel
    public GameObject GetCelesteKeyboardPanel()
    {
        return celesteKeyboardPanel;
    }

    //Get each of the overlays over Celeste's abilities
    public GameObject[] GetCelesteOverlays()
    {
        return celesteAbilityOverlays;
    }

    //Add all of the overlays to the respective arrays
    public void SetCelesteAbilityOverlay()
    {
        for(int i = 0; i < celestePowerCount; i++)
        {
            celesteAbilityOverlays[i] = activeCelesteUI.transform.GetChild(0).GetChild(i).gameObject;
            powerOverlays.Add(celesteAbilityOverlays[i].GetComponent<PowerButtonHandler>().GetHashName(), celesteAbilityOverlays[i]);
            powerCooldownStates.Add(celesteAbilityOverlays[i].GetComponent<PowerButtonHandler>().GetHashName(), false);
            powerCooldownTime.Add(celesteAbilityOverlays[i].GetComponent<PowerButtonHandler>().GetHashName(), -1);
            powerCooldownProgress.Add(celesteAbilityOverlays[i].GetComponent<PowerButtonHandler>().GetHashName(), -1);
        }
    }

    //Get Celeste's controller HUD elements
    public GameObject GetCelesteControllerPanel()
    {
        return celesteControllerPanel;
    }

    //Get Celeste's keyboard movement key indicators
    public GameObject GetCelesteMoveKeyboard()
    {
        return moveCelesteKeyboard;
    }

    //Get Celeste's controller movement key indicators
    public GameObject GetCelesteMoveController()
    {
        return moveCelesteController;
    }

    public GameObject GetEnergyBar()
    {
        return energyBar;
    }


    /// <summary>
    /// SPROUT'S UI ELEMENTS
    /// </summary>
    /// <returns></returns>

    //Returns the elements of Sprout's UI that are active
    public GameObject GetActiveSproutUI()
    {
        return activeSproutUI;
    }

    public void SetActiveSproutUI(GameObject activeUI)
    {
        activeSproutUI = activeUI;
    }

    //Get Sprout's keyboard HUD panel, containing her abilities
    public GameObject GetSproutKeyboardPanel()
    {
        return sproutKeyboardPanel;
    }

    //Get each of the overlays over Sprout's abilities
    public GameObject[] GetSproutOverlays()
    {
        return sproutAbilityComponents;
    }

    public void SetSproutAbilityOverlay()
    {
        for (int i = 0; i < 4; i++)
        {
            sproutAbilityComponents[i] = activeSproutUI.transform.GetChild(0).GetChild(i).gameObject;
            powerOverlays.Add(sproutAbilityComponents[i].GetComponent<PowerButtonHandler>().GetHashName(), sproutAbilityComponents[i]);
            powerCooldownStates.Add(sproutAbilityComponents[i].GetComponent<PowerButtonHandler>().GetHashName(), false);
            powerCooldownTime.Add(sproutAbilityComponents[i].GetComponent<PowerButtonHandler>().GetHashName(), -1);
            powerCooldownProgress.Add(sproutAbilityComponents[i].GetComponent<PowerButtonHandler>().GetHashName(), -1);
        }
        for(int i = 4; i < 6; i++)
        {
            sproutAbilityComponents[i] = activeSproutUI.transform.GetChild(i - 3).gameObject;
            powerOverlays.Add(sproutAbilityComponents[i].GetComponent<PowerButtonHandler>().GetHashName(), sproutAbilityComponents[i]);
            powerCooldownStates.Add(sproutAbilityComponents[i].GetComponent<PowerButtonHandler>().GetHashName(), false);
            powerCooldownTime.Add(sproutAbilityComponents[i].GetComponent<PowerButtonHandler>().GetHashName(), -1);
            powerCooldownProgress.Add(sproutAbilityComponents[i].GetComponent<PowerButtonHandler>().GetHashName(), -1);
        }
    }

    //Get Sprout's controller HUD elements
    public GameObject GetSproutControllerPanel()
    {
        return sproutControllerPanel;
    }

    //Get Sprout's keyboard movement key indicators
    public GameObject GetSproutMoveKeyboard()
    {
        return moveSproutKeyboard;
    }

    //Get Sprout's controller movement key indicators
    public GameObject GetSproutMoveController()
    {
        return moveSproutController;
    }

    public Image GetVirtualMouseImage()
    {
        return cursorImage;
    }

    public void SetVirtualMouseImage(Sprite sprite)
    {
        //Debug.Log("Image object: " + cursorImage + ", sprite: " + sprite);
        cursorImage.sprite = sprite;
        virtualMouseUIMain.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = sprite;
        virtualMouseUISprout.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = sprite;
    }

    public Vector3 GetVirtualMousePosition()
    {
        return virtualMousePosition;
    }

    public void SetVirtualMousePosition(Vector3 incPosition)
    {
        virtualMousePosition = incPosition;
    }

    //Return the VirtualMouseInput object
    public VirtualMouseInput GetVirtualMouseInput()
    {
        return virtualMouseInput;
    }

    public VirtualMouseInput GetMainVirtualMouseInput()
    {
        return virtualMouseInputMain;
    }

    public VirtualMouseInput GetSproutVirtualMouseInput()
    {
        return virtualMouseInputSprout;
    }

    public void SetVirtualMouseInput(VirtualMouseInput incInput)
    {
        virtualMouseInput = incInput;
    }

    public GameObject GetMainVirtualMouseUI()
    {
        return virtualMouseUIMain;
    }

    public GameObject GetSproutVirtualMouseUI()
    {
        return virtualMouseUISprout;
    }

    public void SetActiveVirtualMouseUI(GameObject incMouseUI)
    {
        virtualMouseUI = incMouseUI;
    }

    public void SetSproutVirtualMouseUI(GameObject incMouseUI)
    {
        virtualMouseUISprout = incMouseUI;
        virtualMouseInputSprout = virtualMouseUISprout.GetComponent<VirtualMouseInput>();
        virtualMouseUISprout.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
    }

    


    /// <summary>
    /// OTHER DATA HELPERS
    /// </summary>
    /// <returns></returns>

    //Returns the colour used across the canvas for darkening UI elements
    public Color GetDarkenedColour()
    {
        return darkenedColour;
    }

    public int GetBasePollutionLevel()
    {
        return basePollutionLevel;
    }

    public int GetMaxPollutionLevel()
    {
        return maxPollution;
    }

    public float GetAcidRainThreshold()
    {
        return acidRainThreshold;
    }

    public float GetAcidRainHardThreshold()
    {
        return acidRainHardThreshold;
    }

    public GameObject GetEnergyNode()
    {
        return energyNode;
    }


    //Set the image used for the earth icon sprite
    public void SetEarthIcon(Sprite incSprite)
    {
        earthIcon.sprite = incSprite;
    }

    public Sprite GetHappyEarthIcon()
    {
        return happyLevelImage;
    }

    public Sprite GetSadEarthIcon()
    {
        return sadLevelImage;
    }

    public Sprite GetDesperateEarthIcon()
    {
        return desperateLevelImage;
    }

    public GameObject[] GetAnimalPointers()
    {
        return GetAnimalPointers();
    }

    public Dictionary<string, GameObject> GetOverlayTable()
    {
        return powerOverlays;
    }

    public Dictionary<string, bool> GetCooldownStateTable()
    {
        return powerCooldownStates;
    }

    public Dictionary<string, float> GetCooldownTimeTable()
    {
        return powerCooldownTime;
    }

    public Dictionary<string, float> GetCooldownProgress()
    {
        return powerCooldownProgress;
    }

    public List<GameObject> GetOverlayComponents()
    {
        return overlayComponents;
    }

    public void AddOverlayComponents(GameObject incObject)
    {
        overlayComponents.Add(incObject);
    }

    public void SetManager(HUDManager incHudManager)
    {
        manager = incHudManager;
    }

    public HUDManager GetManager()
    {
        return manager;
    }

    public GameObject GetDialogueManager()
    {
        return dialogueManager;
    }

    public void AddDialogueTrigger(DialogueTrigger trigger)
    {
        dialogueTriggers.Add(trigger);
        if(dialogueManager != null)
            trigger.SetDialogueManager(dialogueManager.GetComponent<DialogueManager>());
    }
}
