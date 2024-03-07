using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneEvents : EventManager
{
    [SerializeField] LevelOneProgress levelOneProgress;

    [SerializeField] Material cleanWaterMaterial;
    //[SerializeField] GameObject river;
    //[SerializeField] GameObject lake;
    [SerializeField] GameObject water;

    [SerializeField] GameObject firstAreaGrid;
    [SerializeField] GameObject secondAreaGrid;
    [SerializeField] GameObject thirdAreaGrid;
    [SerializeField] GameObject fourthAreaGrid;

    List<GameObject> firstAreaTiles = new List<GameObject>();
    List<GameObject> secondAreaTiles = new List<GameObject>();
    List<GameObject> thirdAreaTiles = new List<GameObject>();
    List<GameObject> fourthAreaTiles = new List<GameObject>();

    [SerializeField] TaskListManager task1;
    [SerializeField] TaskListManager task2;
    [SerializeField] TaskListManager task3;
    [SerializeField] TaskListManager task4;
    [SerializeField] TaskListManager task5;
    [SerializeField] TaskListManager task6;

    private GameObject grassSeedSpawn;
    private GameObject treeSeedSpawn;

    public DialogueTrigger firstMonsterDeadDialouge;
    public DialogueTrigger secondMonsterDeadDialouge;
    public DialogueTrigger thirdMonsterDeadDialouge;
    public DialogueTrigger fourthMonsterDeadDialouge;
    public DialogueTrigger allMonstersDefeatedDialogue;
    public DialogueTrigger allObjectivesMetDialogue;

    int keyMonsterDefeatCount;

    private bool runDefeatDialogue = false;
    private bool runReadyToLeaveDialogue = false;


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

    }

    // Update is called once per frame
    void Update()
    {
        EvaluateFoodLevel();
        if (!runDefeatDialogue)
        {
            EvaluateMonsterDefeats();
        }
        
        if (levelOneProgress.EvaluateLevelProgress() && !runReadyToLeaveDialogue)
        {
            OnReadyToLeave();
        }
    }

    private void EvaluateFoodLevel()
    {
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
    }

    private void EvaluateMonsterDefeats()
    {
        if (keyMonsterDefeatCount == 4)
        {
            task6.CrossOutTask();
            levelOneProgress.animalIsSafe = true;
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
        treeSeedSpawn = Instantiate(levelOneProgress.treeSeedPrefab, dyingEnemy.transform.position, Quaternion.identity);
        treeSeedSpawn = Instantiate(levelOneProgress.treeSeedPrefab, 
            new Vector3(dyingEnemy.transform.position.x + 1, dyingEnemy.transform.position.y, dyingEnemy.transform.position.z - 1), Quaternion.identity);
        treeSeedSpawn = Instantiate(levelOneProgress.treeSeedPrefab,
            new Vector3(dyingEnemy.transform.position.x - 1, dyingEnemy.transform.position.y, dyingEnemy.transform.position.z + 1), Quaternion.identity);
        levelOneProgress.shelter = true;

        //We want to activate the objective menu here probably, or once the trigger dialogue is done.
        ////////////////////////////////////////////this is where we are going to drop the celestial cold orb!!!/////////////////////////////////////

        firstMonsterDeadDialouge.TriggerDialogue();
        keyMonsterDefeatCount++;
        
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
        grassSeedSpawn = Instantiate(levelOneProgress.grassSeedPrefab, dyingEnemy.transform.position, Quaternion.identity);
        grassSeedSpawn = Instantiate(levelOneProgress.grassSeedPrefab,
            new Vector3(dyingEnemy.transform.position.x + 1, dyingEnemy.transform.position.y, dyingEnemy.transform.position.z - 1), Quaternion.identity);
        grassSeedSpawn = Instantiate(levelOneProgress.grassSeedPrefab,
            new Vector3(dyingEnemy.transform.position.x - 1, dyingEnemy.transform.position.y, dyingEnemy.transform.position.z + 1), Quaternion.identity);

        keyMonsterDefeatCount++;

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

        keyMonsterDefeatCount++;

        levelOneProgress.animalHasFriend = true;
        task4.CrossOutTask();
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
        levelOneProgress.cleanWater = true;
        task5.CrossOutTask();
    }

    public void OnReadyToLeave()
    {
        //Set the leave trigger to exit the level to on
        runReadyToLeaveDialogue = true;
        allObjectivesMetDialogue.TriggerDialogue();
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
