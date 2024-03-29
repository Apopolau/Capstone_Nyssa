using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelTwoEvents : EventManager
{
    [Header("Scene Data")]
    [SerializeField] LevelTwoProgress levelTwoProgress;

    [Header("Water related events")]
    [SerializeField] Material cleanWaterMaterial;
    [SerializeField] GameObject lake;

    [Header("Tile grids")]
    [SerializeField] GameObject tileGrid;
    [SerializeField] GameObject mainAreaGrid;
    [SerializeField] GameObject hedgehogAreaGrid;
    [SerializeField] GameObject loggingAreaGrid;
    [SerializeField] GameObject buildingAreaGrid;

    List<GameObject> mainAreaTiles = new List<GameObject>();
    List<GameObject> hedgehogAreaTiles = new List<GameObject>();
    List<GameObject> loggingAreaTiles = new List<GameObject>();
    List<GameObject> buildingAreaTiles = new List<GameObject>();

    private bool firstAreaClear = false;
    private bool secondAreaClear = false;
    private bool thirdAreaClear = false;
    private bool fourthAreaClear = false;

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

    [Header("Dialogue triggers")]
    [SerializeField] GameObject leaveTriggerObj;
    public DialogueTrigger firstMonsterDeadDialouge;
    public DialogueTrigger secondMonsterDeadDialouge;
    public DialogueTrigger thirdMonsterDeadDialouge;
    public DialogueTrigger fourthMonsterDeadDialouge;
    public DialogueTrigger allMonstersDefeatedDialogue;
    public DialogueTrigger allObjectivesMetDialogue;

    [Header("Hedgehogs")]
    [SerializeField] private GameObject hog1;
    [SerializeField] private GameObject hog2;
    [SerializeField] private GameObject nyssa;

    int keyMonsterDefeatCount;
    int monsterCount = 4;

    private bool runDefeatDialogue = false;
    private bool runReadyToLeaveDialogue = false;
    private bool hasEncounteredLadder = false;

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

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform childTransform in mainAreaGrid.transform)
        {
            mainAreaTiles.Add(childTransform.gameObject);
        }
        
        foreach (Transform childTransform in hedgehogAreaGrid.transform)
        {
            hedgehogAreaTiles.Add(childTransform.gameObject);
        }
        
        foreach (Transform childTransform in loggingAreaGrid.transform)
        {
            loggingAreaTiles.Add(childTransform.gameObject);
        }

        foreach (Transform childTransform in buildingAreaGrid.transform)
        {
            buildingAreaTiles.Add(childTransform.gameObject);
        }

        hog1.SetActive(true);
        hog2.SetActive(true);

        InitializeTerrain();

        StartCoroutine(EvaluateFoodLevel());
        StartCoroutine(EvaluateBeautyLevel());
    }

    // Update is called once per frame
    void Update()
    {
        if (!runDefeatDialogue)
        {
            EvaluateMonsterDefeats();
        }

        if(EvaluateLevelCompletion() && !runReadyToLeaveDialogue)
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
            levelTwoProgress.animalHasEnoughFood = true;
        }
    }

    private void SetWaterCompletion()
    {
        if (task6.GetTaskCompletion())
        {
            levelTwoProgress.animalHasWater = true;
        }
    }

    private void SetSafetyCompletion()
    {
        if (task7.GetTaskCompletion())
        {
            levelTwoProgress.animalIsSafe = true;
        }
    }

    private void SetFriendCompletion()
    {
        if (task8.GetTaskCompletion())
        {
            levelTwoProgress.animalHasFriend = true;
        }
    }

    //CHANGE THIS TO ACCOUNT FOR SPAWNS
    private void EvaluateMonsterDefeats()
    {
        if (keyMonsterDefeatCount == monsterCount)
        {
            task7.CrossOutTask();
            SetSafetyCompletion();
            runDefeatDialogue = true;
            allMonstersDefeatedDialogue.TriggerDialogue();
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

            }
            else if (plantCount > tileCount / 2)
            {

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
                //follow x axis
                /*
                for (int i = -100; i < -2; i += 8)
                {
                    //follow z axis
                    for (int j = -114; j < -10; j += 8)
                    {
                        Vector3 pos = new Vector3(i, 0, j);
                        ChangeTexture(pos, layers[1]);
                    }
                }
                */

                areaOneMOneMet = true;
            }
        }
        else if (plantCount > tileCount / 2)
        {
            if (!areaOneMTwoMet)
            {
                /*
                //follow x axis
                for (int i = -100; i < -2; i += 8)
                {
                    //follow z axis
                    for (int j = -114; j < -10; j += 8)
                    {
                        Vector3 pos = new Vector3(i, 0, j);
                        ChangeTexture(pos, layers[2]);
                    }
                }
                */
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
                /*
                //follow x axis
                for (int i = 7; i < 84; i += 8)
                {
                    //follow z axis
                    for (int j = -135; j < -21; j += 8)
                    {
                        Vector3 pos = new Vector3(i, 0, j);
                        ChangeTexture(pos, layers[1]);
                    }
                }
                */
                areaTwoMOneMet = true;
            }
        }
        else if (plantCount > tileCount / 2)
        {
            if (!areaTwoMTwoMet)
            {
                /*
                //follow x axis
                for (int i = 7; i < 84; i += 8)
                {
                    //follow z axis
                    for (int j = -135; j < -21; j += 8)
                    {
                        Vector3 pos = new Vector3(i, 0, j);
                        ChangeTexture(pos, layers[2]);
                    }
                }
                */
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
                /*
                //follow x axis
                for (int i = -100; i < -16; i += 8)
                {
                    //follow z axis
                    for (int j = -100; j < 0; j += 8)
                    {
                        Vector3 pos = new Vector3(i, 0, j);
                        ChangeTexture(pos, layers[1]);
                    }
                }
                */
                areaThreeMOneMet = true;
            }
        }
        else if (plantCount > tileCount / 2)
        {
            if (!areaThreeMTwoMet)
            {
                /*
                //follow x axis
                for (int i = -100; i < -16; i += 8)
                {
                    //follow z axis
                    for (int j = -100; j < 0; j += 8)
                    {
                        Vector3 pos = new Vector3(i, 0, j);
                        ChangeTexture(pos, layers[2]);
                    }
                }
                */
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
                //Sludge pump area
                //follow x axis
                /*
                for (int i = 96; i < 209; i += 8)
                {
                    //follow z axis
                    for (int j = -55; j < 85; j += 8)
                    {
                        Vector3 pos = new Vector3(i, 0, j);
                        ChangeTexture(pos, layers[1]);
                    }
                }
                //Middle platform
                for (int i = 2; i < 81; i += 8)
                {
                    //follow z axis
                    for (int j = 4; j < 92; j += 8)
                    {
                        Vector3 pos = new Vector3(i, 0, j);
                        ChangeTexture(pos, layers[1]);
                    }
                }
                */
                areaFourMOneMet = true;
            }
        }
        else if (plantCount > tileCount / 2)
        {
            if (!areaFourMTwoMet)
            {
                //Sludge pump area
                //follow x axis
                /*
                for (int i = 96; i < 209; i += 8)
                {
                    //follow z axis
                    for (int j = -55; j < 85; j += 8)
                    {
                        Vector3 pos = new Vector3(i, 0, j);
                        ChangeTexture(pos, layers[2]);
                    }
                }
                //Middle platform
                for (int i = 2; i < 81; i += 8)
                {
                    //follow z axis
                    for (int j = 4; j < 92; j += 8)
                    {
                        Vector3 pos = new Vector3(i, 0, j);
                        ChangeTexture(pos, layers[2]);
                    }
                }
                */

                /*
                factory.GetComponent<MeshRenderer>().material = cleanFactoryMaterial;
                foreach (GameObject go in pipes)
                {
                    go.GetComponent<MeshRenderer>().material = cleanPipeMaterial;
                }
                tank.GetComponent<MeshRenderer>().material = cleanTankMaterial;

                areaFourMTwoMet = true;
                */
            }
        }
    }

    public void OnFirstMonsterDefeated()
    {
        
        foreach (GameObject go in mainAreaTiles)
        {
            if(go.GetComponent<Cell>().terrainType == Cell.TerrainType.DIRT)
            {
                go.GetComponent<Cell>().enviroState = Cell.EnviroState.CLEAN;
            }
        }
        Vector3 enemyPos = new Vector3(dyingEnemy.transform.position.x, dyingEnemy.transform.position.y + 3, dyingEnemy.transform.position.z);
        flowerSeedSpawn = Instantiate(levelTwoProgress.flowerSeedPrefab, enemyPos, Quaternion.identity);
        flowerSeedSpawn = Instantiate(levelTwoProgress.flowerSeedPrefab, new Vector3(enemyPos.x + 1, enemyPos.y, enemyPos.z - 1), Quaternion.identity);
        flowerSeedSpawn = Instantiate(levelTwoProgress.flowerSeedPrefab, new Vector3(enemyPos.x - 1, enemyPos.y, enemyPos.z + 1), Quaternion.identity);
        levelTwoProgress.animalHasShelter = true;

        //We want to activate the objective menu here probably, or once the trigger dialogue is done.
        ////////////////////////////////////////////this is where we are going to drop the celestial cold orb!!!/////////////////////////////////////

        firstMonsterDeadDialouge.TriggerDialogue();
        keyMonsterDefeatCount++;
        objectiveList.SetActive(true);
        firstAreaClear = true;
        hog1.GetComponent<Hedgehog>().Unstuck();
        /*
        levelTwoProgress.shelter = true;
        task3.CrossOutTask();
        */
    }

    public void OnSecondMonsterDefeated()
    {

        foreach (GameObject go in hedgehogAreaTiles)
        {
            if (go.GetComponent<Cell>().terrainType == Cell.TerrainType.DIRT)
            {
                go.GetComponent<Cell>().enviroState = Cell.EnviroState.CLEAN;
            }
        }
        /*
        levelTwoProgress.shelter = true;
        task3.CrossOutTask();
        */
    }

    public void OnThirdMonsterDefeated()
    {

        foreach (GameObject go in loggingAreaTiles)
        {
            if (go.GetComponent<Cell>().terrainType == Cell.TerrainType.DIRT)
            {
                go.GetComponent<Cell>().enviroState = Cell.EnviroState.CLEAN;
            }
        }
        /*
        levelTwoProgress.shelter = true;
        task3.CrossOutTask();
        */
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
        /*
        levelTwoProgress.shelter = true;
        task3.CrossOutTask();
        */
    }

    /*
    public void OnPumpShutOff()
    {
        //river.GetComponent<Renderer>().material = cleanWaterMaterial;
        lake.GetComponent<Renderer>().material = cleanWaterMaterial;
        foreach(GameObject go in firstAreaTiles)
        {
            if(go.GetComponent<Cell>().terrainType == Cell.TerrainType.WATER)
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
        levelTwoProgress.cleanWater = true;
        task2.CrossOutTask();
    }
    */

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
