using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class EventManager : MonoBehaviour
{
    public Enemy dyingEnemy;

    protected void FlipTiles(List<GameObject> tiles)
    {
        foreach (GameObject tile in tiles)
        {
            tile.GetComponent<Cell>().terrainType = Cell.TerrainType.DIRT;
        }
    }
}
