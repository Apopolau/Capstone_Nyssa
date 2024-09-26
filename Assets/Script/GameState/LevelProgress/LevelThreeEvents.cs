using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelThreeEvents : LevelEventManager
{

    /// <summary>
    /// THIS SCRIPT WAS COPY AND PASTED FROM LEVEL ONE EVENTS
    /// PLEASE READ IT CAREFULLY BEFORE DECIDING IT'S UP TO DATE
    /// </summary>
    [SerializeField] LevelThreeProgress levelThreeProgress;

    [SerializeField] Material cleanWaterMaterial;
    [SerializeField] GameObject river;

    [SerializeField] GameObject mainAreaGrid;
    [SerializeField] GameObject farmAreaGrid;
    [SerializeField] GameObject lowerLeftGrid;
    [SerializeField] GameObject buildingAreaGrid;

    List<GameObject> mainAreaTiles = new List<GameObject>();
    List<GameObject> farmAreaTiles = new List<GameObject>();
    List<GameObject> lowerLeftTiles = new List<GameObject>();
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

        foreach (Transform childTransform in farmAreaGrid.transform)
        {
            farmAreaTiles.Add(childTransform.gameObject);
        }

        foreach (Transform childTransform in lowerLeftGrid.transform)
        {
            lowerLeftTiles.Add(childTransform.gameObject);
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

    public void OnSecondMonsterDefeated()
    {

        foreach (GameObject go in lowerLeftTiles)
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

        foreach (GameObject go in farmAreaTiles)
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

    
    public void OnPesticideShutOff()
    {
        //river.GetComponent<Renderer>().material = cleanWaterMaterial;
        river.GetComponent<Renderer>().material = cleanWaterMaterial;
        /*
        foreach(GameObject go in mainAreaTiles)
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
        */
        levelThreeProgress.animalHasWater = true;
        //task2.CrossOutTask();
    }
    

    private void EvaluateFoodLevel()
    {
        task1.GetComponent<TextMeshProUGUI>().text = $"- Plant 6 trees ({levelThreeProgress.GetTreeCount()}/6)";
        task2.GetComponent<TextMeshProUGUI>().text = $"- Plant 8 grass ({levelThreeProgress.GetGrassCount()}/8)";
        task3.GetComponent<TextMeshProUGUI>().text = $"- Plant 4 cattails ({levelThreeProgress.GetCattailCount()}/4)";
        task4.GetComponent<TextMeshProUGUI>().text = $"- Plant 6 flowers ({levelThreeProgress.GetFlowerCount()}/6)";
        task5.GetComponent<TextMeshProUGUI>().text = $"- Plant 4 lilies ({levelThreeProgress.GetLilyCount()}/4)";
        if (levelThreeProgress.EvaluateTrees())
        {
            task1.CrossOutTask();
        }
        if (levelThreeProgress.EvaluateGrass())
        {
            task2.CrossOutTask();
        }
        if (levelThreeProgress.EvaluateCattails())
        {
            task3.CrossOutTask();
        }
        if (levelThreeProgress.EvaluateFlowers())
        {
            task4.CrossOutTask();
        }
        if (levelThreeProgress.EvaluateLilies())
        {
            task5.CrossOutTask();
        }
    }

    public void DebugTeleport()
    {

    }

    public override LevelProgress GetProgress()
    {
        return levelThreeProgress;
    }

    public override void PopulateInventory()
    {

    }
}
