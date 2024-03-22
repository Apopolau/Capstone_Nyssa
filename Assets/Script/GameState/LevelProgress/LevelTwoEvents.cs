using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelTwoEvents : EventManager
{

    /// <summary>
    /// THIS SCRIPT WAS COPY AND PASTED FROM LEVEL ONE EVENTS
    /// PLEASE READ IT CAREFULLY BEFORE DECIDING IT'S UP TO DATE
    /// </summary>
    [SerializeField] LevelTwoProgress levelTwoProgress;

    [SerializeField] Material cleanWaterMaterial;
    [SerializeField] GameObject lake;

    [SerializeField] GameObject mainAreaGrid;
    [SerializeField] GameObject hedgehogAreaGrid;
    [SerializeField] GameObject loggingAreaGrid;
    [SerializeField] GameObject buildingAreaGrid;

    List<GameObject> mainAreaTiles = new List<GameObject>();
    List<GameObject> hedgehogAreaTiles = new List<GameObject>();
    List<GameObject> loggingAreaTiles = new List<GameObject>();
    List<GameObject> buildingAreaTiles = new List<GameObject>();

    [SerializeField] TaskListManager task1;
    [SerializeField] TaskListManager task2;
    [SerializeField] TaskListManager task3;
    [SerializeField] TaskListManager task4;
    [SerializeField] TaskListManager task5;
    [SerializeField] TaskListManager task6;
    [SerializeField] TaskListManager task7;
    [SerializeField] TaskListManager task8;

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

    }

    // Update is called once per frame
    void Update()
    {
        EvaluateFoodLevel();
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

    private void EvaluateFoodLevel()
    {
        task1.GetComponent<TextMeshProUGUI>().text = $"- Plant 6 trees ({levelTwoProgress.GetTreeCount()}/6)";
        task2.GetComponent<TextMeshProUGUI>().text = $"- Plant 8 grass ({levelTwoProgress.GetGrassCount()}/8)";
        task3.GetComponent<TextMeshProUGUI>().text = $"- Plant 4 cattails ({levelTwoProgress.GetCattailCount()}/4)";
        task4.GetComponent<TextMeshProUGUI>().text = $"- Plant 6 flowers ({levelTwoProgress.GetFlowerCount()}/6)";
        task5.GetComponent<TextMeshProUGUI>().text = $"- Plant 4 lilies ({levelTwoProgress.GetLilyCount()}/4)";
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
    }

    /*
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
    */

    public void DebugTeleport()
    {

    }
}
