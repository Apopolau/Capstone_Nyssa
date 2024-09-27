using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.AI;

public class EarthPlayer : Player
{
    [Header("These need to be set up in each scene")]
    [SerializeField] public GameObject plantParent;
    //[SerializeField] public VirtualMouseInput virtualMouseInput;
    [SerializeField] public Camera mainCamera;
    //[SerializeField] public Image selectTileText;


    // Reference to the UI controller script

    [Header("UI elements")]
    [SerializeField] private GameObject virtualMouseUI;
    //public EarthCharacterUIController uiController;

    //public GameObject plantingControlsUI; 
    //public GameObject spellsControlsUI; //assign controlsUI
    //[SerializeField] private GameObject darkenInSelectMode;
    //[SerializeField] private GameObject darkenWhilePlanting;
    //[SerializeField] private float darkeningAmount = 0.5f; // how much to darken the images

    //public Image healCooldownOverlay;
    //public Image CTRLHealOverlay;
    //public Image thornCooldownOverlay;
    //public Image CTRLThornOverlay;

    [Header("State machine elements")]
    private BaseStateMachine stateMachine;
    //Player contains isStaggered
    //Player contains isDying
    private bool inInteraction_FSM = false;
    private bool inPlantSelection_FSM = false;
    private bool inRemovalSelection_FSM = false;
    private bool inHealSelection_FSM = false;
    private bool inBarrierSelection_FSM = false;
    private bool inDialogue_FSM = false;
    private bool inWaiting_FSM = false;
    private bool inHoldingNyssa_FSM = false;
    //Note: this is for animations where the player should stop moving
    private bool isMidAnimation_FSM;

    private Vector3 turnToTarget;
    private bool isTurning;

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
    [SerializeField] private LayerMask groundMask;
    //public Vector2 virtualMousePosition;

    [Header("Spell Variables")]
    [SerializeField] private GameObjectRuntimeSet playerList;
    [SerializeField] private GameObjectRuntimeSet animalList;
    [SerializeField] private GameObjectRuntimeSet buildList;
    private CelestialPlayer celestialPlayer;
    private GameObject powerTarget;
    private GameObject thornShield;
    List<GameObject> validTargets;
    private int validTargetIndex = 0;

    private WaitForSeconds plantTime;
    private WaitForSeconds healTime;
    private float healCooldown = 10f;
    private WaitForSeconds barrierTime;
    private float barrierCooldown = 10f;
    private WaitForSeconds barrierActiveTime;
    private WaitForSeconds iFramesLength;
    private WaitForSeconds deathAnimLength;
    private WaitForSeconds staggerLength;

    private WaitForSeconds suspensionTime;

    //public event System.Action<string, int> OnSeedCountChange;
    public event System.Action OnPowerStateChange;

    private bool healOnCooldown;
    private bool barrierOnCooldown;
    

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

    [Header("Icon Sprites")]
    [SerializeField] private Sprite i_grassSeed;
    [SerializeField] private Sprite i_flowerSeed;
    [SerializeField] private Sprite i_treeSeed;
    [SerializeField] public Sprite i_shovel;

    [Header("VFX")]
    [SerializeField] private GameObject ThornShieldPrefab;

    [Header("SFX")]
    private SproutSoundLibrary s_soundLibrary;

    [Header("Misc")]
    
    public bool enrouteToPlant = false;
    public EarthPlayerAnimator earthAnimator;
    private NavMeshAgent earthAgent;
    private playerMovement pMovement;
    public EarthPlayerControl earthControls;
    [SerializeField] public WeatherState weatherState;
    public Inventory inventory; // hold a reference to the Inventory scriptable object
    public GameObject nyssaHoldTarget;


    private void Awake()
    {
        stateMachine = GetComponent<BaseStateMachine>();
        earthAnimator = GetComponent<EarthPlayerAnimator>();
        earthControls = GetComponent<EarthPlayerControl>();
        earthAgent = GetComponent<NavMeshAgent>();
        earthAgent.enabled = false;
        pMovement = GetComponent<playerMovement>();
        //virtualMouseInput.gameObject.GetComponentInChildren<Image>().enabled = false;
        //virtualMousePosition = new Vector3();
        OrigPos = this.transform.position;
        health = new Stat(100, 100, false);
        validTargets = new List<GameObject>();
        s_soundLibrary = base.soundLibrary as SproutSoundLibrary;
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
        //healCooldown = new WaitForSeconds(10);
        //barrierCooldown = new WaitForSeconds(10);
        barrierActiveTime = new WaitForSeconds(5);
        iFramesLength = new WaitForSeconds(0.5f);
        staggerLength = new WaitForSeconds(0.958f);
        deathAnimLength = new WaitForSeconds(1.458f);
        hudManager.SetSproutVirtualMouseUI(virtualMouseUI);
    }

    // Update is called once per frame
    void Update()
    {
        ActivateTile();
        MovePlantPreview();
        if (enrouteToPlant && Mathf.Abs((this.transform.position - selectedTile.transform.position).magnitude) < earthAgent.stoppingDistance + 2)
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

    private void FixedUpdate()
    {
        if (isTurning)
        {
            TurnToTarget();
        }
    }

    private void LateUpdate()
    {
        hudManager.MoveVirtualMouse(userSettingsManager.earthControlType);
        
    }

    

    /// <summary>
    /// THESE FUNCTIONS HANDLE WHEN THE PLAYER PICKS A PLANT TO PLANT
    /// </summary>
    public void OnTreeSelected(InputAction.CallbackContext context)
    {
        //We're going to want to check if they even have the seed for the plant they selected before we do anything else
        if (inventory.HasEnoughItems("Tree Seed", 1))
        {
            /*
            if (isPlantSelected)
            {
                isPlantSelected = false;
                Destroy(plantSelected);
                Destroy(tileOutline);
            }
            */
            if (!isPlantSelected)
            {
                //We select a type of plant from the input and make a transparent version of it with no stats
                plantSelectedType = PlantSelectedType.TREE;
                SwitchCursorIcon(i_treeSeed);
                //plantSelected = Instantiate(treePreviewPrefab, plantParent.transform);
                OnPlantSelectedWrapUp();
            }
        }
        else
        {
            string insufficentSeeds = "Insufficient seeds of that type";
            hudManager.ThrowPlayerWarning(insufficentSeeds);
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
                SwitchCursorIcon(i_grassSeed);
                //plantSelected = Instantiate(landGrassPreviewPrefab, plantParent.transform);
                OnPlantSelectedWrapUp();
            }
        }
        else
        {
            string insufficentSeeds = "Insufficient seeds of that type";
            hudManager.ThrowPlayerWarning(insufficentSeeds);
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
                SwitchCursorIcon(i_flowerSeed);
                //plantSelected = Instantiate(landFlowerPreviewPrefab, plantParent.transform);
                
                OnPlantSelectedWrapUp();
            }
        }
        else
        {
            string insufficentSeeds = "Insufficient seeds of that type";
            hudManager.ThrowPlayerWarning(insufficentSeeds);
        }

    }

    //This is called at the end of each plant selection function, to capture shared functionality
    private void OnPlantSelectedWrapUp()
    {
        InitializePlantPreview();
        inPlantSelection_FSM = true;
        OnPowerStateChange();
        //isPlantSelected = true;
        //isATileSelected = false;
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
        if(selectedTile == null)
        {
            //Display error message
            string invalidTile = "Invalid plant placement";
            hudManager.ThrowPlayerWarning(invalidTile);
            yield break;
        }
        else if (isPlantSelected && selectedTile.GetComponent<Cell>().tileValid)
        {
            //isATileSelected = true;
            inPlantSelection_FSM = false;
            
            if (Mathf.Abs((this.transform.position - selectedTile.transform.position).magnitude) < earthAgent.stoppingDistance + 2)
            {
                isPlantSelected = false;
                enrouteToPlant = false;
                Cell activeTileCell = selectedTile.GetComponent<Cell>();

                //Pause other controls, initiate animation
                GetComponent<playerMovement>().playerObj.transform.LookAt(this.transform);
                earthAnimator.animator.SetBool(earthAnimator.IfPlantingHash, true);
                earthAnimator.animator.SetBool(earthAnimator.IfWalkingHash, false);
                inInteraction_FSM = true;
                StartCoroutine(SuspendActions(plantTime));
                s_soundLibrary.PlayPlantClips();
                //After wait time
                yield return plantTime;
                earthAnimator.animator.SetBool(earthAnimator.IfPlantingHash, false);
                inInteraction_FSM = false;
                
                PlantPlant(activeTileCell);
                plantSelectedType = PlantSelectedType.NONE;
            }
            else
            {
                ApproachPlant();
            }
        }
        else if (isPlantSelected && !selectedTile.GetComponent<Cell>().tileValid)
        {
            //Display error message
            string invalidTile = "Invalid plant placement";
            hudManager.ThrowPlayerWarning(invalidTile);
            yield break;
        }
        else
        {
            yield break;
        }
    }

    //Call if the player is too far from a tile they selected to plant
    private void ApproachPlant()
    {
        earthAnimator.animator.SetBool(earthAnimator.IfWalkingHash, true);
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
        tempPlantPlanted.GetComponent<Plant>().SetInventory(inventory);
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
            isPlantSelected = false;
            isATileSelected = false;
            inPlantSelection_FSM = false;
            
            plantSelectedType = PlantSelectedType.NONE;
        }
    }



    /// <summary>
    /// THESE FUNCTIONS HANDLE IF THE PLAYER TRIES TO REMOVE A PLANT
    /// </summary>
    //Called when the player first picks to use this skill
    public void OnRemovePlant()
    {
        //Mark that we've started the process
        if(buildList.Items.Count > 0)
        {
            inRemovalSelection_FSM = true;
        }
        else
        {
            string removeWarning = "No plants to remove";
            hudManager.ThrowPlayerWarning(removeWarning);
        }
        
    }

    //Called when the player selects a tile to remove a plant from
    public void PlantRemovingHandler()
    {
        StartCoroutine(OnPlantRemoved());
    }

    private IEnumerator OnPlantRemoved()
    {
        //Have to add checks to make sure they are on a tile
        if (selectedTile == null)
        {
            //Display error message
            string invalidTile = "No tile selected";
            hudManager.ThrowPlayerWarning(invalidTile);
            yield break;
        }
        //Check if the tile they highlighted has a plant on it
        if (selectedTile.GetComponent<Cell>().tileIsActivated && selectedTile.GetComponent<Cell>().tileHasBuild)
        {
            inRemovalSelection_FSM = false;
            //isATileSelected = true;
            //If we're close enough to the plant, we can go ahead and remove it
            if (Mathf.Abs((this.transform.position - selectedTile.transform.position).magnitude) < earthAgent.stoppingDistance + 2)
            {
                isRemovalStarted = false;
                enrouteToPlant = false;
                Cell activeTileCell = selectedTile.GetComponent<Cell>();
                //Pause other controls and start animations
                GetComponent<playerMovement>().playerObj.transform.LookAt(this.transform);
                earthAnimator.animator.SetBool(earthAnimator.IfPlantingHash, true);
                earthAnimator.animator.SetBool(earthAnimator.IfWalkingHash, false);
                inInteraction_FSM = true;
                StartCoroutine(SuspendActions(plantTime));
                s_soundLibrary.PlayPlantClips();
                yield return plantTime;
                //Set things back
                inInteraction_FSM = false;
                earthAnimator.animator.SetBool(earthAnimator.IfPlantingHash, false);
                
                RemovePlant(activeTileCell);
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
            string invalidTile = "No valid objects to remove";
            hudManager.ThrowPlayerWarning(invalidTile);
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
                if (OnPowerStateChange != null)
                    OnPowerStateChange();
            }
        }
        //Set the appropriate flags back to false
        
        cellToRemoveFrom.tileHasBuild = false;
    }

    //If removing gets cancelled
    public void OnRemovingCancelled()
    {
        if (isRemovalStarted)
        {
            if (enrouteToPlant)
            {
                GetComponent<playerMovement>().ResetNavAgent();
            }
            isATileSelected = false;
            isRemovalStarted = false;
            inRemovalSelection_FSM = false;
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
            //Do not confuse this for the bool that controls the state machine
            interacting = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            interacting = false;
        }
    }

    public void PickUpNyssa()
    {
        inHoldingNyssa_FSM = true;
        earthAnimator.animator.SetBool(earthAnimator.IfCarryingHash, true);
    }

    public void PutDownNyssa()
    {
        inHoldingNyssa_FSM = false;
        earthAnimator.animator.SetBool(earthAnimator.IfCarryingHash, false);
    }

    public GameObject GetNyssaTarget()
    {
        return nyssaHoldTarget;
    }

    public void SetHoldingNyssa(bool isHolding)
    {
        inHoldingNyssa_FSM = isHolding;
    }

    /// <summary>
    /// HEALING FUNCTIONS
    /// </summary>
    public void CastHealHandler()
    {
        if (!healOnCooldown && CheckIfValidTargets())
        {
            inHealSelection_FSM = true;
            OnPowerStateChange();

        }
        else if (healOnCooldown)
        {
            string OnCooldown = "That ability is still on cooldown";
            hudManager.ThrowPlayerWarning(OnCooldown);
            
           
        }
        else if (!CheckIfValidTargets())
        {
            string noValidTargets = "There are no valid targets nearby";
            hudManager.ThrowPlayerWarning(noValidTargets);
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
        /*
        displayText.text = "";
        Destroy(tileOutline);
        earthControls.controls.HealSelect.Disable();
        */
        inHealSelection_FSM = false;
        inInteraction_FSM = true;
        CallSuspendActions(healTime);
        earthAnimator.animator.SetBool(earthAnimator.IfHealingHash, true);
        soundLibrary.PlaySpellClips();
        yield return healTime;
        inInteraction_FSM = false;
        earthAnimator.animator.SetBool(earthAnimator.IfHealingHash, false);
        
        if (powerTarget.GetComponent<Player>())
        {
            powerTarget.GetComponent<Player>().TakeHit(-20);
        }
        else if (powerTarget.GetComponent<Animal>())
        {
            powerTarget.GetComponent<Animal>().IsHealed();
        }
        healOnCooldown = true;
        earthControls.controls.EarthPlayerDefault.Enable();
        //uiController.RestoreUI(darkenWhilePlanting);
        hudManager.ToggleSproutPanel(true);
        StartCoroutine(HandleHealCooldown());
        //healCooldownOverlay.gameObject.SetActive(true);
        //CTRLHealOverlay.gameObject.SetActive(true);
        //StartCoroutine(CoolDownImageFill(healCooldownOverlay, healCooldown));
        //hudManager.InitiateCooldownIndicator("CastHeal", healCooldown);
        StartCooldownUI("CastHeal", healCooldown);
        //StartCoroutine(CoolDownImageFill(CTRLHealOverlay, healCooldown));
        
    }

    //Resets the cooldown on the heal
    private IEnumerator HandleHealCooldown()
    {
        yield return new WaitForSeconds(healCooldown);
        healOnCooldown = false;
        OnPowerStateChange();
    }

    //If the player backs out after initiating picking a heal target
    public void OnHealingCancelled()
    {
        inHealSelection_FSM = false;
        OnPowerStateChange();
        //Destroy(tileOutline);
        //displayText.text = "";
        //earthControls.controls.EarthPlayerDefault.Enable();
        //earthControls.controls.HealSelect.Disable();
    }



    /// <summary>
    /// SHIELDING FUNCTIONS
    /// </summary>
    public void CastThornShieldHandler()
    {
        if (!barrierOnCooldown && CheckIfValidTargets())
        {
            inBarrierSelection_FSM = true;
            OnPowerStateChange();
        }
        else if (barrierOnCooldown)
        {
            string shieldOnCooldown = "That ability is still on cooldown";
            //StartCoroutine(ThrowPlayerWarning(shieldOnCooldown));
            hudManager.ThrowPlayerWarning(shieldOnCooldown);
        }
        else if (!CheckIfValidTargets())
        {
            string noValidTargets = "There are no valid targets nearby";
            hudManager.ThrowPlayerWarning(noValidTargets);
        }
    }

    public void InitiateBarrier()
    {
        StartCoroutine(BarrierStarted());
    }

    private IEnumerator BarrierStarted()
    {
        inBarrierSelection_FSM = false;

        inInteraction_FSM = true;
        CallSuspendActions(barrierTime);
        earthAnimator.animator.SetBool(earthAnimator.IfShieldingHash, true);
        soundLibrary.PlaySpellClips();
        yield return barrierTime;
        earthAnimator.animator.SetBool(earthAnimator.IfShieldingHash, false);
        inInteraction_FSM = false;

        thornShield = Instantiate(ThornShieldPrefab, powerTarget.transform);
        if (powerTarget.GetComponent<Player>())
        {
            thornShield.transform.position = new Vector3(powerTarget.transform.position.x, 
                powerTarget.transform.position.y + powerTarget.GetComponent<CapsuleCollider>().height / 2, powerTarget.transform.position.z);
            powerTarget.GetComponent<Player>().ApplyBarrier();
        }
        else if (powerTarget.GetComponent<Animal>())
        {
            powerTarget.GetComponent<Animal>().ApplyBarrier();
        }
        barrierOnCooldown = true;
        earthControls.controls.EarthPlayerDefault.Enable();
        //uiController.RestoreUI(darkenWhilePlanting);
        hudManager.ToggleSproutPanel(true);
        

        StartCoroutine(HandleShieldCooldown());
        //CTRLThornOverlay.gameObject.SetActive(true);
        //thornCooldownOverlay.gameObject.SetActive(true);
        StartCooldownUI("CastBarrier", barrierCooldown);
        //StartCoroutine(CoolDownImageFill(thornCooldownOverlay, barrierCooldown));

        //StartCoroutine(CoolDownImageFill(CTRLThornOverlay, barrierCooldown));

        StartCoroutine(HandleShieldExpiry());
    }

    private IEnumerator HandleShieldCooldown()
    {
        yield return new WaitForSeconds(barrierCooldown);
        barrierOnCooldown = false;
        OnPowerStateChange();
    }

    private IEnumerator HandleShieldExpiry()
    {
        yield return barrierActiveTime;
        Destroy(thornShield);
    }

    public void OnBarrierCancelled()
    {
        inBarrierSelection_FSM = false;
        OnPowerStateChange();
    }



    /// <summary>
    /// GENERAL SPELL HELPERS
    /// </summary>
    /// <returns></returns>
    private bool CheckIfValidTargets()
    {
        validTargets.Clear();
        validTargets.Add(this.gameObject);
        if(JudgeDistance(celestialPlayer.transform.position, this.gameObject.transform.position, spellRange))
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
            if (validTargetIndex < validTargets.Count - 1)
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
            SetTurnTarget(powerTarget.transform.position);
        }
        else if(!right && validTargets.Count > 1)
        {
            if (validTargetIndex > 0)
            {
                powerTarget = validTargets[validTargetIndex - 1];
                validTargetIndex--;
            }
            else
            {
                powerTarget = validTargets[validTargets.Count - 1];
                validTargetIndex = validTargets.Count - 1;
            }
            tileOutline.transform.position = powerTarget.transform.position;
            SetTurnTarget(powerTarget.transform.position);
        }
    }

    //Create a list of targets that are in range of your abilities
    private bool JudgeDistance(Vector3 transform1, Vector3 transform2, float distance)
    {
        float calcDistance = Mathf.Abs((transform1 - transform2).magnitude);


        if (calcDistance <= distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Goes through the current list of targets in range, finds the closest one
    public void PickClosestTarget()
    {
        closestDistance = spellRange;
        int i = 0;
        foreach (GameObject potTarget in validTargets)
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

    public GameObject GetPowerTarget()
    {
        return powerTarget;
    }


    /// <summary>
    /// UI FUNCTIONS
    /// </summary>
    //Switch between cameras for splitscreen
    public void SetCamera(Camera switchCam)
    {
        mainCamera = switchCam;

    }

    //Handles UI components of tile select
    public void TurnOnTileSelect(Transform target)
    {
        //tileOutline = Instantiate(tileOutlinePrefab, target.transform);
        TurnOnCursor();
        //DisplayTileText();
        hudManager.ThrowPlayerWarning("Please select a tile");
        //uiController.DarkenOverlay(darkenWhilePlanting);
        //DarkenAllImages(GetPlantDarkenObject());
        hudManager.ToggleSproutPanel(true);
    }

    //When the player finishes using an ability with a tile select, shut off the appropriate UI and controls
    public void TurnOffTileSelect()
    {
        TurnOffCursor();
        //HideTileText();
        hudManager.TurnOffPopUpText();
        //uiController.RestoreUI(darkenWhilePlanting);
        //ResetImageColor(GetPlantDarkenObject());
        hudManager.ToggleSproutPanel(false);
        Destroy(tileOutline);
    }

    //Creates a preview of the plant the player is attempting to plant
    public void InitializePlantPreview()
    {
        if (plantSelectedType == PlantSelectedType.NONE)
            return;
        if (plantSelectedType == PlantSelectedType.TREE)
            plantSelected = Instantiate(treePreviewPrefab, plantParent.transform);
        if (plantSelectedType == PlantSelectedType.FLOWER)
            plantSelected = Instantiate(landFlowerPreviewPrefab, plantParent.transform);
        if (plantSelectedType == PlantSelectedType.GRASS)
            plantSelected = Instantiate(landGrassPreviewPrefab, plantParent.transform);
    }

    public void SwitchCursorIcon(Sprite newSprite)
    {
        //virtualMouseInput.cursorGraphic.GetComponent<Image>().sprite = newSprite;
        hudManager.SetVirtualMouseImage(newSprite);
    }

    public void TurnOnCursor()
    {
        /*
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
        */
        hudManager.ToggleVirtualMouseSprite(true);
    }

    public void TurnOffCursor()
    {
        //virtualMouseInput.gameObject.GetComponentInChildren<Image>().enabled = false;
        hudManager.ToggleVirtualMouseSprite(false);
    }

    public GameObject GetTileOutlinePrefab()
    {
        return tileOutlinePrefab;
    }

    /*
    public GameObject GetPlantDarkenObject()
    {
        return darkenInSelectMode;
    }
    */

    // handle cool down for thorn and heal powers
    /*
    public IEnumerator CoolDownImageFill(Image fillImage, float cooldown)
    {
        float timer = 0f;
        float startFillAmount = 1f;
        float endFillAmount = 0f;

        // Gradually decrease fill amount over cooldown duration
        while (timer < cooldown)
        {
            float fillAmount = Mathf.Lerp(startFillAmount, endFillAmount, timer / cooldown);
            fillImage.fillAmount = fillAmount;
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure fill amount is exactly 0
        fillImage.fillAmount = endFillAmount;
    }
    */

    

    /// <summary>
    /// HELPER FUNCTIONS
    /// </summary>
    //When highlighting tiles, get information to move indicators
    private void ActivateTile()
    {
        //Only do this if we haven't selected a tile yet
        if(!isATileSelected)
        {
            Ray cameraRay = mainCamera.ScreenPointToRay(hudManager.GetVirtualMousePosition());
            RaycastHit hit;
            if (Physics.Raycast(cameraRay, out hit, 1000, tileMask))
            {
                selectedTile = hit.transform.gameObject.GetComponentInParent<Cell>().gameObject;
            }
            else
            {
                selectedTile = null;
            }
        }
    }

    private void MovePlantPreview()
    {
        if(isPlantSelected && !isATileSelected && selectedTile == null)
        {
            plantSelected.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 0.5f, 0.5f, 0.5f);
            tileOutline.GetComponentInChildren<SpriteRenderer>().color = Color.red;
            Ray cameraRay = mainCamera.ScreenPointToRay(hudManager.GetVirtualMousePosition());
            RaycastHit hit;
            if (Physics.Raycast(cameraRay, out hit, 1000, groundMask))
            {
                plantSelected.transform.position = hit.point;
                tileOutline.transform.position = hit.point;
            }
        }
    }


    public void SetTurnTarget(Vector3 target)
    {
        turnToTarget = target;
    }

    public void TurnToTarget()
    {
        float step;
        float speed = 0.3f;
        step = speed * Time.deltaTime;
        Vector3 lookVector = new Vector3(pMovement.playerObj.transform.position.x,
            turnToTarget.y, pMovement.playerObj.transform.position.z);
        Vector3 rotateVector = Vector3.RotateTowards(pMovement.playerObj.transform.position, lookVector, step, 0f);
        pMovement.playerObj.rotation = Quaternion.LookRotation(rotateVector);

    }

    public void ToggleTurning()
    {
        if (isTurning)
        {
            isTurning = false;
        }
        else if (!isTurning)
        {
            isTurning = true;
        }
    }

    

    /// <summary>
    /// HELPER FUNCTIONS FOR STATE MACHINE
    /// </summary>
    /// <returns></returns>
    //Call this if you want to have all player controls turned off for a certain amount of time
    protected override IEnumerator SuspendActions(WaitForSeconds waitTime, bool boolToChange)
    {
        //earthControls.controls.EarthPlayerDefault.Disable();
        //earthControls.controls.PlantIsSelected.Disable();
        //uiController.DarkenOverlay(darkenWhilePlanting); //indicate no movement is allowed while planting
        //hudManager.ToggleSproutPanel(false);
        hudManager.SetSproutOccupied(true);
        OnPowerStateChange();
        yield return waitTime;
        //earthControls.controls.EarthPlayerDefault.Enable();
        //Use this to switch off of whatever action is happening
        boolToChange = false;
        //uiController.RestoreUI(darkenWhilePlanting);
        //hudManager.ToggleSproutPanel(true);
        hudManager.SetSproutOccupied(false);
        OnPowerStateChange();
    }

    protected override IEnumerator SuspendActions(WaitForSeconds waitTime)
    {
        hudManager.SetSproutOccupied(true);
        OnPowerStateChange();
        yield return waitTime;
        hudManager.SetSproutOccupied(false);
        OnPowerStateChange();
    }

    public WaitForSeconds GetSuspensionTime()
    {
        return suspensionTime;
    }

    public void SetSuspensionTime(WaitForSeconds timeToSuspend)
    {
        suspensionTime = timeToSuspend;
    }


    //Return if the player is in the middle of interacting
    public bool GetIsInteracting()
    {
        return inInteraction_FSM;
    }

    public bool GetInPlantSelection()
    {
        return inPlantSelection_FSM;
    }

    public bool GetInRemovalSelection()
    {
        return inRemovalSelection_FSM;
    }

    public bool GetInHealSelection()
    {
        return inHealSelection_FSM;
    }

    public bool GetInBarrierSelection()
    {
        return inBarrierSelection_FSM;
    }

    public bool GetBarrierOnCooldown()
    {
        return barrierOnCooldown;
    }

    public bool GetHealOnCooldown()
    {
        return healOnCooldown;
    }

    public bool GetInDialogue()
    {
        return inDialogue_FSM;
    }

    public bool GetIsHoldingNyssa()
    {
        return inHoldingNyssa_FSM;
    }
    
    public void ToggleDialogueState()
    {
        if (inDialogue_FSM)
        {
            inDialogue_FSM = false;
        }
        if (!inDialogue_FSM)
        {
            inDialogue_FSM = true;
        }
    }

    public void ToggleDialogueState(bool newState)
    {
        inDialogue_FSM = newState;
    }

    public void ToggleWaiting(bool newState)
    {
        inWaiting_FSM = newState;
    }

    public bool GetIsWaiting()
    {
        return inWaiting_FSM;
    }

    public void ToggleInteractingState()
    {
        if (inInteraction_FSM)
        {
            inInteraction_FSM = false;
        }
        else if (!inInteraction_FSM)
        {
            inInteraction_FSM = true;
        }
    }

    public bool GetMidAnimation()
    {
        return isMidAnimation_FSM;
    }

    
}
