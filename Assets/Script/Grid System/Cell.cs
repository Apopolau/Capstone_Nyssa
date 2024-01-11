using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private EarthPlayer earthPlayer;
    [SerializeField] GameObjectRuntimeSet playerSet;
    [SerializeField] public GameObject buildingTarget;
    public enum TerrainType {GRASS, DIRT, WATER, POLLUTED};
    public TerrainType terrainType;

    Color selectableColour = new Color(0.5f, 0.9666f, 1, 0.5f);
    Color unselectableColour = new Color(1, 0.5f, 0.5f, 0.5f);

    Vector2 tileVector;
    Vector2 playerVector;
    //Vector2 virtualMousePosition;

    public GameObject placedObject;

    WaitForSeconds waitTime = new WaitForSeconds(0.2f);

    public bool tileValid = true;
    public bool tileIsActivated = false;
    public bool tileHasBuild = false;

    //private VirtualMouseInput virtualMouseInput;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask tileMask;

    private void Awake()
    {
        //virtualMouseInput = GetComponent<VirtualMouseInput>();
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
        UpdateTileState();
        UpdatePlant();
    }

    private void LateUpdate()
    {
        //virtualMousePosition = virtualMouseInput.virtualMouse.position.value;
    }

    private void UpdateTileState()
    {
        if(terrainType == TerrainType.POLLUTED || tileHasBuild)
        {
            tileValid = false;
        }
        else
        {
            tileValid = true;
        }
    }

    private void UpdatePlant()
    {
        if (earthPlayer.isPlantSelected && tileIsActivated)
        {
            earthPlayer.plantSelected.transform.position = buildingTarget.transform.position;
            //Handles indication whether it's a valid position or not
            if (tileHasBuild || terrainType == Cell.TerrainType.POLLUTED)
            {
                earthPlayer.plantSelected.GetComponentInChildren<SpriteRenderer>().color = unselectableColour;
            }
            else
            {
                earthPlayer.plantSelected.GetComponentInChildren<SpriteRenderer>().color = selectableColour;
            }
        }
    }

    IEnumerator CheckForPlayer()
    {
        while (true)
        {
            //playerVector.x = earthPlayer.transform.position.x;
            //playerVector.y = earthPlayer.transform.position.z;
            //Debug.Log("Player has selected a plant");
            /*
            if (Mathf.Abs((tileVector - virtualMousePosition).magnitude) < 5)
            {
                tileIsActivated = true;
                earthPlayer.selectedTile = this.gameObject;
            }
            
            Ray cameraRay = mainCamera.ViewportPointToRay(virtualMousePosition);
            if (Physics.Raycast(cameraRay, out RaycastHit HitInfo, 50, tileMask))
            {

            }
            else
            {
                tileIsActivated = false;
            }
            */
            if(earthPlayer.selectedTile != this)
            {
                tileIsActivated = false;
            }
            else
            {
                tileIsActivated = true;
            }

            yield return waitTime;
        }
    }
}
