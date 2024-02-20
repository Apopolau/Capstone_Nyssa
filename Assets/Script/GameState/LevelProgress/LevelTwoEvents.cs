using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTwoEvents : EventManager
{

    /// <summary>
    /// THIS SCRIPT WAS COPY AND PASTED FROM LEVEL ONE EVENTS
    /// PLEASE READ IT CAREFULLY BEFORE DECIDING IT'S UP TO DATE
    /// </summary>
    [SerializeField] LevelTwoProgress levelTwoProgress;

    [SerializeField] Material cleanWaterMaterial;
    [SerializeField] GameObject lake;

    [SerializeField] GameObject firstAreaGrid;
    [SerializeField] GameObject secondAreaGrid;
    [SerializeField] GameObject thirdAreaGrid;

    List<GameObject> firstAreaTiles = new List<GameObject>();
    List<GameObject> secondAreaTiles = new List<GameObject>();
    List<GameObject> thirdAreaTiles = new List<GameObject>();

    [SerializeField] TaskListManager task1;
    [SerializeField] TaskListManager task2;
    [SerializeField] TaskListManager task3;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform childTransform in firstAreaGrid.transform)
        {
            firstAreaTiles.Add(childTransform.gameObject);
            /*
            Cell[] cells = GetComponentsInChildren<Cell>();
            foreach(Cell cell in cells)
            {
                cell.
            }
            */
            
        }
        
        foreach (Transform childTransform in secondAreaGrid.transform)
        {
            secondAreaTiles.Add(childTransform.gameObject);
        }
        
        foreach (Transform childTransform in thirdAreaGrid.transform)
        {
            thirdAreaTiles.Add(childTransform.gameObject);
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
        levelTwoProgress.shelter = true;
        task3.CrossOutTask();
    }

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

    private void EvaluateFoodLevel()
    {
        if (levelTwoProgress.EvaluateFood())
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
