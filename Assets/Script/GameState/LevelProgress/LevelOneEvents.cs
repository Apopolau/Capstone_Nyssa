using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelOneEvents : EventManager
{
    [Header("Scene Data")]
    [SerializeField] LevelOneProgress levelOneProgress;

    [Header("Water related events")]
    [SerializeField] Material cleanWaterMaterial;
    //[SerializeField] GameObject river;
    //[SerializeField] GameObject lake;
    [SerializeField] GameObject water;

    [Header("Parent objects of tile grids")]
    [SerializeField] GameObject firstAreaGrid;
    [SerializeField] GameObject secondAreaGrid;
    [SerializeField] GameObject thirdAreaGrid;
    [SerializeField] GameObject fourthAreaGrid;

    List<GameObject> firstAreaTiles = new List<GameObject>();
    List<GameObject> secondAreaTiles = new List<GameObject>();
    List<GameObject> thirdAreaTiles = new List<GameObject>();
    List<GameObject> fourthAreaTiles = new List<GameObject>();

    [Header("Objective tasks")]
    [SerializeField] TaskListManager task1;
    [SerializeField] TaskListManager task2;
    [SerializeField] TaskListManager task3;
    [SerializeField] TaskListManager task4;
    [SerializeField] TaskListManager task5;
    [SerializeField] TaskListManager task6;
    [SerializeField] TaskListManager task7;

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

    [Header("Ducks")]
    [SerializeField] private GameObject duck1;
    [SerializeField] private GameObject duck2;

    int keyMonsterDefeatCount;

    private bool runDefeatDialogue = false;
    private bool runReadyToLeaveDialogue = false;
    private bool hasEncounteredBridge = false;

    WaitForSeconds delayTime = new WaitForSeconds(0.1f);


    // Start is called before the first frame update
    void Start()
    {
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
        duck1.SetActive(true);
        duck2.SetActive(true);

        StartCoroutine(EvaluateFoodLevel());
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

    private IEnumerator EvaluateFoodLevel()
    {
        while (true)
        {
            yield return delayTime;

            if (levelOneProgress.EvaluateTrees())
            {
                task1.CrossOutTask();
            }
            if (levelOneProgress.EvaluateGrass())
            {
                task2.CrossOutTask();
            }
            if (levelOneProgress.EvaluateCattails())
            {
                task3.CrossOutTask();
            }

            //Display each task as either strikethrough or not
            if (task1.GetTaskCompletion())
            {
                task1.GetComponent<TextMeshProUGUI>().text = $"<s>- Plant {levelOneProgress.GetTreeGoal()} trees ({levelOneProgress.GetTreeCount()}/{levelOneProgress.GetTreeGoal()})</s>" ;
            }
            else
            {
                task1.GetComponent<TextMeshProUGUI>().text = $"- Plant {levelOneProgress.GetTreeGoal()} trees ({levelOneProgress.GetTreeCount()}/{levelOneProgress.GetTreeGoal()})";
            }
            if (task2.GetTaskCompletion())
            {
                task2.GetComponent<TextMeshProUGUI>().text = $"<s>- Plant {levelOneProgress.GetGrassGoal()} grass ({levelOneProgress.GetGrassCount()}/{levelOneProgress.GetGrassGoal()})</s>";
            }
            else
            {
                task2.GetComponent<TextMeshProUGUI>().text = $"- Plant {levelOneProgress.GetGrassGoal()} grass ({levelOneProgress.GetGrassCount()}/{levelOneProgress.GetGrassGoal()})";
            }
            if (task3.GetTaskCompletion())
            {
                task3.GetComponent<TextMeshProUGUI>().text = $"<s>- Plant {levelOneProgress.GetCattailGoal()} cattails ({levelOneProgress.GetCattailCount()}/{levelOneProgress.GetCattailGoal()})</s>";
            }
            else
            {
                task3.GetComponent<TextMeshProUGUI>().text = $"- Plant {levelOneProgress.GetCattailGoal()} cattails ({levelOneProgress.GetCattailCount()}/{levelOneProgress.GetCattailGoal()})";
            }

            SetFoodCompletion();
        }
        
    }

    private void SetFoodCompletion()
    {
        if(task1.GetTaskCompletion() && task2.GetTaskCompletion() && task3.GetTaskCompletion())
        {
            levelOneProgress.animalHasEnoughFood = true;
        }
    }

    private void SetWaterCompletion()
    {
        if (task5.GetTaskCompletion())
        {
            levelOneProgress.animalHasWater = true;
        }
    }

    private void SetSafetyCompletion()
    {
        if (task6.GetTaskCompletion())
        {
            levelOneProgress.animalIsSafe = true;
        }
    }

    private void SetFriendCompletion()
    {
        if (task4.GetTaskCompletion())
        {
            levelOneProgress.animalHasFriend = true;
        }
    }

    private bool EvaluateLevelCompletion()
    {
        if(levelOneProgress.GetFoodStatus() && levelOneProgress.GetWaterStatus() && levelOneProgress.GetFriendStatus() && levelOneProgress.GetSafetyStatus())
        {
            return true;
        }
        return false;
    }

    private void EvaluateMonsterDefeats()
    {
        if (keyMonsterDefeatCount == 4)
        {
            task6.CrossOutTask();
            SetSafetyCompletion();
            runDefeatDialogue = true;
            allMonstersDefeatedDialogue.TriggerDialogue();
        }
    }

    public void OnFirstMonsterDefeated()
    {
        
        foreach (GameObject go in firstAreaTiles)
        {
            if(go.GetComponent<Cell>().terrainType == Cell.TerrainType.DIRT)
            {
                go.GetComponent<Cell>().enviroState = Cell.EnviroState.CLEAN;
            }
        }
        Vector3 enemyPos = new Vector3(dyingEnemy.transform.position.x, dyingEnemy.transform.position.y + 3, dyingEnemy.transform.position.z);
        treeSeedSpawn = Instantiate(levelOneProgress.treeSeedPrefab, enemyPos, Quaternion.identity);
        treeSeedSpawn = Instantiate(levelOneProgress.treeSeedPrefab, new Vector3(enemyPos.x + 1, enemyPos.y, enemyPos.z - 1), Quaternion.identity);
        treeSeedSpawn = Instantiate(levelOneProgress.treeSeedPrefab, new Vector3(enemyPos.x - 1, enemyPos.y, enemyPos.z + 1), Quaternion.identity);
        levelOneProgress.animalHasShelter = true;

        //We want to activate the objective menu here probably, or once the trigger dialogue is done.
        ////////////////////////////////////////////this is where we are going to drop the celestial cold orb!!!/////////////////////////////////////
        
        firstMonsterDeadDialouge.TriggerDialogue();
        keyMonsterDefeatCount++;
        objectiveList.SetActive(true);
        duck1.GetComponent<Duck>().Unstuck();
    }

    public void OnSecondMonsterDefeated()
    {
        foreach (GameObject go in secondAreaTiles)
        {
            if (go.GetComponent<Cell>().terrainType == Cell.TerrainType.DIRT)
            {
                go.GetComponent<Cell>().enviroState = Cell.EnviroState.CLEAN;
            }
        }
        Vector3 enemyPos = new Vector3(dyingEnemy.transform.position.x, dyingEnemy.transform.position.y + 3, dyingEnemy.transform.position.z);
        grassSeedSpawn = Instantiate(levelOneProgress.grassSeedPrefab, enemyPos, Quaternion.identity);
        grassSeedSpawn = Instantiate(levelOneProgress.grassSeedPrefab, new Vector3(enemyPos.x + 1, enemyPos.y, enemyPos.z - 1), Quaternion.identity);
        grassSeedSpawn = Instantiate(levelOneProgress.grassSeedPrefab, new Vector3(enemyPos.x - 1, enemyPos.y, enemyPos.z + 1), Quaternion.identity);

        keyMonsterDefeatCount++;

        duck1.GetComponent<Duck>().SetUpperBankOn();
        duck2.GetComponent<Duck>().SetUpperBankOn();

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

        duck1.GetComponent<Duck>().SetFarBankOn();
        duck2.GetComponent<Duck>().SetFarBankOn();

        keyMonsterDefeatCount++;
    }

    public void OnFourthMonsterDefeated()
    {
        foreach (GameObject go in fourthAreaTiles)
        {
            if (go.GetComponent<Cell>().terrainType == Cell.TerrainType.DIRT)
            {
                go.GetComponent<Cell>().enviroState = Cell.EnviroState.CLEAN;
            }
        }

        duck1.GetComponent<Duck>().SetHalfwayPointOn();
        duck2.GetComponent<Duck>().SetHalfwayPointOn();

        keyMonsterDefeatCount++;

        task4.CrossOutTask();
        SetFriendCompletion();
        fourthMonsterDeadDialouge.TriggerDialogue();
    }

    public void OnPumpShutOff()
    {
        water.GetComponent<Renderer>().material = cleanWaterMaterial;
        //river.GetComponent<Renderer>().material = cleanWaterMaterial;
        //lake.GetComponent<Renderer>().material = cleanWaterMaterial;
        foreach (GameObject go in firstAreaTiles)
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
        
        task5.CrossOutTask();
        SetWaterCompletion();
    }

    public void OnReadyToLeave()
    {
        //Set the leave trigger to exit the level to on
        runReadyToLeaveDialogue = true;
        allObjectivesMetDialogue.TriggerDialogue();
        leaveTriggerObj.SetActive(true);
    }

    public void OnBridgeEncountered()
    {
        if (!hasEncounteredBridge)
        {
            task7.gameObject.SetActive(true);
            hasEncounteredBridge = true;
        }
    }

    public void OnBridgeBuilt()
    {
        task7.CrossOutTask();
    }

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
}
