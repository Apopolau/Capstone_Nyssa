using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.AI;

public class EarthPlayer : MonoBehaviour
{
    [Header("These need to be set up in each scene")]
    [SerializeField] public GameObject plantParent;
    [SerializeField] public VirtualMouseInput virtualMouseInput;
    [SerializeField] public Camera mainCamera;
    [SerializeField] public TextMeshProUGUI displayText;
    [SerializeField] public Image selectTileText;

    // Reference to the UI controller script
    public EarthCharacterUIController uiController;
    [SerializeField] private GameObject darkenInSelectMode;
    [SerializeField] private GameObject darkenWhilePlanting;
    [SerializeField] private float darkeningAmount = 0.5f; // how much to darken the images

    [Header("Info for selecting plants")]
    public bool isPlantSelected = false;
    public bool isRemovalStarted = false;
    public bool isATileSelected = false;
    public GameObject plantSelected;
    public List<GameObject> plantsPlanted;
    public GameObject tempPlantPlanted;
    public GameObject plantPreview;
    [SerializeField] GameObject tileOutlinePrefab;
    public GameObject tileOutline;

    //Data for when the player picks a plant to plant
    public enum PlantSelectedType { NONE, TREE, FLOWER, GRASS }
    public PlantSelectedType plantSelectedType;

    //Data for the tile they're trying to plant on
    public enum TileSelectedType { LAND, WATER };
    public TileSelectedType currentTileSelectedType;
    public GameObject selectedTile;
    [SerializeField] private LayerMask tileMask;
    public Vector2 virtualMousePosition;

    [Header("Spell Variables")]
    [SerializeField] private GameObjectRuntimeSet playerList;
    [SerializeField] private GameObjectRuntimeSet animalList;
    private CelestialPlayer celestialPlayer;
    private GameObject powerTarget;
    private GameObject thornShield;
    List<GameObject> validTargets;
    private int validTargetIndex = 0;

    private WaitForSeconds plantTime;
    private WaitForSeconds healTime;
    private WaitForSeconds healCooldown;
    private WaitForSeconds barrierTime;
    private WaitForSeconds barrierCooldown;
    private WaitForSeconds barrierActiveTime;

    private bool healUsed;
    private bool shieldUsed;

    [SerializeField] float spellRange;
    private float closestDistance;

    [Header("Plant Objects")]
    [SerializeField] private GameObject treePrefab;
    [SerializeField] private GameObject landGrassPrefab;
    [SerializeField] private GameObject waterGrassPrefab;
    [SerializeField] private GameObject landFlowerPrefab;
    [SerializeField] private GameObject waterFlowerPrefab;

    [Header("Preview Objects")]
    [SerializeField] public GameObject treePreviewPrefab;
    [SerializeField] public GameObject landGrassPreviewPrefab;
    [SerializeField] public GameObject waterGrassPreviewPrefab;
    [SerializeField] public GameObject landFlowerPreviewPrefab;
    [SerializeField] public GameObject waterFlowerPreviewPrefab;

    [Header("VFX")]
    
    [SerializeField] private GameObject ThornShieldPrefab;

    [Header("Misc")]
    public bool enrouteToPlant = false;
    public EarthPlayerAnimator earthAnimator;
    private NavMeshAgent earthAgent;
    public EarthPlayerControl earthControls;
    [SerializeField] public WeatherState weatherState;
    private Vector3 OrigPos;
    public Stat health;

    public bool interacting = false;
    public Inventory inventory; // hold a reference to the Inventory scriptable object

    public event System.Action<int, int> OnHealthChanged;


    private void Awake()
    {
        earthAnimator = GetComponent<EarthPlayerAnimator>();
        earthControls = GetComponent<EarthPlayerControl>();
        earthAgent = GetComponent<NavMeshAgent>();
        earthAgent.enabled = false;
        virtualMouseInput.gameObject.GetComponentInChildren<Image>().enabled = false;
        virtualMousePosition = new Vector3();
        OrigPos = this.transform.position;
        health = new Stat(100, 100, false);
        validTargets = new List<GameObject>();
        
        
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject player in playerList.Items)
        {
            if (player.GetComponent<CelestialPlayer>())
            {
                celestialPlayer = player.GetComponent<CelestialPlayer>();
            }
        }
        plantTime = new WaitForSeconds(4.542f);
        healTime = new WaitForSeconds(0.7f);
        barrierTime = new WaitForSeconds(1.458f);
        healCooldown = new WaitForSeconds(10);
        barrierCooldown = new WaitForSeconds(10);
        barrierActiveTime = new WaitForSeconds(5);
        //playerInput = GetComponent<PlayerInput>();
        //actions = new PlayerInputActions();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Default controls are on: " + earthControls.controls.EarthPlayerDefault.enabled);
        //Debug.Log("Planting controls are on: " + earthControls.controls.PlantIsSelected.enabled);
        ActivateTile();
        if (enrouteToPlant && Mathf.Abs((this.transform.position - selectedTile.transform.position).magnitude) < earthAgent.stoppingDistance)
        {
            this.GetComponent<playerMovement>().ResetNavAgent();
            if (isPlantSelected)
            {
                PlantPlantingHandler();
            }
            else if (isRemovalStarted)
            {
                PlantRemovingHandler();
            }
            
        }

    }

    private void LateUpdate()
    {
        
        if (earthControls.userSettingsManager.earthControlType == UserSettingsManager.ControlType.CONTROLLER)
        {
            //Debug.Log("virtual mouse input: " + virtualMouseInput);
            //Debug.Log("virtual mouse: " + virtualMouseInput.virtualMouse);
            virtualMouseInput.cursorTransform.position = virtualMouseInput.virtualMouse.position.value;
            virtualMousePosition = virtualMouseInput.cursorTransform.position;
        }
        else if (earthControls.userSettingsManager.earthControlType == UserSettingsManager.ControlType.KEYBOARD)
        {
            virtualMouseInput.cursorTransform.position = Mouse.current.position.value;
            virtualMousePosition = Mouse.current.position.value;
        }
        

    }

    

    /// <summary>
    /// THESE FUNCTIONS HANDLE WHEN THE PLAYER PICKS A PLANT TO PLANT
    /// </summary>
    public void OnTreeSelected(InputAction.CallbackContext context)
    {
        //We're going to want to check if they even have the seed for the plant they selected before we do anything else
        if (inventory.HasEnoughItems("Tree Seed", 1))
        {

            if (isPlantSelected)
            {
                isPlantSelected = false;
                Destroy(plantSelected);
                Destroy(tileOutline);
            }
            if (!isPlantSelected)
            {
                //We select a type of plant from the input and make a transparent version of it with no stats
                plantSelectedType = PlantSelectedType.TREE;
                plantSelected = Instantiate(treePreviewPrefab, plantParent.transform);
                OnPlantSelectedWrapUp();
            }
        }
        else
        {
            StartCoroutine(InsufficientSeeds());
        }
    }

    public void OnGrassSelected(InputAction.CallbackContext context)
    {
        if (inventory.HasEnoughItems("Grass Seed", 1))
        {
            if (isPlantSelected)
            {
                isPlantSelected = false;
                Destroy(plantSelected);
                Destroy(tileOutline);
            }
            if (!isPlantSelected)
            {
                //We select a type of plant from the input and make a transparent version of it with no stats
                plantSelectedType = PlantSelectedType.GRASS;
                plantSelected = Instantiate(landGrassPreviewPrefab, plantParent.transform);
                OnPlantSelectedWrapUp();
            }
        }
        else
        {
            StartCoroutine(InsufficientSeeds());
        }
    }

    public void OnFlowerSelected(InputAction.CallbackContext context)
    {

        if (inventory.HasEnoughItems("Flower Seed", 1))
        {
            if (isPlantSelected)
            {
                isPlantSelected = false;
                Destroy(plantSelected);
                Destroy(tileOutline);
            }
            if (!isPlantSelected)
            {
                //We select a type of plant from the input and make a transparent version of it with no stats
                plantSelectedType = PlantSelectedType.FLOWER;
                plantSelected = Instantiate(landFlowerPreviewPrefab, plantParent.transform);
                OnPlantSelectedWrapUp();
            }
        }
        else
        {
            StartCoroutine(InsufficientSeeds());
        }

    }

    //This is called at the end of each plant selection function, to capture shared functionality
    private void OnPlantSelectedWrapUp()
    {
        //Debug.Log("Wrapping up plant selection");
        isPlantSelected = true;
        isATileSelected = false;
        plantSelected.transform.position = this.transform.position;

        //Switch our controls
        earthControls.controls.PlantIsSelected.Enable();
        earthControls.controls.EarthPlayerDefault.Disable();

        virtualMouseInput.gameObject.GetComponentInChildren<Image>().enabled = true;
        virtualMouseInput.cursorTransform.position = new Vector2(Screen.width / 2, Screen.height / 2);
        if (earthControls.userSettingsManager.earthControlType == UserSettingsManager.ControlType.CONTROLLER)
        {
            virtualMouseInput.cursorTransform.position = virtualMouseInput.virtualMouse.position.value;
            virtualMousePosition = virtualMouseInput.cursorTransform.position;
        }
        else if (earthControls.userSettingsManager.earthControlType == UserSettingsManager.ControlType.KEYBOARD)
        {
            virtualMouseInput.cursorTransform.position = Mouse.current.position.value;
            virtualMousePosition = Mouse.current.position.value;
        }

        DisplayTileText();
        DarkenAllImages(darkenInSelectMode);
        tileOutline = Instantiate(tileOutlinePrefab, this.transform);
    }



    /// <summary>
    /// THESE FUNCTIONS HANDLE WHEN THE PLAYER SELECTS A TILE TO PLANT ON
    /// AND ACTUALLY PLANTS
    /// </summary>
    public void PlantPlantingHandler()
    {
        StartCoroutine(OnPlantPlanted());
    }

    //Handles if they're in a position to start planting a plant
    private IEnumerator OnPlantPlanted()
    {
        //Have to add checks to make sure they are on a tile
        if (isPlantSelected && selectedTile.GetComponent<Cell>().tileValid)
        {
            TurnOffTileSelect(true);
            Destroy(plantSelected);
            HideTileText();
            ResetImageColor(darkenInSelectMode); 
            if (Mathf.Abs((this.transform.position - selectedTile.transform.position).magnitude) < earthAgent.stoppingDistance)
            {
                enrouteToPlant = false;
                isPlantSelected = false;
                Cell activeTileCell = selectedTile.GetComponent<Cell>();
                //Destroy(tileOutline);
                //This is a good place to initiate a planting animation
                GetComponent<playerMovement>().playerObj.transform.LookAt(this.transform);
                earthAnimator.animator.SetBool(earthAnimator.IfPlantingHash, true);
                earthAnimator.animator.SetBool(earthAnimator.IfWalkingHash, false);
                //Set other animations to false
                StartCoroutine(SuspendActions(plantTime));
                yield return plantTime;
                earthAnimator.animator.SetBool(earthAnimator.IfPlantingHash, false);
                PlantPlant(activeTileCell);
                plantSelectedType = PlantSelectedType.NONE;

                //Switch our controls
                earthControls.controls.EarthPlayerDefault.Enable();
                earthControls.controls.PlantIsSelected.Disable();
                virtualMouseInput.gameObject.GetComponentInChildren<Image>().enabled = false;
            }
            else
            {
                ApproachPlant();
            }
        }
        else if (isPlantSelected && !selectedTile.GetComponent<Cell>().tileValid)
        {
            //Display error message
            StartCoroutine(InvalidPlantLocation());
            yield break;
        }
        else
        {
            yield break;
        }
    }

    private IEnumerator InvalidPlantLocation()
    {
        displayText.text = "Invalid plant placement";
        yield return plantTime;
        displayText.text = "";
    }

    private IEnumerator InsufficientSeeds()
    {
        displayText.text = "Insufficient seeds of that type";
        yield return plantTime;
        displayText.text = "";
    }

    //Call if the player is too far from a tile they selected to plant
    private void ApproachPlant()
    {
        earthAgent.enabled = true;
        earthAgent.SetDestination(selectedTile.transform.position);
        enrouteToPlant = true;
    }

    //Finishes the process of planting a plant
    private void PlantPlant(Cell activeTileCell)
    {
        //We will want to add checks to make sure the tile type is valid, and check whether they are selecting a water or land tile
        //Pick the right plant based on the type of plant selected, and the tile selected, and then consume the appropriate seed
        if (plantSelectedType == PlantSelectedType.TREE)
        {
            tempPlantPlanted = Instantiate(treePrefab, activeTileCell.buildingTarget.transform);
            inventory.RemoveItemByName("Tree Seed", 1); //remove the item "Tree Seed"
        }
        else if (plantSelectedType == PlantSelectedType.FLOWER)
        {
            inventory.RemoveItemByName("Flower Seed", 1); //remove the item "Flower Seed"
            if (currentTileSelectedType == TileSelectedType.LAND)
            {
                tempPlantPlanted = Instantiate(landFlowerPrefab, activeTileCell.buildingTarget.transform);
            }
            else if (currentTileSelectedType == TileSelectedType.WATER)
            {
                tempPlantPlanted = Instantiate(waterFlowerPrefab, activeTileCell.buildingTarget.transform);
            }

        }
        else if (plantSelectedType == PlantSelectedType.GRASS)
        {
            inventory.RemoveItemByName("Grass Seed", 1); //remove the item "Flower Seed"
            if (currentTileSelectedType == TileSelectedType.LAND)
            {
                tempPlantPlanted = Instantiate(landGrassPrefab, activeTileCell.buildingTarget.transform);
            }
            else if (currentTileSelectedType == TileSelectedType.WATER)
            {
                tempPlantPlanted = Instantiate(waterGrassPrefab, activeTileCell.buildingTarget.transform);
            }
        }
        plantsPlanted.Add(tempPlantPlanted);
        int finalIndex = plantsPlanted.Count - 1;
        activeTileCell.placedObject = plantsPlanted[finalIndex];
        activeTileCell.tileHasBuild = true;
    }

    //If the player cancels planting while they have a plant selected
    public void OnPlantingCancelled()
    {
        if (isPlantSelected)
        {
            if (enrouteToPlant)
            {
                GetComponent<playerMovement>().ResetNavAgent();
            }
            isATileSelected = false;
            plantSelectedType = PlantSelectedType.NONE;
            Destroy(plantSelected);
            TurnOffTileSelect(false);
        }
    }



    /// <summary>
    /// THESE FUNCTIONS HANDLE IF THE PLAYER TRIES TO REMOVE A PLANT
    /// </summary>
    //Called when the player first picks to use this skill
    public void OnRemovePlant()
    {
        //Mark that we've started the process
        isRemovalStarted = true;
        isATileSelected = false;
        //Switch our controls
        earthControls.controls.RemovingPlant.Enable();
        earthControls.controls.EarthPlayerDefault.Disable();
        
        //Turn on the virtual mouse cursor
        virtualMouseInput.gameObject.GetComponentInChildren<Image>().enabled = true;
        virtualMouseInput.cursorTransform.position = new Vector2(Screen.width / 2, Screen.height / 2);
        if (earthControls.userSettingsManager.earthControlType == UserSettingsManager.ControlType.CONTROLLER)
        {
            virtualMousePosition = virtualMouseInput.cursorTransform.position;
        }
        else if (earthControls.userSettingsManager.earthControlType == UserSettingsManager.ControlType.KEYBOARD)
        {
            virtualMousePosition = Mouse.current.position.value;
        }

        //Create our tile outline effect
        DisplayTileText();
        DarkenAllImages(darkenInSelectMode);
        tileOutline = Instantiate(tileOutlinePrefab, this.transform);
    }

    //Called when the player selects a tile to remove a plant from
    public void PlantRemovingHandler()
    {
        StartCoroutine(OnPlantRemoved());
    }

    private IEnumerator OnPlantRemoved()
    {
        //Check if the tile they highlighted has a plant on it
        if (selectedTile.GetComponent<Cell>().tileIsActivated && selectedTile.GetComponent<Cell>().tileHasBuild)
        {
            
            TurnOffTileSelect(true);
            HideTileText();
            ResetImageColor(darkenInSelectMode); 
            //If we're close enough to the plant, we can go ahead and remove it
            if (Mathf.Abs((this.transform.position - selectedTile.transform.position).magnitude) < earthAgent.stoppingDistance)
            {
                isRemovalStarted = false;
                enrouteToPlant = false;
                Cell activeTileCell = selectedTile.GetComponent<Cell>();
                //Set our animations
                GetComponent<playerMovement>().playerObj.transform.LookAt(this.transform);
                earthAnimator.animator.SetBool(earthAnimator.IfPlantingHash, true);
                earthAnimator.animator.SetBool(earthAnimator.IfWalkingHash, false);
                StartCoroutine(SuspendActions(plantTime));
                yield return plantTime;
                earthAnimator.animator.SetBool(earthAnimator.IfPlantingHash, false);
                
                RemovePlant(activeTileCell);

                //Switch our controls
                earthControls.controls.EarthPlayerDefault.Enable();
                earthControls.controls.RemovingPlant.Disable();
                virtualMouseInput.gameObject.GetComponentInChildren<Image>().enabled = false;
            }
            //If we're not close enough, we'll have to get close enough
            else
            {
                ApproachPlant();
            }
        }
        //If the tile has no build
        else if (selectedTile.GetComponent<Cell>().tileIsActivated && !selectedTile.GetComponent<Cell>().tileHasBuild)
        {
            StartCoroutine(InvalidRemovalTile());
        }
        else
        {
            yield break;
        }
    }

    //Complete the action of removing the plant
    private void RemovePlant(Cell cellToRemoveFrom)
    {
        if(cellToRemoveFrom.placedObject != null)
        {
            if (cellToRemoveFrom.placedObject.GetComponent<Plant>())
            {
                
                cellToRemoveFrom.placedObject.GetComponent<Plant>().PlantDies();
                cellToRemoveFrom.placedObject = null;
            }
        }
        //Set the appropriate flags back to false
        
        cellToRemoveFrom.tileHasBuild = false;
    }

    private IEnumerator InvalidRemovalTile()
    {
        displayText.text = "No valid objects to remove";
        yield return plantTime;
        displayText.text = "";
    }

    public void OnRemovingCancelled()
    {
        if (isRemovalStarted)
        {
            if (enrouteToPlant)
            {
                GetComponent<playerMovement>().ResetNavAgent();
            }
            isRemovalStarted = false;
            TurnOffTileSelect(false);
        }
        
    }



    /// <summary>
    /// INTERACT FUNCTION
    /// </summary>
    /// <param name="context"></param>
    //A catch-all interact button. Simply sends a signal that the player is interacting, so various objects
    //  can react to it appropriately
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("Interacting");
            interacting = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Debug.Log("Not interacting anymore");
            interacting = false;
        }
    }



    /// <summary>
    /// HEALING FUNCTIONS
    /// </summary>
    public void CastHealHandler()
    {
        if (!healUsed && CheckIfValidTargets())
        {
            //HealSelectMode();
            displayText.text = "Select a target to heal";
            earthControls.controls.EarthPlayerDefault.Disable();
            earthControls.controls.HealSelect.Enable();
            Debug.Log(validTargets);
            PickClosestTarget();
            tileOutline = Instantiate(tileOutlinePrefab, powerTarget.transform);
            tileOutline.GetComponent<SpriteRenderer>().color = Color.green;
        }
        else if (healUsed)
        {
            StartCoroutine(AbilityOnCooldown());
        }
        else if (!CheckIfValidTargets())
        {
            StartCoroutine(NoAvailableTargets());
        }
    }

    //Called when the player selects a target and casts heal
    public void InitiateHealing()
    {
        StartCoroutine(HealingStarted());
    }

    //Handles staggering the functionality of healing
    private IEnumerator HealingStarted()
    {
        Destroy(tileOutline);
        earthControls.controls.HealSelect.Disable();
        CallSuspendActions(healTime);
        earthAnimator.animator.SetBool(earthAnimator.IfHealingHash, true);
        yield return healTime;
        earthAnimator.animator.SetBool(earthAnimator.IfHealingHash, false);
        if (powerTarget.GetComponent<CelestialPlayer>())
        {
            powerTarget.GetComponent<CelestialPlayer>().TakeHit(-20);
        }
        else if (powerTarget.GetComponent<Animal>())
        {
            powerTarget.GetComponent<Animal>().IsHealed();
        }
        healUsed = true;
        earthControls.controls.EarthPlayerDefault.Enable();
        StartCoroutine(HandleHealCooldown());
    }

    //Resets the cooldown on the heal
    private IEnumerator HandleHealCooldown()
    {
        yield return healCooldown;
        healUsed = false;
    }

    //If the player backs out after initiating picking a heal target
    public void OnHealingCancelled()
    {
        Destroy(tileOutline);
        displayText.text = "";
        earthControls.controls.EarthPlayerDefault.Enable();
        earthControls.controls.HealSelect.Disable();
    }



    /// <summary>
    /// SHIELDING FUNCTIONS
    /// </summary>
    public void CastThornShieldHandler()
    {
        if (!shieldUsed && CheckIfValidTargets())
        {
            displayText.text = "Select a target to shield";
            earthControls.controls.EarthPlayerDefault.Disable();
            earthControls.controls.BarrierSelect.Enable();
            PickClosestTarget();
            Debug.Log(validTargets);
            tileOutline = Instantiate(tileOutlinePrefab, powerTarget.transform);
            tileOutline.GetComponent<SpriteRenderer>().color = Color.green;
        }
        else if (shieldUsed)
        {
            StartCoroutine(AbilityOnCooldown());
        }
        else if (!CheckIfValidTargets())
        {
            StartCoroutine(NoAvailableTargets());
        }
    }

    public void InitiateBarrier()
    {
        StartCoroutine(BarrierStarted());
    }

    private IEnumerator BarrierStarted()
    {
        Destroy(tileOutline);
        earthControls.controls.BarrierSelect.Disable();
        CallSuspendActions(barrierTime);
        earthAnimator.animator.SetBool(earthAnimator.IfShieldingHash, true);
        yield return barrierTime;
        earthAnimator.animator.SetBool(earthAnimator.IfShieldingHash, false);
        thornShield = Instantiate(ThornShieldPrefab, powerTarget.transform);
        if (powerTarget.GetComponent<CelestialPlayer>())
        {
            powerTarget.GetComponent<CelestialPlayer>().ApplyBarrier();
        }
        else if (powerTarget.GetComponent<Animal>())
        {
            powerTarget.GetComponent<Animal>().ApplyBarrier();
        }
        shieldUsed = true;
        earthControls.controls.EarthPlayerDefault.Enable();
        StartCoroutine(HandleShieldCooldown());
        StartCoroutine(HandleShieldExpiry());
    }

    private IEnumerator HandleShieldCooldown()
    {
        yield return barrierCooldown;
        shieldUsed = false;
    }

    private IEnumerator HandleShieldExpiry()
    {
        yield return barrierActiveTime;
        Destroy(thornShield);
    }

    public void OnBarrierCancelled()
    {
        Destroy(tileOutline);
        displayText.text = "";
        earthControls.controls.EarthPlayerDefault.Enable();
        earthControls.controls.BarrierSelect.Disable();
    }

    /// <summary>
    /// GENERAL SPELL HELPERS
    /// </summary>
    /// <returns></returns>

    /// Warning telling the player the heal is on cooldown
    private IEnumerator AbilityOnCooldown()
    {
        displayText.text = "That ability still on cooldown";
        yield return plantTime;
        displayText.text = "";
    }

    //Warning telling the player they have no valid targets
    private IEnumerator NoAvailableTargets()
    {
        displayText.text = "No valid targets nearby";
        yield return plantTime;
        displayText.text = "";
    }

    private bool CheckIfValidTargets()
    {
        validTargets.Clear();
        Debug.Log(celestialPlayer);
        if(JudgeDistance(celestialPlayer.transform.position, this.transform.position, spellRange))
        {
            validTargets.Add(celestialPlayer.gameObject);
        }
        foreach(GameObject animal in animalList.Items)
        {
            if (JudgeDistance(animal.transform.position, this.transform.position, spellRange))
            {
                validTargets.Add(animal.gameObject);
            }
        }
        if(validTargets.Count > 0)
        {
            return true;
        }
        return false;
    }

    public void OnCycleTargets(bool right)
    {
        if (right && validTargets.Count > 1)
        {
            if (validTargets[validTargetIndex + 1] != null)
            {
                powerTarget = validTargets[validTargetIndex + 1];
                validTargetIndex++;
            }
            else
            {
                powerTarget = validTargets[0];
                validTargetIndex = 0;
            }
            tileOutline.transform.position = powerTarget.transform.position;
        }
        else if(!right && validTargets.Count > 1)
        {
            if (validTargets[validTargetIndex - 1] != null)
            {
                powerTarget = validTargets[validTargetIndex + 1];
                validTargetIndex++;
            }
            else
            {
                powerTarget = validTargets[validTargets.Count - 1];
                validTargetIndex = validTargets.Count - 1;
            }
            tileOutline.transform.position = powerTarget.transform.position;
        }
    }


    /// <summary>
    /// HELPER FUNCTIONS
    /// </summary>
    //When highlighting tiles, get information to move indicators
    private void ActivateTile()
    {
        //Only do this if we haven't selected a tile yet
        if(!isATileSelected)
        {
            Ray cameraRay = mainCamera.ScreenPointToRay(virtualMousePosition);
            RaycastHit hit;
            if (Physics.Raycast(cameraRay, out hit, 1000, tileMask))
            {
                selectedTile = hit.transform.gameObject.GetComponentInParent<Cell>().gameObject;
            }
        }
    }

    //When the player finishes using an ability with a tile select, shut off the appropriate UI and controls
    private void TurnOffTileSelect(bool tileSelectionState)
    {
        earthControls.controls.RemovingPlant.Disable();
        earthControls.controls.PlantIsSelected.Disable();
        earthControls.controls.EarthPlayerDefault.Enable();
        isATileSelected = tileSelectionState;
        Destroy(tileOutline);
        virtualMouseInput.gameObject.GetComponentInChildren<Image>().enabled = false;
    }

    //Create a list of targets that are in range of your abilities
    private bool JudgeDistance(Vector3 transform1, Vector3 transform2, float distance)
    {
        float calcDistance = Mathf.Abs((transform1 - transform2).magnitude);
        

        if (calcDistance < distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Goes through the current list of targets in range, finds the closest one
    private void PickClosestTarget()
    {
        int i = 0;
        foreach(GameObject potTarget in validTargets)
        {
            i++;
            float distanceMeasured = Mathf.Abs((potTarget.transform.position - this.transform.position).magnitude);
            if (distanceMeasured < closestDistance)
            {
                validTargetIndex = i;
                closestDistance = distanceMeasured;
                powerTarget = potTarget;
            }
        }
    }
    
    //If the earth player takes damage
    public bool TakeHit()
    {

        health.current -= 10;

        Debug.Log(health.current);

        if (OnHealthChanged != null)
            OnHealthChanged(health.max, health.current);

        bool isDead = health.current <= 0;
        if (isDead)
        {
            Respawn();
        }

        return isDead;

    }

    //If the earth player takes so much damage they get defeated
    private void Respawn()
    {

        health.current = 100;
        gameObject.transform.position = OrigPos;

    }

    //Call this if you want to have all player controls turned off for a certain amount of time
    public void CallSuspendActions(WaitForSeconds waitTime)
    {
        StartCoroutine(SuspendActions(waitTime));
    }

    private IEnumerator SuspendActions(WaitForSeconds waitTime)
    {
        earthControls.controls.EarthPlayerDefault.Disable();
        earthControls.controls.PlantIsSelected.Disable();
        uiController.DarkenOverlay(darkenWhilePlanting); //indicate no movement is allowed while planting
        yield return waitTime;
        earthControls.controls.EarthPlayerDefault.Enable();
        uiController.RestoreUI(darkenWhilePlanting);
    }

    //Switch between cameras for splitscreen
    public void SetCamera(Camera switchCam)
    {
        mainCamera = switchCam;

    }

    //Turns the UI element asking the player to pick a tile to plant on on or off
    public void DisplayTileText()
    {
        // Check if the Image component is disabled
        if (!selectTileText.enabled)
        {
            // Enable the Image component
            selectTileText.enabled = true;
        }

        // Activate the GameObject
         selectTileText.gameObject.SetActive(true);

    }

    public void HideTileText()
    {
        // Check if the Image component is disabled
        if (selectTileText.enabled)
        {
            // Enable the Image component
            selectTileText.enabled = false;
        }

        // Activate the GameObject
        selectTileText.gameObject.SetActive(false);

    
    }

    void DarkenAllImages(GameObject targetGameObject)
    {
        if (targetGameObject != null)
        {
            Image[] images = targetGameObject.GetComponentsInChildren<Image>();
            foreach (Image image in images)
            {
                // Create a copy of the current material
                Material darkenedMaterial = new Material(image.material);

                // Darken the material color
                Color darkenedColor = darkenedMaterial.color * darkeningAmount;
                darkenedMaterial.color = darkenedColor;

                // Assign the new material to the image
                image.material = darkenedMaterial;
            }
        }
        else
        {
            Debug.LogWarning("Target GameObject is not assigned.");
        }
    }
    
     // Function to reset color to original
    public void ResetImageColor(GameObject targetGameObject)
    {
        Image[] images = targetGameObject.GetComponentsInChildren<Image>();
            foreach (Image image in images)
            {
                 // Restore the original color
                 image.material.color = image.color;
            }
    }
    
}
