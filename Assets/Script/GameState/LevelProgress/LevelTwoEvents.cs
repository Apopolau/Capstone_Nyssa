using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelTwoEvents : LevelEventManager
{
    [Header("Specific to level 2")]
    [Header("Scene Data")]
    [SerializeField] LevelTwoProgress levelTwoProgress;
    
    [SerializeField] Item grassSeed;
    [SerializeField] Item treeSeed;
    [SerializeField] private PowerBehaviour power;
    [SerializeField] private GameObject spawn1;
    [SerializeField] private GameObject spawn2;
    //[SerializeField] private List<GameObject> animalKidnapIcons;

    [Header("Water related events")]
    [SerializeField] Material cleanWaterMaterial;
    [SerializeField] GameObject lake;

    [Header("Tile grids")]
    [SerializeField] GameObject tileGrid;
    [SerializeField] GameObject hedgehogAreaGrid;
    [SerializeField] GameObject mainAreaGrid;
    [SerializeField] GameObject farLeftGrid;
    [SerializeField] GameObject loggingAreaGrid;
    [SerializeField] GameObject buildingAreaGrid;
    [SerializeField] GameObject waterGrid;

    List<GameObject> mainAreaTiles = new List<GameObject>();
    List<GameObject> hedgehogAreaTiles = new List<GameObject>();
    List<GameObject> farLeftTiles = new List<GameObject>();
    List<GameObject> loggingAreaTiles = new List<GameObject>();
    List<GameObject> buildingAreaTiles = new List<GameObject>();
    List<GameObject> waterTiles = new List<GameObject>();

    [Header("Terrain variables")]
    [SerializeField] private Vector3 areaOneMinVals;
    [SerializeField] private Vector3 areaOneMaxVals;
    [SerializeField] private Vector3 areaTwoMinVals;
    [SerializeField] private Vector3 areaTwoMaxVals;
    [SerializeField] private Vector3 areaThreeMinVals;
    [SerializeField] private Vector3 areaThreeMaxVals;
    [SerializeField] private Vector3 areaFourMinVals;
    [SerializeField] private Vector3 areaFourMaxVals;

    [SerializeField] private int grassLayer;
    [SerializeField] private int dirtLayer;
    [SerializeField] private int grassSteps;
    [SerializeField] private int dirtSteps;

    private bool firstAreaClear = false;
    private bool secondAreaClear = false;
    private bool thirdAreaClear = false;
    private bool fourthAreaClear = false;

    private bool staticMonstersDefeated = false;
    private bool spawnsDestroyed = false;

    [Header("Objectives")]
    //[SerializeField] GameObject objectiveListEN;
    //[SerializeField] GameObject objectiveListFR;
    //[SerializeField] private TaskListManager[] task;
    //[SerializeField] TaskListManager task1;
    //[SerializeField] TaskListManager task2;
    //[SerializeField] TaskListManager task3;
    //[SerializeField] TaskListManager task4;
    //[SerializeField] TaskListManager task5;
    //[SerializeField] TaskListManager task6;
    //[SerializeField] TaskListManager task7;
    //[SerializeField] TaskListManager task8;

    private GameObject flowerSeedSpawn;
    private GameObject powerDrop;


    [Header("Dialogue triggers")]
    [SerializeField] GameObject leaveTriggerObj;
    public DialogueTrigger firstMonsterDeadDialouge;
    public DialogueTrigger secondMonsterDeadDialouge;
    public DialogueTrigger thirdMonsterDeadDialouge;
    public DialogueTrigger fourthMonsterDeadDialouge;
    public DialogueTrigger allMonstersDefeatedDialogue;
    public DialogueTrigger allObjectivesMetDialogue;

    [Header("Animals")]
    [SerializeField] private GameObject hog1;
    [SerializeField] private GameObject hog2;
    [SerializeField] private GameObject hog3;
    [SerializeField] private GameObject fox;
    [SerializeField] private GameObject nyssa;

    int keyMonsterDefeatCount;
    int monsterCount = 7;

    int area3MonsterCount = 2;
    int area4MonsterCount = 3;

    private bool runDefeatDialogue = false;
    private bool runReadyToLeaveDialogue = false;
    //private bool hasEncounteredLadder = false;
    //private bool hasFoundNyssa = false;

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
    [SerializeField] private GameObject facility;
    [SerializeField] private GameObject loggingBuilding;

    bool setClean = false;
    bool setSuperClean = false;

    private void Awake()
    {
        levelTwoProgress.SetEventManager(this);
        soundPlayers = AmbienceManager.Instance.GetComponents<SoundPlayer>();
        weatherState.SetEventManager(this);
        //PopulateInventory();
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform childTransform in hedgehogAreaGrid.transform)
        {
            hedgehogAreaTiles.Add(childTransform.gameObject);
        }

        foreach (Transform childTransform in mainAreaGrid.transform)
        {
            mainAreaTiles.Add(childTransform.gameObject);
        }

        foreach (Transform childTransform in farLeftGrid.transform)
        {
            farLeftTiles.Add(childTransform.gameObject);
        }

        foreach (Transform childTransform in loggingAreaGrid.transform)
        {
            loggingAreaTiles.Add(childTransform.gameObject);
        }

        foreach (Transform childTransform in buildingAreaGrid.transform)
        {
            buildingAreaTiles.Add(childTransform.gameObject);
        }

        foreach(Transform childTransform in waterGrid.transform)
        {
            waterTiles.Add(childTransform.gameObject);
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
                earthPlayer.inventory = levelTwoProgress.GetInventory();
            }
        }

        hog1.SetActive(true);
        hog2.SetActive(true);
        hog3.SetActive(true);
        fox.SetActive(true);
        nyssa.SetActive(true);


        InitializeTerrain();
        if (!hasFlipped)
        {
            DuplicateTerrain();
        }

        StartCoroutine(EvaluateFoodLevel());
        StartCoroutine(EvaluateBeautyLevel());

        earthPlayer.earthControls.controls.EarthPlayerDefault.Enable();
        celestialPlayer.celestialControls.controls.CelestialPlayerDefault.Enable();

        soundPlayers[1].Play(wind, 0.5f);
    }

    private void OnDisable()
    {
        ResetTerrain();
    }

    // Update is called once per frame
    void Update()
    {
        if (!runDefeatDialogue)
        {
            EvaluateMonsterDefeats();
        }

        if (EvaluateLevelCompletion() && !runReadyToLeaveDialogue)
        {
            OnReadyToLeave();
        }
        levelTwoProgress.totalPlants = levelTwoProgress.GetTotalPlantCount();
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

            if (levelTwoProgress.EvaluateTrees())
            {
                levelTwoProgress.GetTask(0).CrossOutTask();
            }
            if (levelTwoProgress.EvaluateGrass())
            {
                levelTwoProgress.GetTask(1).CrossOutTask();
            }
            if (levelTwoProgress.EvaluateCattails())
            {
                levelTwoProgress.GetTask(2).CrossOutTask();
            }
            if (levelTwoProgress.EvaluateFlowers())
            {
                levelTwoProgress.GetTask(3).CrossOutTask();
            }
            if (levelTwoProgress.EvaluateLilies())
            {
                levelTwoProgress.GetTask(4).CrossOutTask();
            }
            UpdateObjective();
            SetFoodCompletion();
        }

    }

    //Evaluate tasks 1-5: food (trees, grass, cattails, flowers, lilies)
    private void SetFoodCompletion()
    {
        if (levelTwoProgress.GetTask(0).GetTaskCompletion() && levelTwoProgress.GetTask(1).GetTaskCompletion() && levelTwoProgress.GetTask(2).GetTaskCompletion()
            && levelTwoProgress.GetTask(3).GetTaskCompletion() && levelTwoProgress.GetTask(4).GetTaskCompletion())
        {
            uiSoundLibrary.PlayProgressClips();
            levelTwoProgress.animalHasEnoughFood = true;
        }
    }

    //Set the water to clean, complete evaluation of task 6
    private void SetWaterCompletion()
    {
        levelTwoProgress.GetTask(5).CrossOutTask();
        if (levelTwoProgress.GetTask(5).GetTaskCompletion())
        {
            uiSoundLibrary.PlayProgressClips();
            levelTwoProgress.animalHasWater = true;
        }
    }

    //Set the level to safe, complete evaluation of task 7
    private void SetSafetyCompletion()
    {
        levelTwoProgress.GetTask(6).CrossOutTask();
        if (levelTwoProgress.GetTask(6).GetTaskCompletion())
        {
            uiSoundLibrary.PlayProgressClips();
            levelTwoProgress.animalIsSafe = true;
        }
    }

    //Set the hedgehog to have some friends, complete evaluation of task 8
    private void SetFriendCompletion()
    {
        levelTwoProgress.GetTask(7).CrossOutTask();
        if (levelTwoProgress.GetTask(7).GetTaskCompletion())
        {
            uiSoundLibrary.PlayProgressClips();
            levelTwoProgress.animalHasFriend = true;
        }
    }

    //CHANGE THIS TO ACCOUNT FOR SPAWNS
    private void EvaluateMonsterDefeats()
    {
        if (keyMonsterDefeatCount >= monsterCount)
        {
            staticMonstersDefeated = true;
            runDefeatDialogue = true;
            allMonstersDefeatedDialogue.TriggerDialogue();
        }
        //Set an evaluation for spawns here
        if (!spawn1.GetComponent<EnemySpawner>().GetSpawnsOn() && !spawn2.GetComponent<EnemySpawner>().GetSpawnsOn())
        {
            spawnsDestroyed = true;
        }
        //Finally if both are true, set this to true
        if (staticMonstersDefeated && spawnsDestroyed)
        {
            SetSafetyCompletion();
        }
    }

    private bool EvaluateLevelCompletion()
    {
        if (levelTwoProgress.GetFoodStatus() && levelTwoProgress.GetWaterStatus() && levelTwoProgress.GetFriendStatus() && levelTwoProgress.GetSafetyStatus())
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
            if (fourthAreaClear)
            {
                EvaluateAreaFour();
            }

            int plantCount = 0;
            int tileCount = hedgehogAreaTiles.Count + mainAreaTiles.Count + loggingAreaTiles.Count + buildingAreaTiles.Count;

            //Calculate the total amount of plants that have been planted
            plantCount = levelTwoProgress.totalPlants;

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
    /// <summary>
    /// CHANGE THIS TO MATCH APPROPRIATE TILE AREA
    /// </summary>
    private void EvaluateAreaOne()
    {
        int plantCount = 0;
        int tileCount = hedgehogAreaTiles.Count;
        foreach (GameObject go in hedgehogAreaTiles)
        {
            if (go.GetComponentInChildren<Plant>())
            {
                plantCount++;
            }
        }
        if (plantCount > tileCount / 4 && plantCount < tileCount / 2)
        {
            if (!areaOneMOneMet)
            {
                ApplyTextureChangeOverArea(172, 250, 6, 60, 8, 6, 1);
                areaOneMOneMet = true;
            }
        }
        else if (plantCount > tileCount / 2)
        {
            if (!areaOneMTwoMet)
            {
                ApplyTextureChangeOverArea(172, 250, 6, 60, 8, 2, 1);
                areaOneMTwoMet = true;
            }
        }
    }

    //Handles the main area by the lake
    private void EvaluateAreaTwo()
    {
        int plantCount = 0;
        int tileCount = mainAreaTiles.Count;
        foreach (GameObject go in mainAreaTiles)
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
                ApplyTextureChangeOverArea((int)areaOneMinVals.x, (int)areaOneMaxVals.x, (int)areaOneMinVals.z, (int)areaOneMaxVals.z, dirtSteps, dirtLayer, 1);
                areaTwoMOneMet = true;
            }
        }
        else if (plantCount > tileCount / 2)
        {
            if (!areaTwoMTwoMet)
            {
                ApplyTextureChangeOverArea((int)areaOneMinVals.x, (int)areaOneMaxVals.x, (int)areaOneMinVals.z, (int)areaOneMaxVals.z, grassSteps, grassLayer, 1);
                areaTwoMTwoMet = true;
            }
        }
    }

    //Handles the logging area
    private void EvaluateAreaThree()
    {
        int plantCount = 0;
        int tileCount = loggingAreaTiles.Count;
        foreach (GameObject go in loggingAreaTiles)
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
                ApplyTextureChangeOverArea((int)areaThreeMinVals.x, (int)areaThreeMaxVals.x, (int)areaThreeMinVals.z, (int)areaThreeMaxVals.z, dirtSteps, dirtLayer, 1);
                areaThreeMOneMet = true;
            }
        }
        else if (plantCount > tileCount / 2)
        {
            if (!areaThreeMTwoMet)
            {
                ApplyTextureChangeOverArea((int)areaThreeMinVals.x, (int)areaThreeMaxVals.x, (int)areaThreeMinVals.z, (int)areaThreeMaxVals.z, grassSteps, grassLayer, 1);
                areaThreeMTwoMet = true;
            }
        }
    }

    //Handles the building area
    private void EvaluateAreaFour()
    {
        int plantCount = 0;
        int tileCount = buildingAreaTiles.Count;
        foreach (GameObject go in buildingAreaTiles)
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
                ApplyTextureChangeOverArea((int)areaFourMinVals.x, (int)areaFourMinVals.x, (int)areaFourMinVals.z, (int)areaFourMinVals.z, dirtSteps, dirtLayer, 1);
                areaFourMOneMet = true;
            }
        }
        else if (plantCount > tileCount / 2)
        {
            if (!areaFourMTwoMet)
            {
                ApplyTextureChangeOverArea((int)areaFourMinVals.x, (int)areaFourMinVals.x, (int)areaFourMinVals.z, (int)areaFourMinVals.z, grassSteps, grassLayer, 1);

                areaFourMTwoMet = true;
            }
        }
    }

    //When the player defeats the first oil spill monster that is threatening the hedgehog
    public void OnFirstMonsterDefeated()
    {

        foreach (GameObject go in hedgehogAreaTiles)
        {
            if (go.GetComponent<Cell>().terrainType == Cell.TerrainType.DIRT)
            {
                go.GetComponent<Cell>().enviroState = Cell.EnviroState.CLEAN;
            }
        }

        levelTwoProgress.animalHasShelter = true;

        firstMonsterDeadDialouge.TriggerDialogue();
        keyMonsterDefeatCount++;
        //objectiveContainer.SetActive(true);
        hudManager.ToggleObjectivesState(true);
        firstAreaClear = true;
        hog1.GetComponent<Hedgehog>().Unstuck();
    }

    //When the player defeats the static smog monster in the lumber yard area
    public void OnSecondMonsterDefeated()
    {

        foreach (GameObject go in loggingAreaTiles)
        {
            if (go.GetComponent<Cell>().terrainType == Cell.TerrainType.DIRT)
            {
                go.GetComponent<Cell>().enviroState = Cell.EnviroState.CLEAN;
            }
        }
        secondMonsterDeadDialouge.TriggerDialogue();
        Vector3 enemyPos = new Vector3(dyingEnemy.transform.position.x, dyingEnemy.transform.position.y + 1, dyingEnemy.transform.position.z);
        powerDrop = Instantiate(power.MoonTideAttackStats.powerDropPrefab, enemyPos, Quaternion.identity);
        keyMonsterDefeatCount++;
        secondAreaClear = true;
        spawn1.GetComponent<EnemySpawner>().ToggleSpawns(false);
        //Cause Celeste's tidal wave to drop here
    }

    //When the player defeats the 2 oil monsters hanging out on the west side of the main area
    public void OnThirdMonsterDefeated()
    {
        foreach (GameObject go in mainAreaTiles)
        {
            if (go.GetComponent<Cell>().terrainType == Cell.TerrainType.DIRT)
            {
                go.GetComponent<Cell>().enviroState = Cell.EnviroState.CLEAN;
            }
        }

        foreach (GameObject go in farLeftTiles)
        {
            if (go.GetComponent<Cell>().terrainType == Cell.TerrainType.DIRT)
            {
                go.GetComponent<Cell>().enviroState = Cell.EnviroState.CLEAN;
            }
        }

        Vector3 enemyPos = new Vector3(dyingEnemy.transform.position.x, dyingEnemy.transform.position.y + 3, dyingEnemy.transform.position.z);
        flowerSeedSpawn = Instantiate(levelTwoProgress.flowerSeedPrefab, enemyPos, Quaternion.identity);
        flowerSeedSpawn.GetComponent<PickupObject>().SetInventory(levelTwoProgress.GetInventory());
        flowerSeedSpawn = Instantiate(levelTwoProgress.flowerSeedPrefab, new Vector3(enemyPos.x + 1, enemyPos.y, enemyPos.z - 1), Quaternion.identity);
        flowerSeedSpawn.GetComponent<PickupObject>().SetInventory(levelTwoProgress.GetInventory());
        flowerSeedSpawn = Instantiate(levelTwoProgress.flowerSeedPrefab, new Vector3(enemyPos.x - 1, enemyPos.y, enemyPos.z + 1), Quaternion.identity);
        flowerSeedSpawn.GetComponent<PickupObject>().SetInventory(levelTwoProgress.GetInventory());

        thirdAreaClear = true;
        fourthAreaClear = true;
    }

    //When the player defeats the last 3 oil monsters, over by the terminal, that are surrounding the hedgehog friends
    public void OnFourthMonsterDefeated()
    {

        foreach (GameObject go in buildingAreaTiles)
        {
            if (go.GetComponent<Cell>().terrainType == Cell.TerrainType.DIRT)
            {
                go.GetComponent<Cell>().enviroState = Cell.EnviroState.CLEAN;
            }
        }

        fourthAreaClear = true;
        levelTwoProgress.GetTask(7).CrossOutTask();
        SetFriendCompletion();
        fourthMonsterDeadDialouge.TriggerDialogue();
        hog2.GetComponent<Hedgehog>().Unstuck();
        hog3.GetComponent<Hedgehog>().Unstuck();
    }

    //Run this for each defeated monster assigned to this spot
    public void CountDownArea3Monsters()
    {
        keyMonsterDefeatCount += 1;
        area3MonsterCount -= 1;
        if (area3MonsterCount <= 0)
        {
            OnThirdMonsterDefeated();
        }
    }

    //Run this for each defeated monster assigned to this spot
    public void CountDownArea4Monsters()
    {
        keyMonsterDefeatCount += 1;
        area4MonsterCount -= 1;
        if (area4MonsterCount <= 0)
        {
            OnFourthMonsterDefeated();
        }
    }


    public void OnFacilityShutOff()
    {
        //river.GetComponent<Renderer>().material = cleanWaterMaterial;
        lake.GetComponent<Renderer>().material = cleanWaterMaterial;
        /*
        foreach (GameObject go in mainAreaTiles)
        {
            if (go.GetComponent<Cell>().terrainType == Cell.TerrainType.WATER)
            {
                go.GetComponent<Cell>().enviroState = Cell.EnviroState.CLEAN;
            }
        }
        foreach (GameObject go in buildingAreaTiles)
        {
            if (go.GetComponent<Cell>().terrainType == Cell.TerrainType.WATER)
            {
                go.GetComponent<Cell>().enviroState = Cell.EnviroState.CLEAN;
            }
        }
        */
        foreach(GameObject go in waterTiles)
        {
            if (go.GetComponent<Cell>().terrainType == Cell.TerrainType.WATER)
            {
                go.GetComponent<Cell>().enviroState = Cell.EnviroState.CLEAN;
            }
        }
        levelTwoProgress.GetTask(5).CrossOutTask();
        SetWaterCompletion();
        spawn2.GetComponent<EnemySpawner>().ToggleSpawns(false);
    }

    public void TurnOnSecondSpawner()
    {
        spawn2.GetComponent<EnemySpawner>().ToggleSpawns(true);
    }

    ///
    /// MISC
    /// 
    public override void PopulateInventory()
    {
        levelTwoProgress.GetInventory().AddItem(grassSeed, grassSeed.quantity);
        levelTwoProgress.GetInventory().AddItem(treeSeed, treeSeed.quantity);
    }

    private void SetCelestialPowers()
    {
        celestialPlayer.GetComponent<PowerBehaviour>().setEnabled(power.ColdSnapStats);
        celestialPlayer.GetComponent<PowerBehaviour>().setEnabled(power.LightningStats);
        celestialPlayer.GetComponent<PowerBehaviour>().setDisabled(power.MoonTideAttackStats);
        levelTwoProgress.SetPowers(false);
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

    public void DebugTeleport()
    {

    }

    /*
    protected void SetObjectiveLanguage()
    {
        
        if (userSettingsManager.chosenLanguage == UserSettingsManager.GameLanguage.ENGLISH)
        {
            
            objectiveListFR.SetActive(false);
            objectiveListEN.SetActive(true);
            task1 = objectiveListEN.transform.GetChild(7).GetComponent<TaskListManager>();
            task2 = objectiveListEN.transform.GetChild(6).GetComponent<TaskListManager>();
            task3 = objectiveListEN.transform.GetChild(5).GetComponent<TaskListManager>();
            task4 = objectiveListEN.transform.GetChild(4).GetComponent<TaskListManager>();
            task5 = objectiveListEN.transform.GetChild(3).GetComponent<TaskListManager>();
            task6 = objectiveListEN.transform.GetChild(2).GetComponent<TaskListManager>();
            task7 = objectiveListEN.transform.GetChild(1).GetComponent<TaskListManager>();
            task8 = objectiveListEN.transform.GetChild(0).GetComponent<TaskListManager>();
            
        }
        else if (userSettingsManager.chosenLanguage == UserSettingsManager.GameLanguage.FRENCH)
        {
            
            objectiveListEN.SetActive(false);
            objectiveListFR.SetActive(true);
            task1 = objectiveListFR.transform.GetChild(7).GetComponent<TaskListManager>();
            task2 = objectiveListFR.transform.GetChild(6).GetComponent<TaskListManager>();
            task3 = objectiveListFR.transform.GetChild(5).GetComponent<TaskListManager>();
            task4 = objectiveListFR.transform.GetChild(4).GetComponent<TaskListManager>();
            task5 = objectiveListFR.transform.GetChild(3).GetComponent<TaskListManager>();
            task6 = objectiveListFR.transform.GetChild(2).GetComponent<TaskListManager>();
            task7 = objectiveListFR.transform.GetChild(1).GetComponent<TaskListManager>();
            task8 = objectiveListFR.transform.GetChild(0).GetComponent<TaskListManager>();
        }
    }
    */

    //Sets the objectives based on language and completion
    protected void UpdateObjective()
    {
        //Display each task as either strikethrough or not
        //English
        if (userSettingsManager.chosenLanguage == UserSettingsManager.GameLanguage.ENGLISH)
        {
            //Task 1
            if (levelTwoProgress.GetTask(0).GetTaskCompletion())
            {
                levelTwoProgress.GetTask(0).GetComponent<TextMeshProUGUI>().text = $"<s>- Plant {levelTwoProgress.GetTreeGoal()} trees ({levelTwoProgress.GetTreeCount()}/{levelTwoProgress.GetTreeGoal()})</s>";
            }
            else
            {
                levelTwoProgress.GetTask(0).GetComponent<TextMeshProUGUI>().text = $"- Plant {levelTwoProgress.GetTreeGoal()} trees ({levelTwoProgress.GetTreeCount()}/{levelTwoProgress.GetTreeGoal()})";
            }
            //Task 2
            if (levelTwoProgress.GetTask(1).GetTaskCompletion())
            {
                levelTwoProgress.GetTask(1).GetComponent<TextMeshProUGUI>().text = $"<s>- Plant {levelTwoProgress.GetGrassGoal()} grass ({levelTwoProgress.GetGrassCount()}/{levelTwoProgress.GetGrassGoal()})</s>";
            }
            else
            {
                levelTwoProgress.GetTask(1).GetComponent<TextMeshProUGUI>().text = $"- Plant {levelTwoProgress.GetGrassGoal()} grass ({levelTwoProgress.GetGrassCount()}/{levelTwoProgress.GetGrassGoal()})";
            }
            //Task 3
            if (levelTwoProgress.GetTask(2).GetTaskCompletion())
            {
                levelTwoProgress.GetTask(2).GetComponent<TextMeshProUGUI>().text = $"<s>- Plant {levelTwoProgress.GetCattailGoal()} cattails ({levelTwoProgress.GetCattailCount()}/{levelTwoProgress.GetCattailGoal()})</s>";
            }
            else
            {
                levelTwoProgress.GetTask(2).GetComponent<TextMeshProUGUI>().text = $"- Plant {levelTwoProgress.GetCattailGoal()} cattails ({levelTwoProgress.GetCattailCount()}/{levelTwoProgress.GetCattailGoal()})";
            }
            //Task 4
            if (levelTwoProgress.GetTask(3).GetTaskCompletion())
            {
                levelTwoProgress.GetTask(3).GetComponent<TextMeshProUGUI>().text = $"- Plant {levelTwoProgress.GetFlowerGoal()} flowers ({levelTwoProgress.GetFlowerCount()}/{levelTwoProgress.GetFlowerGoal()})";
            }
            else
            {
                levelTwoProgress.GetTask(3).GetComponent<TextMeshProUGUI>().text = $"- Plant {levelTwoProgress.GetFlowerGoal()} flowers ({levelTwoProgress.GetFlowerCount()}/{levelTwoProgress.GetFlowerGoal()})";
            }
            //Task 5
            if (levelTwoProgress.GetTask(4).GetTaskCompletion())
            {
                levelTwoProgress.GetTask(4).GetComponent<TextMeshProUGUI>().text = $"- Plant {levelTwoProgress.GetLilyGoal()} lilies ({levelTwoProgress.GetLilyCount()}/{levelTwoProgress.GetLilyGoal()})";
            }
            else
            {
                levelTwoProgress.GetTask(4).GetComponent<TextMeshProUGUI>().text = $"- Plant {levelTwoProgress.GetLilyGoal()} lilies ({levelTwoProgress.GetLilyCount()}/{levelTwoProgress.GetLilyGoal()})";
            }
        }
        //French
        else if (userSettingsManager.chosenLanguage == UserSettingsManager.GameLanguage.FRENCH)
        {
            //Task 1
            if (levelTwoProgress.GetTask(0).GetTaskCompletion())
            {
                levelTwoProgress.GetTask(0).GetComponent<TextMeshProUGUI>().text = $"<s>- Planter {levelTwoProgress.GetTreeGoal()} arbres ({levelTwoProgress.GetTreeCount()}/{levelTwoProgress.GetTreeGoal()})</s>";
            }
            else
            {
                levelTwoProgress.GetTask(0).GetComponent<TextMeshProUGUI>().text = $"- Planter {levelTwoProgress.GetTreeGoal()} arbres ({levelTwoProgress.GetTreeCount()}/{levelTwoProgress.GetTreeGoal()})";
            }
            //Task 2
            if (levelTwoProgress.GetTask(1).GetTaskCompletion())
            {
                levelTwoProgress.GetTask(1).GetComponent<TextMeshProUGUI>().text = $"<s>- Planter {levelTwoProgress.GetGrassGoal()} herbes ({levelTwoProgress.GetGrassCount()}/{levelTwoProgress.GetGrassGoal()})</s>";
            }
            else
            {
                levelTwoProgress.GetTask(1).GetComponent<TextMeshProUGUI>().text = $"- Planter {levelTwoProgress.GetGrassGoal()} herbes ({levelTwoProgress.GetGrassCount()}/{levelTwoProgress.GetGrassGoal()})";
            }
            //Task 3
            if (levelTwoProgress.GetTask(2).GetTaskCompletion())
            {
                levelTwoProgress.GetTask(2).GetComponent<TextMeshProUGUI>().text = $"<s>- Planter {levelTwoProgress.GetCattailGoal()} quenouilles ({levelTwoProgress.GetCattailCount()}/{levelTwoProgress.GetCattailGoal()})</s>";
            }
            else
            {
                levelTwoProgress.GetTask(2).GetComponent<TextMeshProUGUI>().text = $"- Planter {levelTwoProgress.GetCattailGoal()} quenouilles ({levelTwoProgress.GetCattailCount()}/{levelTwoProgress.GetCattailGoal()})";
            }
            //Task 4
            if (levelTwoProgress.GetTask(3).GetTaskCompletion())
            {
                levelTwoProgress.GetTask(3).GetComponent<TextMeshProUGUI>().text = $"<s>- Planter {levelTwoProgress.GetFlowerGoal()} fleurs ({levelTwoProgress.GetFlowerCount()}/{levelTwoProgress.GetFlowerGoal()})</s>";
            }
            else
            {
                levelTwoProgress.GetTask(3).GetComponent<TextMeshProUGUI>().text = $"- Planter {levelTwoProgress.GetFlowerGoal()} fleurs ({levelTwoProgress.GetFlowerCount()}/{levelTwoProgress.GetFlowerGoal()})";
            }
            //Task 5
            if (levelTwoProgress.GetTask(4).GetTaskCompletion())
            {
                levelTwoProgress.GetTask(4).GetComponent<TextMeshProUGUI>().text = $"<s>- Planter {levelTwoProgress.GetLilyGoal()} lys ({levelTwoProgress.GetLilyCount()}/{levelTwoProgress.GetLilyGoal()})</s>";
            }
            else
            {
                levelTwoProgress.GetTask(4).GetComponent<TextMeshProUGUI>().text = $"- Planter {levelTwoProgress.GetLilyGoal()} lys ({levelTwoProgress.GetLilyCount()}/{levelTwoProgress.GetLilyGoal()})";
            }
        }
    }

    public override LevelProgress GetProgress()
    {
        return levelTwoProgress;
    }
}
