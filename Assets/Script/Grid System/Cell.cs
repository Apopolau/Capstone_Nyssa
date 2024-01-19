using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private EarthPlayer earthPlayer;
    [SerializeField] private CelestialPlayer celestialPlayer;
    [SerializeField] GameObjectRuntimeSet playerSet;
    [SerializeField] public GameObject buildingTarget;

    [SerializeField] private Texture grassTile;
    [SerializeField] private Texture dirtTile;
    [SerializeField] private Texture pollutedTile;
    [SerializeField] private Material waterTile;

    public enum TerrainType {GRASS, DIRT, WATER};
    public TerrainType terrainType;

    public enum EnviroState { CLEAN, POLLUTED};
    public EnviroState enviroState;

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
    //[SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask tileMask;

    private void Awake()
    {
        //virtualMouseInput = GetComponent<VirtualMouseInput>();
    }

    private void Start()
    {
        foreach(GameObject player in playerSet.Items)
        {
            if (player.tag == "Player1")
            {
                earthPlayer = player.GetComponent<EarthPlayer>();
            }
            else if (player.tag == "Player2")
            {
                celestialPlayer = player.GetComponent<CelestialPlayer>();
            }
        }
        //earthPlayer = playerSet.GetItemIndex(0).GetComponent<EarthPlayer>();
        StartCoroutine(CheckForPlayer());
        StartCoroutine(UpdateTileAppearance());
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
        if(enviroState == EnviroState.POLLUTED || tileHasBuild)
        {
            tileValid = false;
        }
        else
        {
            tileValid = true;
        }
    }

    private IEnumerator UpdateTileAppearance()
    {
        while (true)
        {
            if (terrainType == TerrainType.WATER)
            {
                GetComponentInChildren<MeshRenderer>().material = waterTile;
            }
            else
            {
                if (enviroState == EnviroState.POLLUTED)
                {
                    GetComponentInChildren<MeshRenderer>().material.mainTexture = pollutedTile;
                }
                else
                {
                    if (tileHasBuild)
                    {
                        GetComponentInChildren<MeshRenderer>().material.mainTexture = grassTile;
                    }
                    else
                    {
                        GetComponentInChildren<MeshRenderer>().material.mainTexture = dirtTile;
                    }
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void UpdatePlant()
    {
        //Debug.Log(earthPlayer);
        //If the player is currently picking a place to plant their plant
        if (earthPlayer.isPlantSelected && tileIsActivated)
        {
            //Move the plant position to the center of the currently highlighted tile
            earthPlayer.plantSelected.transform.position = buildingTarget.transform.position;

            //Update the plant type if the player pans over a different kind of tile
            if((this.terrainType == TerrainType.GRASS || this.terrainType == TerrainType.DIRT) && earthPlayer.currentTileSelectedType == EarthPlayer.TileSelectedType.WATER)
            {
                
                if(earthPlayer.plantSelectedType != EarthPlayer.PlantSelectedType.TREE)
                {
                    Destroy(earthPlayer.plantSelected);
                }
                
                if (earthPlayer.plantSelectedType == EarthPlayer.PlantSelectedType.FLOWER)
                {
                    earthPlayer.plantSelected = Instantiate(earthPlayer.landFlowerPreviewPrefab, earthPlayer.plantParent.transform);
                }
                else if (earthPlayer.plantSelectedType == EarthPlayer.PlantSelectedType.GRASS)
                {
                    earthPlayer.plantSelected = Instantiate(earthPlayer.landGrassPreviewPrefab, earthPlayer.plantParent.transform);
                }
                earthPlayer.currentTileSelectedType = EarthPlayer.TileSelectedType.LAND;
                /*
                else if (earthPlayer.plantSelectedType == Earth)
                {
                    landFlowerPreviewPrefab
                landGrassPreviewPrefab
                waterFlowerPreviewPrefab
                waterGrassPreviewPrefab
                }
                */
            }
            else if((this.terrainType == TerrainType.WATER) && earthPlayer.currentTileSelectedType == EarthPlayer.TileSelectedType.LAND)
            {
                
                if (earthPlayer.plantSelectedType != EarthPlayer.PlantSelectedType.TREE)
                {
                    Destroy(earthPlayer.plantSelected);
                }

                if (earthPlayer.plantSelectedType == EarthPlayer.PlantSelectedType.FLOWER)
                {
                    earthPlayer.plantSelected = Instantiate(earthPlayer.waterFlowerPreviewPrefab, earthPlayer.plantParent.transform);
                }
                else if (earthPlayer.plantSelectedType == EarthPlayer.PlantSelectedType.GRASS)
                {
                    earthPlayer.plantSelected = Instantiate(earthPlayer.waterGrassPreviewPrefab, earthPlayer.plantParent.transform);
                }
                earthPlayer.currentTileSelectedType = EarthPlayer.TileSelectedType.WATER;
            }

            //Handles indication whether it's a valid position or not
            if (earthPlayer.plantSelectedType == EarthPlayer.PlantSelectedType.TREE)
            {
                if (tileHasBuild || enviroState == EnviroState.POLLUTED || terrainType == TerrainType.WATER)
                {
                    earthPlayer.plantSelected.GetComponentInChildren<SpriteRenderer>().color = unselectableColour;
                }
                else
                {
                    earthPlayer.plantSelected.GetComponentInChildren<SpriteRenderer>().color = selectableColour;
                }
            }
            else
            {
                if (tileHasBuild || enviroState == EnviroState.POLLUTED)
                {
                    earthPlayer.plantSelected.GetComponentInChildren<SpriteRenderer>().color = unselectableColour;
                }
                else if(earthPlayer.plantSelectedType == EarthPlayer.PlantSelectedType.GRASS)
                {
                    earthPlayer.plantSelected.GetComponentInChildren<SpriteRenderer>().color = selectableColour;
                }
            }
            
        }
    }

    IEnumerator CheckForPlayer()
    {
        while (true)
        {
            if(earthPlayer.selectedTile == this.gameObject)
            {
                tileIsActivated = true;
            }
            else
            {
                tileIsActivated = false;
            }

            yield return waitTime;
        }
    }
}
