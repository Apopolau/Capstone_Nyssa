using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private EarthPlayer earthPlayer;
    [SerializeField] GameObjectRuntimeSet playerSet;
    [SerializeField] GameObject buildingTarget;
    enum TerrainType {GRASS, DIRT, WATER, POLLUTED};
    [SerializeField] TerrainType terrainType;

    GameObject placedObject;

    WaitForSeconds waitTime = new WaitForSeconds(0.2f);

    bool tileIsActivated;
    bool tileHasBuild;

    private void Awake()
    {
        terrainType = TerrainType.POLLUTED;
    }

    private void Start()
    {
        earthPlayer = playerSet.GetItemIndex(0).GetComponent<EarthPlayer>();
        StartCoroutine(CheckForPlayer());
    }

    IEnumerator CheckForPlayer()
    {
        if (earthPlayer.isPlantSelected)
        {
            if(Mathf.Abs((this.transform.position - earthPlayer.transform.position).magnitude) < 1)
            {
                tileIsActivated = true;
                earthPlayer.plantSelected.transform.position = buildingTarget.transform.position;
            }
            else
            {
                tileIsActivated = false;
            }
        }
        else
        {
            tileIsActivated = false;
        }
        yield return waitTime;
    }
}
