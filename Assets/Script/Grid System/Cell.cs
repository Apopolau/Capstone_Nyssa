using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] GameObject buildingTarget;
    enum TerrainType {GRASS, DIRT, WATER, POLLUTED};
    [SerializeField] TerrainType terrainType;

    GameObject placedObject;

    private void Awake()
    {
        terrainType = TerrainType.POLLUTED;
    }

    private void OnMouseOver()
    {
        
    }
}
