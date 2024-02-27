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
        levelOneProgress.shelter = true;
        task3.CrossOutTask();
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
        task2.CrossOutTask();
    }

    private void EvaluateFoodLevel()
    {
        if (levelOneProgress.EvaluateFood())
        {
            task3.CrossOutTask();
        }
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
