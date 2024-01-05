using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private EarthPlayer earthPlayer;
    [SerializeField] GameObjectRuntimeSet playerSet;
    [SerializeField] public GameObject buildingTarget;
    public enum TerrainType {GRASS, DIRT, WATER, POLLUTED};
    public TerrainType terrainType;

    Vector2 tileVector;
    Vector2 playerVector;

    public GameObject placedObject;

    WaitForSeconds waitTime = new WaitForSeconds(0.2f);

    public bool tileIsActivated;
    public bool tileHasBuild;

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

    private void Update()
    {
        UpdatePlant();
    }

    private void UpdatePlant()
    {
        if (earthPlayer.isPlantSelected && tileIsActivated)
        {
            earthPlayer.plantSelected.transform.position = buildingTarget.transform.position;
            //Handles indication whether it's a valid position or not
            if (tileHasBuild || terrainType == Cell.TerrainType.POLLUTED)
            {
                earthPlayer.plantSelected.GetComponentInChildren<SpriteRenderer>().color.Equals(Color.red);
            }
            else
            {
                earthPlayer.plantSelected.GetComponentInChildren<SpriteRenderer>().color.Equals(Color.blue);
            }
        }
    }

    IEnumerator CheckForPlayer()
    {
        while (true)
        {
            playerVector.x = earthPlayer.transform.position.x;
            playerVector.y = earthPlayer.transform.position.z;
            //Debug.Log("Player has selected a plant");
            if (Mathf.Abs((tileVector - playerVector).magnitude) < 5)
            {
                tileIsActivated = true;
                earthPlayer.selectedTile = this.gameObject;
            }
            else
            {
                tileIsActivated = false;
            }

            yield return waitTime;
        }
    }
}
