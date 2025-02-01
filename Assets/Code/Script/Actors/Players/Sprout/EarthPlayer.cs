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
    [SerializeField] public Camera mainCamera;

    [Header("UI elements")]
    [SerializeField] private GameObject virtualMouseUI;
    [SerializeField] private GameObject plantControlsUI;
    [SerializeField] private GameObject castControlsUI;

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
    private bool inInGameMenu_FSM = false;
    //Note: this is for animations where the player should stop moving
    private bool isMidAnimation_FSM;

    private Vector3 turnToTarget;
    private bool isTurning;

    [Header("Info for selecting plants")]
    public bool isPlantSelected = false;
    private bool isPlanting = false;
    public bool isRemovalStarted = false;
    private bool isRemoving = false;
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
    private GameObject thornShield;

    private float healCooldown = 10f;
    private float barrierCooldown = 10f;
    private WaitForSeconds barrierActiveTime;
    private WaitForSeconds iFramesLength;
    //private WaitForSeconds deathAnimLength;
    //private WaitForSeconds staggerLength;

    private WaitForSeconds suspensionTime;

    //Run this when our power usability changes
    public event System.Action OnPowerStateChange;

    private bool healOnCooldown;
    private bool barrierOnCooldown;
    
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
    //private NavMeshAgent earthAgent;
    private playerMovement pMovement;
    public EarthPlayerControl earthControls;
    [SerializeField] public WeatherState weatherState;
    public Inventory inventory; // hold a reference to the Inventory scriptable object
    public GameObject nyssaHoldTarget;


    private void Awake()
    {
        //Grab component scripts
        stateMachine = GetComponent<BaseStateMachine>();
        animator = GetComponent<EarthPlayerAnimator>();
        earthControls = GetComponent<EarthPlayerControl>();
        agent = GetComponent<NavMeshAgent>();
        pMovement = GetComponent<playerMovement>();
        s_soundLibrary = base.soundLibrary as SproutSoundLibrary;

        OrigPos = this.gameObject.transform.position;

        //Initialize stats
        health = new Stat(100, 100, false);

        validTargets = new List<GameObject>();

        agent.enabled = false;

        SetLanguageState(userSettingsManager.chosenLanguage);
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

        //Initialize other coroutine timers
        barrierActiveTime = new WaitForSeconds(5);
        iFramesLength = new WaitForSeconds(0.5f);
        
        //hudManager.SetSproutVirtualMouseUI(virtualMouseUI);
    }

    // Update is called once per frame
    void Update()
    {
        ActivateTile();
        MovePlantPreview();
        if (enrouteToPlant && Mathf.Abs((this.transform.position - selectedTile.transform.position).magnitude) <= agent.stoppingDistance + 1)
        {
            ResetAgentPath();
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
            if (!isPlantSelected)
            {
                //We select a type of plant from the input and make a transparent version of it with no stats
                plantSelectedType = PlantSelectedType.TREE;
                SwitchCursorIcon(i_treeSeed);
                OnPlantSelectedWrapUp();
            }
        }
        else
        {
            string enInsufficentSeeds = "Insufficient seeds of that type";
            string frInsufficientSeeds = "Des graines insuffisantes de ce type";
            hudManager.ThrowPlayerWarning(enInsufficentSeeds, frInsufficientSeeds);
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
                OnPlantSelectedWrapUp();
            }
        }
        else
        {
            string enInsufficentSeeds = "Insufficient seeds of that type";
            string frInsufficientSeeds = "Des graines insuffisantes de ce type";
            hudManager.ThrowPlayerWarning(enInsufficentSeeds, frInsufficientSeeds);
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
                OnPlantSelectedWrapUp();
            }
        }
        else
        {
            string enInsufficentSeeds = "Insufficient seeds of that type";
            string frInsufficientSeeds = "Des graines insuffisantes de ce type";
            hudManager.ThrowPlayerWarning(enInsufficentSeeds, frInsufficientSeeds);
        }

    }

    //This is called at the end of each plant selection function, to capture shared functionality
    private void OnPlantSelectedWrapUp()
    {
        InitializePlantPreview();
        inPlantSelection_FSM = true;
        OnPowerStateChange();
    }



    /// <summary>
    /// THESE FUNCTIONS HANDLE WHEN THE PLAYER SELECTS A TILE TO PLANT ON
    /// AND ACTUALLY PLANTS
    /// </summary>
    //This function is called when the player selects a tile while in the planting controls
    public void PlantPlantingHandler()
    {
        //StartCoroutine(OnPlantPlanted());
        SetPlant();
    }

    //Handles if they're in a position to start planting a plant
    private IEnumerator OnPlantPlanted()
    {
        //Have to add checks to make sure they are on a tile at all
        if(selectedTile == null)
        {
            //Display error message
            string enInvalidTile = "Invalid plant placement";
            string frInvalidTile = "Placement des plantes non valides";
            hudManager.ThrowPlayerWarning(enInvalidTile, frInvalidTile);
            yield break;
        }
        //If the tile selected checks out
        else if (isPlantSelected && selectedTile.GetComponent<Cell>().tileValid)
        {
            inPlantSelection_FSM = false;
            
            if (Mathf.Abs((this.transform.position - selectedTile.transform.position).magnitude) < agent.stoppingDistance + 2)
            {
                //Set all our flags
                isPlantSelected = false;
                enrouteToPlant = false;
                animator.SetAnimationFlag("plant", true);
                inInteraction_FSM = true;
                ResetAgentPath();

                Cell activeTileCell = selectedTile.GetComponent<Cell>();
                pMovement.GetPlayerGeo().LookAt(this.transform);

                //Pause other controls, initiate animation
                StartCoroutine(SuspendActions(animator.GetAnimationWaitTime("plant")));
                s_soundLibrary.PlayPlantClips();

                //After wait time
                yield return animator.GetAnimationWaitTime("plant");

                //Return our flags off
                animator.SetAnimationFlag("plant", false);
                inInteraction_FSM = false;
                
                //Create the plant and reset our variables
                PlantPlant(activeTileCell);
                plantSelectedType = PlantSelectedType.NONE;
            }
            //If we're not close enough, we need to start heading towards the tile selected
            else
            {
                ApproachTile();
            }
        }
        //If they're trying to select an invalid tile
        else if (isPlantSelected && !selectedTile.GetComponent<Cell>().tileValid)
        {
            //Display error message
            string enInvalidTile = "Invalid plant placement";
            string frInvalidTile = "Placement des plantes non valides";

            hudManager.ThrowPlayerWarning(enInvalidTile, frInvalidTile);
            yield break;
        }
        //Unknown use case?
        else
        {
            yield break;
        }
    }

    //Called after a player selects a tile to plant on
    public void SetPlant()
    {
        //Have to add checks to make sure they are on a tile at all
        if (selectedTile == null)
        {
            //Display error message
            string enInvalidTile = "Invalid plant placement";
            string frInvalidTile = "Placement des plantes non valides";
            hudManager.ThrowPlayerWarning(enInvalidTile, frInvalidTile);
        }
        //If the tile selected checks out
        else if (isPlantSelected && selectedTile.GetComponent<Cell>().tileValid)
        {
            inPlantSelection_FSM = false;

            if (Mathf.Abs((this.transform.position - selectedTile.transform.position).magnitude) <= agent.stoppingDistance + 1)
            {
                //Set all our flags
                isPlantSelected = false;
                enrouteToPlant = false;
                isPlanting = true;
                ResetAgentPath();
                animator.SetAnimationFlag("plant", true);

                pMovement.GetPlayerGeo().LookAt(this.transform);

                //Pause other controls, initiate animation
                inInteraction_FSM = true;
                SuspendActions(true);
                s_soundLibrary.PlayPlantClips();
            }
            //If we're not close enough, we need to start heading towards the tile selected
            else
            {
                ApproachTile();
            }
        }
        //If they're trying to select an invalid tile
        else if (isPlantSelected && !selectedTile.GetComponent<Cell>().tileValid)
        {
            //Display error message
            string enInvalidTile = "Invalid plant placement";
            string frInvalidTile = "Placement des plantes non valides";
            hudManager.ThrowPlayerWarning(enInvalidTile, frInvalidTile);
        }
    }

    public void HandlePlantAnimation()
    {
        if (isPlanting)
        {
            CreatePlant();
            PlantWrapUp();
        }
        else if (isRemoving)
        {
            Cell activeTileCell = selectedTile.GetComponent<Cell>();
            RemovePlant(activeTileCell);
            PlantRemoveWrapUp();
        }
    }

    //Create the plant
    public void CreatePlant()
    {
        Cell activeTileCell = selectedTile.GetComponent<Cell>();
        PlantPlant(activeTileCell);
    }

    public void PlantWrapUp()
    {
        inInteraction_FSM = false;
        animator.SetAnimationFlag("plant", false);
        animator.SetInSoftLock(true);
        plantSelectedType = PlantSelectedType.NONE;
        isPlanting = false;
    }

    //Call if the player is too far from a tile they selected to plant
    private void ApproachTile()
    {
        SetAgentPath(selectedTile.transform.position);
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
                //GetComponent<playerMovement>().ResetNavAgent();
                ResetAgentPath();
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
            string enRemoveWarning = "No plants to remove";
            string frRemoveWarning = "Pas de plantes à éliminer";
            hudManager.ThrowPlayerWarning(enRemoveWarning, frRemoveWarning);
        }
        
    }

    //Called when the player selects a tile to remove a plant from
    public void PlantRemovingHandler()
    {
        //StartCoroutine(OnPlantRemoved());
        SetPlantRemoval();
    }

    private IEnumerator OnPlantRemoved()
    {
        //Have to add checks to make sure they are on a tile
        if (selectedTile == null)
        {
            //Display error message
            string enInvalidTile = "No tile selected";
            string frInvalidTile = "Aucune tuile sélectionnée";
            hudManager.ThrowPlayerWarning(enInvalidTile, frInvalidTile);
            yield break;
        }
        //Check if the tile they highlighted has a plant on it
        if (selectedTile.GetComponent<Cell>().tileIsActivated && selectedTile.GetComponent<Cell>().tileHasBuild)
        {
            inRemovalSelection_FSM = false;

            //If we're close enough to the plant, we can go ahead and remove it
            if (Mathf.Abs((this.transform.position - selectedTile.transform.position).magnitude) < agent.stoppingDistance + 2)
            {
                //Set all our flags
                isRemovalStarted = false;
                enrouteToPlant = false;
                animator.SetAnimationFlag("plant", true);
                inInteraction_FSM = true;
                ResetAgentPath();

                Cell activeTileCell = selectedTile.GetComponent<Cell>();
                pMovement.GetPlayerGeo().LookAt(this.transform);

                //Pause other controls and start animations
                StartCoroutine(SuspendActions(animator.GetAnimationWaitTime("plant")));
                s_soundLibrary.PlayPlantClips();

                yield return animator.GetAnimationWaitTime("plant");

                //Return our flags off
                inInteraction_FSM = false;
                animator.SetAnimationFlag("plant", false);

                RemovePlant(activeTileCell);
            }
            //If we're not close enough, we'll have to get close enough
            else
            {
                ApproachTile();
            }
        }
        //If the tile has no build
        else if (selectedTile.GetComponent<Cell>().tileIsActivated && !selectedTile.GetComponent<Cell>().tileHasBuild)
        {
            string enInvalidTile = "No valid objects to remove";
            string frInvalidTile = "Pas d'objets valides à supprimer";

            hudManager.ThrowPlayerWarning(enInvalidTile, frInvalidTile);
        }
        else
        {
            yield break;
        }
    }

    private void SetPlantRemoval()
    {
        //Have to add checks to make sure they are on a tile
        if (selectedTile == null)
        {
            //Display error message
            string enInvalidTile = "No tile selected";
            string frInvalidTile = "Aucune tuile sélectionnée";
            hudManager.ThrowPlayerWarning(enInvalidTile, frInvalidTile);
        }
        //Check if the tile they highlighted has a plant on it
        if (selectedTile.GetComponent<Cell>().tileIsActivated && selectedTile.GetComponent<Cell>().tileHasBuild)
        {
            inRemovalSelection_FSM = false;

            //If we're close enough to the plant, we can go ahead and remove it
            if (Mathf.Abs((this.transform.position - selectedTile.transform.position).magnitude) <= agent.stoppingDistance + 1)
            {
                //Set all our flags
                isRemovalStarted = false;
                enrouteToPlant = false;
                inInteraction_FSM = true;
                isRemoving = true;
                ResetAgentPath();

                pMovement.GetPlayerGeo().LookAt(this.transform);

                //Pause other controls and start animations
                SuspendActions(true);
                s_soundLibrary.PlayPlantClips();
                animator.SetAnimationFlag("plant", true);
            }
            //If we're not close enough, we'll have to get close enough
            else
            {
                ApproachTile();
            }
        }
        //If the tile has no build
        else if (selectedTile.GetComponent<Cell>().tileIsActivated && !selectedTile.GetComponent<Cell>().tileHasBuild)
        {
            string enInvalidTile = "No valid objects to remove";
            string frInvalidTile = "Pas d'objets valides à supprimer";

            hudManager.ThrowPlayerWarning(enInvalidTile, frInvalidTile);
        }
    }

    public void PlantRemoveWrapUp()
    {
        //Return our flags off
        inInteraction_FSM = false;
        animator.SetAnimationFlag("plant", false);
        animator.SetInSoftLock(true);
        isRemoving = false;
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
                ResetAgentPath();
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
        animator.SetAnimationFlag("carry", true);
    }

    public void PutDownNyssa()
    {
        inHoldingNyssa_FSM = false;
        animator.SetAnimationFlag("carry", false);
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
            string enOnCooldown = "That ability is still on cooldown";
            string frOnCooldown = "Cette capacité est en temps de recharge";
            hudManager.ThrowPlayerWarning(enOnCooldown, frOnCooldown);
        }
        else if (!CheckIfValidTargets())
        {
            string enNoValidTargets = "There are no valid targets nearby";
            string frNoValidTargets = "Il n'y a pas de cibles valides à proximité";
            hudManager.ThrowPlayerWarning(enNoValidTargets, frNoValidTargets);
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
        //Set our flags
        inHealSelection_FSM = false;
        inInteraction_FSM = true;
        animator.SetAnimationFlag("castHeal", true);
        
        CallSuspendActions(animator.GetAnimationWaitTime("castHeal"));
        soundLibrary.PlaySpellClips();

        //Wait for the action to finish
        yield return animator.GetAnimationWaitTime("castHeal");

        //Reset our flags
        inInteraction_FSM = false;
        animator.SetAnimationFlag("castHeal", false);
        
        //Apply the hit points healed
        if (powerTarget.GetComponent<Player>())
        {
            powerTarget.GetComponent<Player>().TakeHit(-20);
        }
        else if (powerTarget.GetComponent<Animal>())
        {
            powerTarget.GetComponent<Animal>().IsHealed();
        }
        
        //Re-enable the overlay
        earthControls.controls.EarthPlayerDefault.Enable();
        hudManager.ToggleSproutPanel(true);

        //Handle the cooldowns
        healOnCooldown = true;
        StartCoroutine(HandleHealCooldown());
        StartCooldownUI("CastHeal", healCooldown);
    }

    //Resets the cooldown on the heal
    private IEnumerator HandleHealCooldown()
    {
        OnPowerStateChange();
        yield return new WaitForSeconds(healCooldown);
        healOnCooldown = false;
        OnPowerStateChange();
    }

    //If the player backs out after initiating picking a heal target
    public void OnHealingCancelled()
    {
        inHealSelection_FSM = false;
        OnPowerStateChange();
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
            string enOnCooldown = "That ability is still on cooldown";
            string frOnCooldown = "Cette capacité est en temps de recharge";
            hudManager.ThrowPlayerWarning(enOnCooldown, frOnCooldown);
        }
        else if (!CheckIfValidTargets())
        {
            string enNoValidTargets = "There are no valid targets nearby";
            string frNoValidTargets = "Il n'y a pas de cibles valides à proximité";
            hudManager.ThrowPlayerWarning(enNoValidTargets, frNoValidTargets);
        }
    }

    public void InitiateBarrier()
    {
        //StartCoroutine(BarrierStarted());
        SetBarrier();
    }

    public void SetBarrier()
    {
        //Set our flags
        inBarrierSelection_FSM = false;
        inInteraction_FSM = true;
        animator.SetAnimationFlag("castBarrier", true);

        SuspendActions(true);
        soundLibrary.PlaySpellClips();
    }

    private IEnumerator BarrierStarted()
    {
        //Wait for the action to finish
        yield return animator.GetAnimationWaitTime("castBarrier");

        animator.SetAnimationFlag("castBarrier", false);
    }

    public void TurnShieldOn()
    {
        //Apply the shield and the effect
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

        //Handle the cooldowns
        barrierOnCooldown = true;
        StartCoroutine(HandleShieldCooldown());
        StartCooldownUI("CastBarrier", barrierCooldown);
        StartCoroutine(HandleShieldExpiry());
    }

    public void BarrierWrapUp()
    {
        //Reset our flags
        inInteraction_FSM = false;

        SuspendActions(false);

        //Re-enable the overlay
        earthControls.controls.EarthPlayerDefault.Enable();
        hudManager.ToggleSproutPanel(true);
    }

    private IEnumerator HandleShieldCooldown()
    {
        OnPowerStateChange();
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
    //Used when casting a spell to see if any targets in the spell range can receive the spell
    private bool CheckIfValidTargets()
    {
        //Start our valid targets over
        validTargets.Clear();
        //Add ourselves to the list
        validTargets.Add(this.gameObject);

        //If we're in spell range of Celeste, add her
        if(JudgeDistance(celestialPlayer.transform.position, this.gameObject.transform.position, spellRange))
        {
            validTargets.Add(celestialPlayer.gameObject);
        }

        //If we're in spell range of an animal, add them
        foreach(GameObject animal in animalList.Items)
        {
            if (JudgeDistance(animal.transform.position, this.transform.position, spellRange))
            {
                validTargets.Add(animal.gameObject);
            }
        }
        //If we found any targets (which I guess we will since the player is always a valid target), say yes
        if(validTargets.Count > 0)
        {
            return true;
        }
        return false;
    }

    //Used when we're switching between targets for a spell
    public void OnCycleTargets(bool right)
    {
        //Debug.Log("Player is cycling targets, there are " + validTargets.Count + " targets to cycle through");
        if (right && validTargets.Count > 1)
        {
            if (validTargetIndex < validTargets.Count - 1)
            {
                //Debug.Log("Valid target index is: " + validTargetIndex + " before switching, power target was " + powerTarget);
                validTargetIndex++;
                powerTarget = validTargets[validTargetIndex];
                //Debug.Log("Valid target index is: " + validTargetIndex + " after switching, power target is " + powerTarget);
            }
            else
            {
                //Debug.Log("Valid target index is: " + validTargetIndex + " before switching, power target was " + powerTarget);
                validTargetIndex = 0;
                powerTarget = validTargets[validTargetIndex];
                //Debug.Log("Valid target index is: " + validTargetIndex + " after switching, power target is " + powerTarget);
            }
            tileOutline.transform.position = powerTarget.transform.position;
            SetTurnTarget(powerTarget);
        }
        else if(!right && validTargets.Count > 1)
        {
            if (validTargetIndex > 0)
            {
                //Debug.Log("Valid target index is: " + validTargetIndex + " before switching, power target was " + powerTarget);
                validTargetIndex--;
                powerTarget = validTargets[validTargetIndex];
                //Debug.Log("Valid target index is: " + validTargetIndex + " after switching, power target is " + powerTarget);
            }
            else
            {
                //Debug.Log("Valid target index is: " + validTargetIndex + " before switching, power target was " + powerTarget);
                validTargetIndex = validTargets.Count - 1;
                powerTarget = validTargets[validTargetIndex];
                //Debug.Log("Valid target index is: " + validTargetIndex + " after switching, power target is " + powerTarget);
            }
            tileOutline.transform.position = powerTarget.transform.position;
            SetTurnTarget(powerTarget);
        }
        else
        {
            //Debug.Log("Valid target count wasn't high enough to switch targets: " + validTargets.Count);
        }
    }

    


    /// <summary>
    /// OTHER ANIMATION STUFF
    /// </summary>
    public void BuildWrapUp()
    {
        animator.SetAnimationFlag("build", false);
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
        hudManager.ThrowPlayerWarning("Please select a tile", "Veuillez sélectionner une tuile");
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

    public GameObject GetPlantControlsUI()
    {
        return plantControlsUI;
    }

    public GameObject GetCastControlsUI()
    {
        return castControlsUI;
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

    public void SetTurnTarget(GameObject target)
    {
        pMovement.SetTurnTarget(target);
    }

    public void ToggleTurning(bool turning)
    {
        //isTurning = turning;
        pMovement.SetTurning(turning);
    }

    

    /// <summary>
    /// HELPER FUNCTIONS FOR STATE MACHINE
    /// </summary>
    /// <returns></returns>
    //Call this if you want to have all player controls turned off for a certain amount of time
    protected override IEnumerator SuspendActions(WaitForSeconds waitTime)
    {
        hudManager.SetSproutOccupied(true);
        OnPowerStateChange();
        yield return waitTime;
        hudManager.SetSproutOccupied(false);
        OnPowerStateChange();
    }

    //Toggles whether actions are suspended or not for no defined time
    public override void SuspendActions(bool suspend)
    {
        if (suspend)
        {
            hudManager.SetSproutOccupied(true);
        }
        else
        {
            hudManager.SetSproutOccupied(false);
        }
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

    /// <summary>
    /// STATE MACHINE GETTERS
    /// </summary>
    /// <returns></returns>

    //Return if the player is in the middle of interacting
    public bool GetInInteraction()
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

    public bool GetInMenu()
    {
        return inInGameMenu_FSM;
    }

    public void SetInMenu(bool inMenu)
    {
        inInGameMenu_FSM = inMenu;
    }

    public bool GetMidAnimation()
    {
        return isMidAnimation_FSM;
    }

    public bool GetIsWaiting()
    {
        return inWaiting_FSM;
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

    public void SetLanguageState(UserSettingsManager.GameLanguage gameLanguage)
    {

    }
}
