using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.VFX;

public class LevelOneEvents : LevelEventManager
{
    [Header("Specific to level 1")]
    [Header("Scene Data")]
    [SerializeField] LevelOneProgress levelOneProgress;

    [Header("Water related events")]
    [SerializeField] Material cleanWaterMaterial;
    //[SerializeField] GameObject river;
    //[SerializeField] GameObject lake;
    [SerializeField] GameObject water;

    [Header("Tile grids")]
    [SerializeField] GameObject tileGrid;
    [SerializeField] GameObject firstAreaGrid;
    [SerializeField] GameObject secondAreaGrid;
    [SerializeField] GameObject thirdAreaGrid;
    [SerializeField] GameObject fourthAreaGrid;

    List<GameObject> firstAreaTiles = new List<GameObject>();
    List<GameObject> secondAreaTiles = new List<GameObject>();
    List<GameObject> thirdAreaTiles = new List<GameObject>();
    List<GameObject> fourthAreaTiles = new List<GameObject>();

    private bool firstAreaClear = false;
    private bool secondAreaClear = false;
    private bool thirdAreaClear = false;
    //private bool fourthAreaClear = false;
    private bool fifthAreaClear = false;

    [Header("Terrain variables")]
    [SerializeField] private Vector3 areaOneMinVals;
    [SerializeField] private Vector3 areaOneMaxVals;
    [SerializeField] private Vector3 areaTwoMinVals;
    [SerializeField] private Vector3 areaTwoMaxVals;
    [SerializeField] private Vector3 areaThreeMinVals;
    [SerializeField] private Vector3 areaThreeMaxVals;
    
    [SerializeField] private Vector3 areaFour1MinVals;
    [SerializeField] private Vector3 areaFour1MaxVals;
    [SerializeField] private Vector3 areaFour2MinVals;
    [SerializeField] private Vector3 areaFour2MaxVals;

    [SerializeField] private int grassLayer;
    [SerializeField] private int dirtLayer;
    [SerializeField] private int grassSteps;
    [SerializeField] private int dirtSteps;


    //[Header("Objectives")]
    /*
    [SerializeField] GameObject objectiveListEN;
    [SerializeField] GameObject objectiveListFR;
    [SerializeField] TaskListManager task1;
    [SerializeField] TaskListManager task2;
    [SerializeField] TaskListManager task3;
    [SerializeField] TaskListManager task4;
    [SerializeField] TaskListManager task5;
    [SerializeField] TaskListManager task6;
    [SerializeField] TaskListManager task7;
    */

    private GameObject grassSeedSpawn;
    private GameObject treeSeedSpawn;

    [Header("Dialogue triggers")]
    [SerializeField] GameObject leaveTriggerObj;
    public DialogueTrigger firstMonsterDeadDialouge;
    public DialogueTrigger secondMonsterDeadDialouge;
    public DialogueTrigger thirdMonsterDeadDialouge;
    public DialogueTrigger fourthMonsterDeadDialouge;
    public DialogueTrigger allMonstersDefeatedDialogue;
    public DialogueTrigger allObjectivesMetDialogue;

    [Header("PowerDrop")]
    [SerializeField] private PowerBehaviour power;
    private GameObject powerDrop;

    /*
    [Header("UI indicators")]
    [SerializeField] PlantingUIIndicator treeIndicator;
    [SerializeField] PlantingUIIndicator flowerIndicator;
    [SerializeField] PlantingUIIndicator grassIndicator;
    */

    [Header("Ducks")]
    [SerializeField] private GameObject duck1;
    [SerializeField] private GameObject duck2;

    int keyMonsterDefeatCount;
    int totalMonsters = 6;
    int areaFiveMonsterCount = 2;

    private bool runDefeatDialogue = false;
    private bool runReadyToLeaveDialogue = false;
    private bool hasEncounteredBridge = false;

    WaitForSeconds delayTime = new WaitForSeconds(0.1f);
    WaitForSeconds extraDelayTime = new WaitForSeconds(1);

    //If different conversion milestones have been met
    private bool areaOneMOneMet = false;
    private bool areaOneMTwoMet = false;
    private bool areaTwoMOneMet = false;
    private bool areaTwoMTwoMet = false;
    private bool areaThreeMOneMet = false;
    private bool areaThreeMTwoMet = false;
    private bool areaFourMOneMet = false;
    private bool areaFourMTwoMet = false;

    [Header("Visual Update Assets")]
    
    [SerializeField] private GameObject factory;
    [SerializeField] private GameObject plasticBagInvaderWaypoint;
    [SerializeField] private List<GameObject> pipes;
    [SerializeField] private GameObject tank;
    [SerializeField] private Material cleanFactoryMaterial;
    [SerializeField] private Material cleanPipeMaterial;
    [SerializeField] private Material cleanTankMaterial;

    bool setClean = false;
    bool setSuperClean = false;

    private void Awake()
    {
        levelOneProgress.SetEventManager(this);
        soundPlayers = AmbienceManager.Instance.GetComponents<SoundPlayer>();
        weatherState.SetEventManager(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        //SetObjectiveLanguage();

        //soundPlayers = AmbienceManager.Instance.GetComponents<SoundPlayer>();

        //levelOneProgress = GetComponent<LevelOneProgress>();
        foreach (Transform childTransform in firstAreaGrid.transform)
        {
            firstAreaTiles.Add(childTransform.gameObject);
        }

        foreach (Transform childTransform in secondAreaGrid.transform)
        {
            secondAreaTiles.Add(childTransform.gameObject);
        }

        foreach (Transform childTransform in thirdAreaGrid.transform)
        {
            thirdAreaTiles.Add(childTransform.gameObject);
        }

        foreach (Transform childTransform in fourthAreaGrid.transform)
        {
            fourthAreaTiles.Add(childTransform.gameObject);
        }

        foreach (GameObject player in playerSet.Items)
        {
            if (player.GetComponent<CelestialPlayer>())
            {
                celestialPlayer = player.GetComponent<CelestialPlayer>();
                SetCelestialPowers();
            }
            else if (player.GetComponent<EarthPlayer>())
            {
                earthPlayer = player.GetComponent<EarthPlayer>();
                earthPlayer.inventory = levelOneProgress.GetInventory();
            }
        }

        duck1.SetActive(true);
        duck2.SetActive(true);
        dialogueManager.SetActive(true);

        InitializeTerrain();
        if (!hasFlipped)
        {
            DuplicateTerrain();
        }

        StartCoroutine(EvaluateFoodLevel());
        StartCoroutine(EvaluateBeautyLevel());

        soundPlayers[1].Play(wind, 0.5f);

        weatherState.SetEventManager(this);
    }

    private void OnDisable()
    {
       ResetTerrain();
    }

    // Update is called once per frame
    void Update()
    {
        //EvaluateFoodLevel();
        if (!runDefeatDialogue)
        {
            EvaluateMonsterDefeats();
        }

        if (EvaluateLevelCompletion() && !runReadyToLeaveDialogue)
        {
            OnReadyToLeave();
        }
        levelOneProgress.totalPlants = levelOneProgress.GetTotalPlantCount();
    }



    /// <summary>
    /// EVALUATE OBJECTIVES FROM LIST
    /// </summary>
    /// <returns></returns>
    private IEnumerator EvaluateFoodLevel()
    {
        while (true)
        {
            yield return delayTime;

            if (levelOneProgress.EvaluateTrees())
            {
                levelOneProgress.GetTask(0).CrossOutTask();
            }
            if (levelOneProgress.EvaluateGrass())
            {
                levelOneProgress.GetTask(1).CrossOutTask();
            }
            if (levelOneProgress.EvaluateCattails())
            {
                levelOneProgress.GetTask(2).CrossOutTask();
            }

            UpdateObjective();
            SetFoodCompletion();
        }

    }

    //Evaluate tasks 1-3: food
    private void SetFoodCompletion()
    {
        if (levelOneProgress.GetTask(0).GetTaskCompletion() && levelOneProgress.GetTask(1).GetTaskCompletion() && levelOneProgress.GetTask(2).GetTaskCompletion())
        {
            uiSoundLibrary.PlayProgressClips();
            levelOneProgress.animalHasEnoughFood = true;
        }
    }

    //Evaluate task 4: find a friend for the duck
    private void SetFriendCompletion()
    {
        if (levelOneProgress.GetTask(3).GetTaskCompletion())
        {
            uiSoundLibrary.PlayProgressClips();
            levelOneProgress.animalHasFriend = true;
        }
    }

    //Evaluate task 5: secure clean water
    private void SetWaterCompletion()
    {
        if (levelOneProgress.GetTask(4).GetTaskCompletion())
        {
            uiSoundLibrary.PlayProgressClips();
            levelOneProgress.animalHasWater = true;
        }
    }

    //Evaluate task 6: clear out the monsters
    private void EvaluateMonsterDefeats()
    {
        if (keyMonsterDefeatCount == totalMonsters)
        {
            levelOneProgress.GetTask(5).CrossOutTask();
            SetSafetyCompletion();
            runDefeatDialogue = true;
            allMonstersDefeatedDialogue.TriggerDialogue();
        }
    }

    //Cross the safety task out
    private void SetSafetyCompletion()
    {
        if (levelOneProgress.GetTask(5).GetTaskCompletion())
        {
            uiSoundLibrary.PlayProgressClips();
            levelOneProgress.animalIsSafe = true;
        }
    }


    //Final level evaluation
    private bool EvaluateLevelCompletion()
    {
        if (levelOneProgress.GetFoodStatus() && levelOneProgress.GetWaterStatus() && levelOneProgress.GetFriendStatus() && levelOneProgress.GetSafetyStatus())
        {
            return true;
        }
        return false;
    }



    /// <summary>
    /// FUNCTIONS FOR HANDLING LEVEL VISUAL UPDATES
    /// </summary>
    //This function runs the overall level updates
    private IEnumerator EvaluateBeautyLevel()
    {
        while (true)
        {
            yield return delayTime;
            if (firstAreaClear)
            {
                EvaluateAreaOne();
            }
            if (secondAreaClear)
            {
                EvaluateAreaTwo();
            }
            if (thirdAreaClear)
            {
                EvaluateAreaThree();
            }
            if (fifthAreaClear)
            {
                EvaluateAreaFour();
            }

            int plantCount = 0;
            int tileCount = firstAreaTiles.Count + secondAreaTiles.Count + thirdAreaTiles.Count + fourthAreaTiles.Count;

            //Calculate the total amount of plants that have been planted
            plantCount = levelOneProgress.totalPlants;

            //ambientSoundPlayer.SetVolume(plantCount / tileCount);

            //We want to start changing up the ambient sounds as the map gets improved
            if ((plantCount > tileCount / 6) && (plantCount < tileCount / 4))
            {
                if (setClean)
                {
                    soundPlayers[0].Stop(0.5f);
                    soundPlayers[2].Stop(0.5f);
                    GetComponent<DayNightCycle>().SwapSkyColours(false);
                    setClean = false;
                    setSuperClean = false;
                }
                soundPlayers[1].SetVolume((tileCount / plantCount) / 1);
            }
            else if ((plantCount > tileCount / 4) && (plantCount < tileCount / 2))
            {
                if (!setClean)
                {
                    soundPlayers[0].Play(music, 0.5f);
                    GetComponent<DayNightCycle>().SwapSkyColours(true);
                    setClean = true;
                }
                if (setSuperClean)
                {
                    soundPlayers[2].Stop(0.5f);
                    setSuperClean = false;
                }
                soundPlayers[0].SetVolume(plantCount / tileCount);
                soundPlayers[1].SetVolume((tileCount / plantCount) / 1);
            }
            else if (plantCount > tileCount / 2)
            {
                if (!setSuperClean)
                {
                    soundPlayers[1].Stop(0.5f);
                    soundPlayers[2].Play(nature, 0.5f);
                    setSuperClean = true;
                }
                soundPlayers[0].SetVolume(plantCount / tileCount);
                soundPlayers[2].SetVolume(plantCount / tileCount);

            }
        }
        
    }

    //Handles the first area where players spawn
    private void EvaluateAreaOne()
    {
        int plantCount = 0;
        int tileCount = firstAreaTiles.Count;
        foreach (GameObject go in firstAreaTiles)
        {
            if (go.GetComponentInChildren<Plant>())
            {
                plantCount++;
            }
        }
        //Once the player reaches the first milestone
        if (plantCount > tileCount / 4 && plantCount < tileCount / 2)
        {
            if (!areaOneMOneMet)
            {
                //Main switch to dirt
                ApplyTextureChangeOverArea((int)areaOneMinVals.x, (int)areaOneMaxVals.x, (int)areaOneMinVals.z, (int)areaOneMaxVals.z, dirtSteps, dirtLayer, 1);
                //Switch the path back
                //ApplyTextureChangeOverArea(-47, -43, -88, -21, 4, 4, 0.1f);

                areaOneMOneMet = true;
            }
        }
        //Once the player reaches the second milestone
        else if (plantCount > tileCount / 2)
        {
            if (!areaOneMTwoMet)
            {
                //Main switch to grass
                ApplyTextureChangeOverArea((int)areaOneMinVals.x, (int)areaOneMaxVals.x, (int)areaOneMinVals.z, (int)areaOneMaxVals.z, grassSteps, grassLayer, 1);
                //Switch the path back
                //ApplyTextureChangeOverArea(-47, -43, -88, -21, 4, 4, 0.1f);

                areaOneMTwoMet = true;
            }
        }
    }

    //Handles the platform directly above area one
    private void EvaluateAreaTwo()
    {
        int plantCount = 0;
        int tileCount = secondAreaTiles.Count;
        foreach (GameObject go in secondAreaTiles)
        {
            if (go.GetComponentInChildren<Plant>())
            {
                plantCount++;
            }
        }
        if (plantCount > tileCount / 4 && plantCount < tileCount / 2)
        {
            if (!areaTwoMOneMet)
            {
                //Main switch to dirt
                ApplyTextureChangeOverArea((int)areaTwoMinVals.x, (int)areaTwoMaxVals.x, (int)areaTwoMinVals.z, (int)areaTwoMaxVals.z, dirtSteps, dirtLayer, 1);

                areaTwoMOneMet = true;
            }
        }
        else if (plantCount > tileCount / 2)
        {
            if (!areaTwoMTwoMet)
            {
                //Main switch to grass
                ApplyTextureChangeOverArea((int)areaTwoMinVals.x, (int)areaTwoMaxVals.x, (int)areaTwoMinVals.z, (int)areaTwoMaxVals.z, grassSteps, grassLayer, 1);

                areaTwoMTwoMet = true;
            }
        }
    }

    //Handles the far bank, across the bridge
    private void EvaluateAreaThree()
    {
        int plantCount = 0;
        int tileCount = thirdAreaTiles.Count;
        foreach (GameObject go in thirdAreaTiles)
        {
            if (go.GetComponentInChildren<Plant>())
            {
                plantCount++;
            }
        }
        if (plantCount > tileCount / 4 && plantCount < tileCount / 2)
        {
            if (!areaThreeMOneMet)
            {
                //Main switch to dirt texture
                ApplyTextureChangeOverArea((int)areaThreeMinVals.x, (int)areaThreeMaxVals.x, (int)areaThreeMinVals.z, (int)areaThreeMaxVals.z, dirtSteps, dirtLayer, 1);
                //Switch part of it to a path
                //ApplyTextureChangeOverArea(-45, -41, 2, 74, 4, 4, 0.1f);

                areaThreeMOneMet = true;
            }
        }
        else if (plantCount > tileCount / 2)
        {
            if (!areaThreeMTwoMet)
            {
                //Main area grass texture
                ApplyTextureChangeOverArea((int)areaThreeMinVals.x, (int)areaThreeMaxVals.x, (int)areaThreeMinVals.z, (int)areaThreeMaxVals.z, grassSteps, grassLayer, 1);
                //Maintain the path
                //ApplyTextureChangeOverArea(-45, -41, 2, 74, 4, 4, 0.1f);

                areaThreeMTwoMet = true;
            }
        }
    }

    //Handles the sludge pump area
    private void EvaluateAreaFour()
    {
        int plantCount = 0;
        int tileCount = fourthAreaTiles.Count;
        foreach (GameObject go in fourthAreaTiles)
        {
            if (go.GetComponentInChildren<Plant>())
            {
                plantCount++;
            }
        }
        if (plantCount > tileCount / 4 && plantCount < tileCount / 2)
        {
            if (!areaFourMOneMet)
            {


                //Sludge pump area
                ApplyTextureChangeOverArea((int)areaFour1MinVals.x, (int)areaFour1MaxVals.x, (int)areaFour1MinVals.z, (int)areaFour1MaxVals.z, dirtSteps, dirtLayer, 1);
                //Middle platform
                ApplyTextureChangeOverArea((int)areaFour1MinVals.x, (int)areaFour1MaxVals.x, (int)areaFour1MinVals.z, (int)areaFour1MaxVals.z, grassSteps, grassLayer, 1);

                areaFourMOneMet = true;
            }
        }
        else if (plantCount > tileCount / 2)
        {
            if (!areaFourMTwoMet)
            {
                //Sludge pump area
                ApplyTextureChangeOverArea((int)areaFour2MinVals.x, (int)areaFour2MaxVals.x, (int)areaFour2MinVals.z, (int)areaFour2MaxVals.z, dirtSteps, dirtLayer, 1);
                //Middle platform
                ApplyTextureChangeOverArea((int)areaFour2MinVals.x, (int)areaFour2MaxVals.x, (int)areaFour2MinVals.z, (int)areaFour2MaxVals.z, grassSteps, grassLayer, 1);

                factory.GetComponent<MeshRenderer>().material = cleanFactoryMaterial;
                foreach(GameObject go in pipes)
                {
                    go.GetComponent<MeshRenderer>().material = cleanPipeMaterial;
                }
                tank.GetComponent<MeshRenderer>().material = cleanTankMaterial;

                areaFourMTwoMet = true;
            }
        }
    }




    /// <summary>
    /// EVENTS RELATED TO DEFEATING MONSTERS
    /// </summary>
    public void OnFirstMonsterDefeated(Enemy enemy)
    {

        foreach (GameObject go in firstAreaTiles)
        {
            if (go.GetComponent<Cell>().terrainType == Cell.TerrainType.DIRT)
            {
                go.GetComponent<Cell>().enviroState = Cell.EnviroState.CLEAN;
            }
        }
        Vector3 enemyPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 3, enemy.transform.position.z);
        grassSeedSpawn = Instantiate(levelOneProgress.grassSeedPrefab, enemyPos, Quaternion.identity);
        grassSeedSpawn.GetComponent<PickupObject>().SetInventory(levelOneProgress.GetInventory());
        grassSeedSpawn = Instantiate(levelOneProgress.grassSeedPrefab, new Vector3(enemyPos.x + 1, enemyPos.y, enemyPos.z - 1), Quaternion.identity);
        grassSeedSpawn.GetComponent<PickupObject>().SetInventory(levelOneProgress.GetInventory());
        grassSeedSpawn = Instantiate(levelOneProgress.grassSeedPrefab, new Vector3(enemyPos.x - 1, enemyPos.y, enemyPos.z + 1), Quaternion.identity);
        grassSeedSpawn.GetComponent<PickupObject>().SetInventory(levelOneProgress.GetInventory());
        levelOneProgress.animalHasShelter = true;
        powerDrop = Instantiate( power.ColdSnapStats.powerDropPrefab, enemyPos, Quaternion.identity);
        plasticBagInvaderWaypoint.SetActive(true);


        //We want to activate the objective menu here probably, or once the trigger dialogue is done.
        ////////////////////////////////////////////this is where we are going to drop the celestial cold orb!!!//////////////////////////////////////////////
        ///   GetComponent<CelestialPlayer>().PowerDrop((GetComponent<PowerBehaviour>().ColdSnapStats), enemyPos);



        firstMonsterDeadDialouge.TriggerDialogue();
        keyMonsterDefeatCount++;
        //objectiveContainer.SetActive(true);
        hudManager.ToggleObjectivesState(true);
        firstAreaClear = true;
        duck1.GetComponent<Duck>().Unstuck();
    }

    public void OnSecondMonsterDefeated(Enemy enemy)
    {
        foreach (GameObject go in secondAreaTiles)
        {
            if (go.GetComponent<Cell>().terrainType == Cell.TerrainType.DIRT)
            {
                go.GetComponent<Cell>().enviroState = Cell.EnviroState.CLEAN;
            }
        }
        Vector3 enemyPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 3, enemy.transform.position.z);
        treeSeedSpawn = Instantiate(levelOneProgress.treeSeedPrefab, enemyPos, Quaternion.identity);
        treeSeedSpawn.GetComponent<PickupObject>().SetInventory(levelOneProgress.GetInventory());
        treeSeedSpawn = Instantiate(levelOneProgress.treeSeedPrefab, new Vector3(enemyPos.x + 1, enemyPos.y, enemyPos.z - 1), Quaternion.identity);
        treeSeedSpawn.GetComponent<PickupObject>().SetInventory(levelOneProgress.GetInventory());
        treeSeedSpawn = Instantiate(levelOneProgress.treeSeedPrefab, new Vector3(enemyPos.x - 1, enemyPos.y, enemyPos.z + 1), Quaternion.identity);
        treeSeedSpawn.GetComponent<PickupObject>().SetInventory(levelOneProgress.GetInventory());

        keyMonsterDefeatCount++;

        //Reinstantiate these when we actually have behaviours for them
        //duck1.GetComponent<Duck>().SetUpperBankOn();
        //duck2.GetComponent<Duck>().SetUpperBankOn();

        secondAreaClear = true;

        secondMonsterDeadDialouge.TriggerDialogue();
    }

    public void OnThirdMonsterDefeated()
    {
        foreach (GameObject go in thirdAreaTiles)
        {
            if (go.GetComponent<Cell>().terrainType == Cell.TerrainType.DIRT)
            {
                go.GetComponent<Cell>().enviroState = Cell.EnviroState.CLEAN;
            }
        }

        duck2.GetComponent<Duck>().Unstuck();

        //duck1.GetComponent<Duck>().SetFarBankOn();
        //duck2.GetComponent<Duck>().SetFarBankOn();

        thirdAreaClear = true;

        keyMonsterDefeatCount++;
    }

    public void OnFourthMonsterDefeated()
    {
        //duck1.GetComponent<Duck>().SetHalfwayPointOn();
        //duck2.GetComponent<Duck>().SetHalfwayPointOn();

        keyMonsterDefeatCount++;
        //fourthAreaClear = true;


        levelOneProgress.GetTask(3).CrossOutTask();
        SetFriendCompletion();
        fourthMonsterDeadDialouge.TriggerDialogue();
    }

    public void OnFifthMonstersDefeated()
    {
        foreach (GameObject go in fourthAreaTiles)
        {
            if (go.GetComponent<Cell>().terrainType == Cell.TerrainType.DIRT)
            {
                go.GetComponent<Cell>().enviroState = Cell.EnviroState.CLEAN;
            }
        }

        //duck1.GetComponent<Duck>().SetTopAreaOn();
        //duck2.GetComponent<Duck>().SetTopAreaOn();

        fifthAreaClear = true;
    }

    public void CountDownFinalMonsters()
    {
        
        keyMonsterDefeatCount++;
        areaFiveMonsterCount -= 1;
        if (areaFiveMonsterCount <= 0)
        {
            OnFifthMonstersDefeated();
        }
    }

    /// <summary>
    /// OTHER EVENTS
    /// </summary>
    public void OnPumpShutOff()
    {
        water.GetComponent<Renderer>().material = cleanWaterMaterial;
        //river.GetComponent<Renderer>().material = cleanWaterMaterial;
        //lake.GetComponent<Renderer>().material = cleanWaterMaterial;
        foreach (GameObject go in firstAreaTiles)
        {
            if (go.GetComponent<Cell>().terrainType == Cell.TerrainType.WATER)
            {
                go.GetComponent<Cell>().enviroState = Cell.EnviroState.CLEAN;
            }
        }
        foreach (GameObject go in thirdAreaTiles)
        {
            if (go.GetComponent<Cell>().terrainType == Cell.TerrainType.WATER)
            {
                go.GetComponent<Cell>().enviroState = Cell.EnviroState.CLEAN;
            }
        }

        levelOneProgress.GetTask(4).CrossOutTask();
        SetWaterCompletion();
    }

    public void OnBridgeEncountered()
    {
        if (!hasEncounteredBridge)
        {
            //levelOneProgress.GetTask(6).gameObject.SetActive(true);
            ActivateTask(6, true);
            hasEncounteredBridge = true;
        }
    }

    public void OnBridgeBuilt()
    {
        levelOneProgress.GetTask(6).CrossOutTask();
    }


    private void SetCelestialPowers()
    {
        celestialPlayer.GetComponent<PowerBehaviour>().setDisabled(power.ColdSnapStats);
        celestialPlayer.GetComponent<PowerBehaviour>().setEnabled(power.LightningStats);
        celestialPlayer.GetComponent<PowerBehaviour>().setDisabled(power.MoonTideAttackStats);
        levelOneProgress.SetPowers(false);
    }



    /// <summary>
    /// WHEN THE PLAYER HAS COMPLETED ALL OBJECTIVES
    /// </summary>
    public void OnReadyToLeave()
    {
        //Set the leave trigger to exit the level to on
        runReadyToLeaveDialogue = true;
        allObjectivesMetDialogue.TriggerDialogue();
        leaveTriggerObj.SetActive(true);
    }


    /// <summary>
    /// DEBUG TOOLS
    /// </summary>
    public void DebugTileFlip()
    {
        foreach (GameObject go in firstAreaTiles)
        {
            if (go.GetComponent<Cell>().terrainType == Cell.TerrainType.DIRT)
            {
                go.GetComponent<Cell>().enviroState = Cell.EnviroState.CLEAN;
            }
        }
        foreach (GameObject go in secondAreaTiles)
        {
            if (go.GetComponent<Cell>().terrainType == Cell.TerrainType.DIRT)
            {
                go.GetComponent<Cell>().enviroState = Cell.EnviroState.CLEAN;
            }
        }
        foreach (GameObject go in thirdAreaTiles)
        {
            if (go.GetComponent<Cell>().terrainType == Cell.TerrainType.DIRT)
            {
                go.GetComponent<Cell>().enviroState = Cell.EnviroState.CLEAN;
            }
        }
    }

    public void DebugTeleport()
    {

    }

    /*
    protected void SetObjectiveLanguage()
    {
        if(userSettingsManager.chosenLanguage == UserSettingsManager.GameLanguage.ENGLISH)
        {
            objectiveListFR.SetActive(false);
            objectiveListEN.SetActive(true);
            task1 = objectiveListEN.transform.GetChild(6).GetComponent<TaskListManager>();
            task2 = objectiveListEN.transform.GetChild(5).GetComponent<TaskListManager>();
            task3 = objectiveListEN.transform.GetChild(4).GetComponent<TaskListManager>();
            task4 = objectiveListEN.transform.GetChild(3).GetComponent<TaskListManager>();
            task5 = objectiveListEN.transform.GetChild(2).GetComponent<TaskListManager>();
            task6 = objectiveListEN.transform.GetChild(1).GetComponent<TaskListManager>();
            task7 = objectiveListEN.transform.GetChild(0).GetComponent<TaskListManager>();
        }
        else if(userSettingsManager.chosenLanguage == UserSettingsManager.GameLanguage.FRENCH)
        {
            objectiveListEN.SetActive(false);
            objectiveListFR.SetActive(true);
            task1 = objectiveListFR.transform.GetChild(6).GetComponent<TaskListManager>();
            task2 = objectiveListFR.transform.GetChild(5).GetComponent<TaskListManager>();
            task3 = objectiveListFR.transform.GetChild(4).GetComponent<TaskListManager>();
            task4 = objectiveListFR.transform.GetChild(3).GetComponent<TaskListManager>();
            task5 = objectiveListFR.transform.GetChild(2).GetComponent<TaskListManager>();
            task6 = objectiveListFR.transform.GetChild(1).GetComponent<TaskListManager>();
            task7 = objectiveListFR.transform.GetChild(0).GetComponent<TaskListManager>();
        }
    }
    */

    //Sets the objectives based on language and completion
    protected void UpdateObjective()
    {
        //Display each task as either strikethrough or not
        if (userSettingsManager.chosenLanguage == UserSettingsManager.GameLanguage.ENGLISH)
        {
            if (levelOneProgress.GetTask(0).GetTaskCompletion())
            {
                levelOneProgress.GetTask(0).GetComponent<TextMeshProUGUI>().text = $"<s>- Plant {levelOneProgress.GetTreeGoal()} trees ({levelOneProgress.GetTreeCount()}/{levelOneProgress.GetTreeGoal()})</s>";
            }
            else
            {
                levelOneProgress.GetTask(0).GetComponent<TextMeshProUGUI>().text = $"- Plant {levelOneProgress.GetTreeGoal()} trees ({levelOneProgress.GetTreeCount()}/{levelOneProgress.GetTreeGoal()})";
            }
            if (levelOneProgress.GetTask(1).GetTaskCompletion())
            {
                levelOneProgress.GetTask(1).GetComponent<TextMeshProUGUI>().text = $"<s>- Plant {levelOneProgress.GetGrassGoal()} grass ({levelOneProgress.GetGrassCount()}/{levelOneProgress.GetGrassGoal()})</s>";
            }
            else
            {
                levelOneProgress.GetTask(1).GetComponent<TextMeshProUGUI>().text = $"- Plant {levelOneProgress.GetGrassGoal()} grass ({levelOneProgress.GetGrassCount()}/{levelOneProgress.GetGrassGoal()})";
            }
            if (levelOneProgress.GetTask(2).GetTaskCompletion())
            {
                levelOneProgress.GetTask(2).GetComponent<TextMeshProUGUI>().text = $"<s>- Plant {levelOneProgress.GetCattailGoal()} cattails ({levelOneProgress.GetCattailCount()}/{levelOneProgress.GetCattailGoal()})</s>";
            }
            else
            {
                levelOneProgress.GetTask(2).GetComponent<TextMeshProUGUI>().text = $"- Plant {levelOneProgress.GetCattailGoal()} cattails ({levelOneProgress.GetCattailCount()}/{levelOneProgress.GetCattailGoal()})";
            }
        }
        else if (userSettingsManager.chosenLanguage == UserSettingsManager.GameLanguage.FRENCH)
        {
            if (levelOneProgress.GetTask(0).GetTaskCompletion())
            {
                levelOneProgress.GetTask(0).GetComponent<TextMeshProUGUI>().text = $"<s>- Planter {levelOneProgress.GetTreeGoal()} arbres ({levelOneProgress.GetTreeCount()}/{levelOneProgress.GetTreeGoal()})</s>";
            }
            else
            {
                levelOneProgress.GetTask(0).GetComponent<TextMeshProUGUI>().text = $"- Planter {levelOneProgress.GetTreeGoal()} arbres ({levelOneProgress.GetTreeCount()}/{levelOneProgress.GetTreeGoal()})";
            }
            if (levelOneProgress.GetTask(1).GetTaskCompletion())
            {
                levelOneProgress.GetTask(1).GetComponent<TextMeshProUGUI>().text = $"<s>- Planter {levelOneProgress.GetGrassGoal()} herbes ({levelOneProgress.GetGrassCount()}/{levelOneProgress.GetGrassGoal()})</s>";
            }
            else
            {
                levelOneProgress.GetTask(1).GetComponent<TextMeshProUGUI>().text = $"- Planter {levelOneProgress.GetGrassGoal()} herbes ({levelOneProgress.GetGrassCount()}/{levelOneProgress.GetGrassGoal()})";
            }
            if (levelOneProgress.GetTask(2).GetTaskCompletion())
            {
                levelOneProgress.GetTask(2).GetComponent<TextMeshProUGUI>().text = $"<s>- Planter {levelOneProgress.GetCattailGoal()} quenouilles ({levelOneProgress.GetCattailCount()}/{levelOneProgress.GetCattailGoal()})</s>";
            }
            else
            {
                levelOneProgress.GetTask(2).GetComponent<TextMeshProUGUI>().text = $"- Planter {levelOneProgress.GetCattailGoal()} quenouilles ({levelOneProgress.GetCattailCount()}/{levelOneProgress.GetCattailGoal()})";
            }
        }
    }

    public override LevelProgress GetProgress()
    {
        return levelOneProgress;
    }

    public override void PopulateInventory()
    {
        
    }
}
