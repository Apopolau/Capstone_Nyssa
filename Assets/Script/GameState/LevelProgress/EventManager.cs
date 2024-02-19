using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class EventManager : MonoBehaviour
{
    //private bool isTaskCompleted = false;
    //private TextMeshProUGUI textComponent;

    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void FlipTiles(List<GameObject> tiles)
    {
        foreach (GameObject tile in tiles)
        {
            tile.GetComponent<Cell>().terrainType = Cell.TerrainType.DIRT;
        }
    }
}
