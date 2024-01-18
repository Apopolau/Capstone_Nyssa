using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneEvents : EventManager
{
    LevelOneProgress levelOneProgress;

    [SerializeField] Texture cleanWaterMaterial;
    GameObject river;

    GameObject firstAreaGrid;
    GameObject secondAreaGrid;
    GameObject thirdAreaGrid;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnFirstMonsterDefeated()
    {
        List<GameObject> firstAreaTiles = new List<GameObject>();
        foreach (Transform childTransform in firstAreaGrid.transform)
        {
            firstAreaTiles.Add(childTransform.gameObject);
        }
        foreach (GameObject go in firstAreaTiles)
        {
            go.GetComponent<Cell>().terrainType = Cell.TerrainType.DIRT;
        }
    }

    private void OnPumpShutOff()
    {
        //Change texture from gross to nice?
        river.GetComponent<Material>().SetTexture("Clean Water", cleanWaterMaterial);
        levelOneProgress.GetComponent<LevelProgress>().animalHasEnoughWater = true;
    }


}
