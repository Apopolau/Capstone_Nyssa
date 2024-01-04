using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private EarthPlayer earthPlayer;
    [SerializeField] GameObjectRuntimeSet playerSet;
    [SerializeField] GameObject buildingTarget;
    enum TerrainType {GRASS, DIRT, WATER, POLLUTED};
    [SerializeField] TerrainType terrainType;

    Vector2 tileVector;
    Vector2 playerVector;

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
        tileVector.x = this.transform.position.x;
        tileVector.y = this.transform.position.z;
    }

    IEnumerator CheckForPlayer()
    {
        while (true)
        {
            if (earthPlayer.isPlantSelected)
            {
                playerVector.x = earthPlayer.transform.position.x;
                playerVector.y = earthPlayer.transform.position.z;
                //Debug.Log("Player has selected a plant");
                if (Mathf.Abs((tileVector - playerVector).magnitude) < 2)
                {
                    Debug.Log("Player is near tile with a plant");
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
                //Debug.Log("Player has not selected a plant");
                tileIsActivated = false;
            }
            yield return waitTime;
        }
    }
}
