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

    [SerializeField] private Texture fullGrassTile;
    /*
    [SerializeField] private Texture grassAllSidesDirtTile;
    [SerializeField] private Texture grassStraightEdgeTile;
    [SerializeField] private Texture grassDirtCornerTile;
    [SerializeField] private Texture grassTetrisPieceTile;
    [SerializeField] private Texture grass1CornerDirtTile;
    [SerializeField] private Texture grass2CornerOppositeDirtTile;
    [SerializeField] private Texture grass2CornerAdjacentDirtTile;
    [SerializeField] private Texture grass3CornersDirtTile;
    [SerializeField] private Texture grass4CornersDirtTile;
    [SerializeField] private Texture grassLineTile;
    */

    [SerializeField] private Texture dirtTile;
    [SerializeField] private Texture pollutedTile;
    [SerializeField] private Material cleanWaterTile;
    [SerializeField] private Material pollutedWaterTile;

    public enum TerrainType { GRASS, DIRT, WATER };
    public TerrainType terrainType;

    public enum EnviroState { CLEAN, POLLUTED };
    public EnviroState enviroState;

    Color selectableColour = new Color(0.5f, 0.9666f, 1, 0.5f);
    Color unselectableColour = new Color(1, 0.5f, 0.5f, 0.5f);

    Vector2 tileVector;
    Vector2 playerVector;

    public GameObject tileGroup;
    //0: top left, 1: top, 2: top right, 3: left, 4: right, 5: bottom left, 6: bottom 7: bottom right
    [SerializeField] Cell[] neighbours = new Cell[8];
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
        tileGroup = gameObject.transform.parent.gameObject;
        FindNeighbours();
    }

    private void Start()
    {
        foreach (GameObject player in playerSet.Items)
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
        UpdateTileValid();
        UpdatePlant();
        if (earthPlayer.isRemovalStarted)
        {
            UpdatePlantRemoval();
        }
    }

    private void LateUpdate()
    {
        //virtualMousePosition = virtualMouseInput.virtualMouse.position.value;
    }

    private void UpdateTileValid()
    {
        if (enviroState == EnviroState.POLLUTED || tileHasBuild ||
            (earthPlayer.plantSelectedType == EarthPlayer.PlantSelectedType.TREE && terrainType == TerrainType.WATER))
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
            if (terrainType == TerrainType.WATER && enviroState == EnviroState.CLEAN)
            {
                GetComponentInChildren<MeshRenderer>().material = cleanWaterTile;
            }
            else if (terrainType == TerrainType.WATER && enviroState == EnviroState.POLLUTED)
            {
                GetComponentInChildren<MeshRenderer>().material = pollutedWaterTile;
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
                        GetComponentInChildren<MeshRenderer>().material.mainTexture = fullGrassTile;
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
        //If the player is currently picking a place to plant their plant
        if (earthPlayer.isPlantSelected && tileIsActivated)
        {
            //Move the plant position to the center of the currently highlighted tile
            earthPlayer.plantSelected.transform.position = buildingTarget.transform.position;
            earthPlayer.tileOutline.transform.position = buildingTarget.transform.position;

            //Update the plant type if the player pans over a different kind of tile
            /////IF TILE IS EARTH
            if ((this.terrainType == TerrainType.GRASS || this.terrainType == TerrainType.DIRT) && earthPlayer.currentTileSelectedType == EarthPlayer.TileSelectedType.WATER)
            {

                if (earthPlayer.plantSelectedType != EarthPlayer.PlantSelectedType.TREE)
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
            }
            //////IF TILE IS WATER
            else if ((this.terrainType == TerrainType.WATER) && earthPlayer.currentTileSelectedType == EarthPlayer.TileSelectedType.LAND)
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
                    earthPlayer.tileOutline.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                }
                else
                {
                    earthPlayer.plantSelected.GetComponentInChildren<SpriteRenderer>().color = selectableColour;
                    earthPlayer.tileOutline.GetComponentInChildren<SpriteRenderer>().color = Color.green;
                }
            }
            else
            {
                if (tileHasBuild || enviroState == EnviroState.POLLUTED)
                {
                    earthPlayer.plantSelected.GetComponentInChildren<SpriteRenderer>().color = unselectableColour;
                    earthPlayer.tileOutline.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                }
                else if (earthPlayer.plantSelectedType == EarthPlayer.PlantSelectedType.GRASS)
                {
                    earthPlayer.plantSelected.GetComponentInChildren<SpriteRenderer>().color = selectableColour;
                    earthPlayer.tileOutline.GetComponentInChildren<SpriteRenderer>().color = Color.green;
                }
            }

        }
    }

    private void UpdatePlantRemoval()
    {
        if (tileIsActivated)
        {
            earthPlayer.tileOutline.transform.position = buildingTarget.transform.position;
            if (tileHasBuild)
            {
                earthPlayer.tileOutline.GetComponentInChildren<SpriteRenderer>().color = Color.green;
            }
            else
            {
                earthPlayer.tileOutline.GetComponentInChildren<SpriteRenderer>().color = Color.red;
            }
        }
    }

    IEnumerator CheckForPlayer()
    {
        while (true)
        {
            if (earthPlayer.selectedTile == this.gameObject)
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

    private void FindNeighbours()
    {
        float margin = 0.5f;
        Cell[] cells = tileGroup.GetComponentsInChildren<Cell>();
        foreach (Cell cell in cells)
        {
            if ((cell.transform.position.x <= this.transform.position.x - 10 + margin)
                && (cell.transform.position.x >= this.transform.position.x - 10 - margin))
            {
                if ((cell.transform.position.z <= this.transform.position.z + margin) &&
                    (cell.transform.position.z >= this.transform.position.z - margin))
                {
                    neighbours[3] = cell;
                }
                else if ((cell.transform.position.z <= this.transform.position.z - 10 + margin) &&
                    (cell.transform.position.z >= this.transform.position.z - 10 - margin))
                {
                    neighbours[5] = cell;
                }
                else if ((cell.transform.position.z <= this.transform.position.z + 10 + margin) &&
                    (cell.transform.position.z >= this.transform.position.z + 10 - margin))
                {
                    neighbours[0] = cell;
                }
            }
            else if ((cell.transform.position.x <= this.transform.position.x + margin)
                && (cell.transform.position.x >= this.transform.position.x - margin))
            {
                if ((cell.transform.position.z <= this.transform.position.z - 10 + margin) &&
                    (cell.transform.position.z >= this.transform.position.z - 10 - margin))
                {
                    neighbours[6] = cell;
                }
                else if ((cell.transform.position.z <= this.transform.position.z + 10 + margin) &&
                    (cell.transform.position.z >= this.transform.position.z + 10 - margin))
                {
                    neighbours[1] = cell;
                }
            }
            else if ((cell.transform.position.x <= this.transform.position.x + 10 + margin)
                && (cell.transform.position.x >= this.transform.position.x + 10 - margin))
            {
                if ((cell.transform.position.z <= this.transform.position.z + margin) &&
                    (cell.transform.position.z >= this.transform.position.z - margin))
                {
                    neighbours[4] = cell;
                }
                else if ((cell.transform.position.z <= this.transform.position.z - 10 + margin) &&
                    (cell.transform.position.z >= this.transform.position.z - 10 - margin))
                {
                    neighbours[7] = cell;
                }
                else if ((cell.transform.position.z <= this.transform.position.z + 10 + margin) &&
                    (cell.transform.position.z >= this.transform.position.z + 10 - margin))
                {
                    neighbours[2] = cell;
                }
            }
        }
    }

    public GameObject GetPlacedObject()
    {
        return placedObject;
    }
}
