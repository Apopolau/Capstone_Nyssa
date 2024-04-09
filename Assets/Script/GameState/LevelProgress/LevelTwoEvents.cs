using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelTwoEvents : LevelEventManager
{
    [Header("Scene Data")]
    [SerializeField] LevelTwoProgress levelTwoProgress;
    [SerializeField] GameObject dialogueManager;
    [SerializeField] GameObject inventorySlotManager;
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

    List<GameObject> mainAreaTiles = new List<GameObject>();
    List<GameObject> hedgehogAreaTiles = new List<GameObject>();
    List<GameObject> farLeftTiles = new List<GameObject>();
    List<GameObject> loggingAreaTiles = new List<GameObject>();
    List<GameObject> buildingAreaTiles = new List<GameObject>();

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

    [Header("Objective tasks")]
    [SerializeField] TaskListManager task1;
    [SerializeField] TaskListManager task2;
    [SerializeField] TaskListManager task3;
    [SerializeField] TaskListManager task4;
    [SerializeField] TaskListManager task5;
    [SerializeField] TaskListManager task6;
    [SerializeField] TaskListManager task7;
    [SerializeField] TaskListManager task8;

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
    private bool hasEncounteredLadder = false;
    private bool hasFoundNyssa = false;

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

        inventorySlotManager.SetActive(true);

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
                PopulateInventory();
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
                task1.CrossOutTask();
            }
            if (levelTwoProgress.EvaluateGrass())
            {
                task2.CrossOutTask();
            }
            if (levelTwoProgress.EvaluateCattails())
            {
                task3.CrossOutTask();
            }
            if (levelTwoProgress.EvaluateFlowers())
            {
                task4.CrossOutTask();
            }
            if (levelTwoProgress.EvaluateLilies())
            {
                task5.CrossOutTask();
            }

            //Display each task as either strikethrough or not
            if (task1.GetTaskCompletion())
            {
                task1.GetComponent<TextMeshProUGUI>().text = $"<s>- Plant {levelTwoProgress.GetTreeGoal()} trees ({levelTwoProgress.GetTreeCount()}/{levelTwoProgress.GetTreeGoal()})</s>";
            }
            else
            {
                task1.GetComponent<TextMeshProUGUI>().text = $"- Plant {levelTwoProgress.GetTreeGoal()} trees ({levelTwoProgress.GetTreeCount()}/{levelTwoProgress.GetTreeGoal()})";
            }
            if (task2.GetTaskCompletion())
            {
                task2.GetComponent<TextMeshProUGUI>().text = $"<s>- Plant {levelTwoProgress.GetGrassGoal()} grass ({levelTwoProgress.GetGrassCount()}/{levelTwoProgress.GetGrassGoal()})</s>";
            }
            else
            {
                task2.GetComponent<TextMeshProUGUI>().text = $"- Plant {levelTwoProgress.GetGrassGoal()} grass ({levelTwoProgress.GetGrassCount()}/{levelTwoProgress.GetGrassGoal()})";
            }
            if (task3.GetTaskCompletion())
            {
                task3.GetComponent<TextMeshProUGUI>().text = $"<s>- Plant {levelTwoProgress.GetCattailGoal()} cattails ({levelTwoProgress.GetCattailCount()}/{levelTwoProgress.GetCattailGoal()})</s>";
            }
            else
            {
                task3.GetComponent<TextMeshProUGUI>().text = $"- Plant {levelTwoProgress.GetCattailGoal()} cattails ({levelTwoProgress.GetCattailCount()}/{levelTwoProgress.GetCattailGoal()})";
            }
            if (task4.GetTaskCompletion())
            {
                task4.GetComponent<TextMeshProUGUI>().text = $"- Plant {levelTwoProgress.GetFlowerGoal()} flowers ({levelTwoProgress.GetFlowerCount()}/{levelTwoProgress.GetFlowerGoal()})";
            }
            else
            {
                task4.GetComponent<TextMeshProUGUI>().text = $"- Plant {levelTwoProgress.GetFlowerGoal()} flowers ({levelTwoProgress.GetFlowerCount()}/{levelTwoProgress.GetFlowerGoal()})";
            }
            if (task5.GetTaskCompletion())
            {
                task5.GetComponent<TextMeshProUGUI>().text = $"- Plant {levelTwoProgress.GetLilyGoal()} lilies ({levelTwoProgress.GetLilyCount()}/{levelTwoProgress.GetLilyGoal()})";
            }
            else
            {
                task5.GetComponent<TextMeshProUGUI>().text = $"- Plant {levelTwoProgress.GetLilyGoal()} lilies ({levelTwoProgress.GetLilyCount()}/{levelTwoProgress.GetLilyGoal()})";
            }

            SetFoodCompletion();
        }

    }

    private void SetFoodCompletion()
    {
        if (task1.GetTaskCompletion() && task2.GetTaskCompletion() && task3.GetTaskCompletion()
            && task4.GetTaskCompletion() && task5.GetTaskCompletion())
        {
            uiSoundLibrary.PlayProgressClips();
            levelTwoProgress.animalHasEnoughFood = true;
        }
    }

    private void SetWaterCompletion()
    {
        task6.CrossOutTask();
        if (task6.GetTaskCompletion())
        {
            uiSoundLibrary.PlayProgressClips();
            levelTwoProgress.animalHasWater = true;
        }
    }

    private void SetSafetyCompletion()
    {
        task7.CrossOutTask();
        if (task7.GetTaskCompletion())
        {
            uiSoundLibrary.PlayProgressClips();
            levelTwoProgress.animalIsSafe = true;
        }
    }

    private void SetFriendCompletion()
    {
        task8.CrossOutTask();
        if (task8.GetTaskCompletion())
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
        if(!spawn1.activeSelf && !spawn2.activeSelf)
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
            if (plantCount > tileCount / 4)
            {
                if (setClean)
                {
                    GetComponent<DayNightCycle>().SwapSkyColours(false);
                    setClean = false;
                }
            }
            else if (plantCount > tileCount / 2)
            {
                if (!setClean)
                {
                    GetComponent<DayNightCycle>().SwapSkyColours(true);
                    setClean = true;
                }
            }
            else if (plantCount > tileCount / 4 + tileCount / 2)
            {

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
        objectiveList.SetActive(true);
        firstAreaClear = true;
        hog1.GetComponent<Hedgehog>().Unstuck();
    }

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
        spawn1.SetActive(false);
        //Cause Celeste's tidal wave to drop here
    }

    public void OnThirdMonsterDefeated()
    {
        foreach (GameObject go in mainAreaTiles)
        {
            //Debug.Log(go);
            if (go.GetComponent<Cell>().terrainType == Cell.TerrainType.DIRT)
            {
                go.GetComponent<Cell>().enviroState = Cell.EnviroState.CLEAN;
            }
        }

        foreach (GameObject go in farLeftTiles)
        {
            //Debug.Log(go);
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
        task8.CrossOutTask();
        SetFriendCompletion();
        fourthMonsterDeadDialouge.TriggerDialogue();
        hog2.GetComponent<Hedgehog>().Unstuck();
        hog3.GetComponent<Hedgehog>().Unstuck();
    }

    public void CountDownArea3Monsters()
    {
        keyMonsterDefeatCount += 1;
        area3MonsterCount -= 1;
        if (area3MonsterCount <= 0)
        {
            OnThirdMonsterDefeated();
        }
    }

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
        task6.CrossOutTask();
        SetWaterCompletion();
        spawn2.SetActive(false);
    }


    ///
    /// MISC
    /// 
    private void PopulateInventory()
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
}
