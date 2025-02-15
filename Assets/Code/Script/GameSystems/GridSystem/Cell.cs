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

    WaitForSeconds waitTime = new WaitForSeconds(0.3f);

    public bool tileValid = true;
    public bool tileIsActivated = false;
    public bool tileHasBuild = false;
    public bool shouldBeGrass = false;

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
        StartCoroutine(UpdateNeighbours());
        tileVector.x = this.transform.position.x;
        tileVector.y = this.transform.position.z;

    }

    private void Update()
    {
        UpdateTileValid();
        //If the player is currently picking a place to plant their plant
        if ((earthPlayer.GetIsPlantSelected()) && (tileIsActivated) && (!earthPlayer.GetIsATileSelected()))
        {
            UpdatePlantPreview();
            CheckTileValidity();
        }
        //Or if they're trying to remove a plant
        if ((earthPlayer.GetIsRemovalStarted()) && (tileIsActivated) && (!earthPlayer.GetIsATileSelected()))
        {
            UpdatePlantRemoval();
            //CheckTileValidity();
        }
    }

    private void LateUpdate()
    {
        //virtualMousePosition = virtualMouseInput.virtualMouse.position.value;
    }

    //Updates whether or not this is a tile that can be planted on
    private void UpdateTileValid()
    {
        
        //Don't let them plant a tree on a tile next to a tree, or in the water
        if(earthPlayer.plantSelectedType == EarthPlayer.PlantSelectedType.TREE)
        {
            if(terrainType == TerrainType.WATER)
            {
                tileValid = false;
                return;
            }
            foreach(Cell c in neighbours)
            {
                if (c != null)
                {
                    if (c.tileHasBuild)
                    {
                        if (c.placedObject.GetComponent<Plant>().stats.plantName == "Tree")
                        {
                            tileValid = false;
                            break;
                        }
                        else if(enviroState == EnviroState.POLLUTED || tileHasBuild)
                        {
                            tileValid = false;
                        }
                        else
                        {
                            tileValid = true;
                        }
                    }
                }
                
            }
        }
        //Can't plant on polluted tiles or tiles with builds
        else if (enviroState == EnviroState.POLLUTED || tileHasBuild)
        {
            tileValid = false;
        }
        else
        {
            tileValid = true;
        }
    }

    //Handles flipping tiles based on various settings
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
                    if (tileHasBuild || shouldBeGrass)
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

    private IEnumerator UpdateNeighbours()
    {
        //I'm so sorry for this stupid nested statement
        while (true)
        {
            if (tileHasBuild)
            {
                if(placedObject.GetComponent<Plant>().stats.plantName == "Tree")
                {
                    foreach(Cell c in neighbours)
                    {
                        if(c != null)
                        {
                            c.shouldBeGrass = true;
                            if(placedObject.GetComponent<Plant>().currentPlantStage == PlantStats.PlantStage.JUVENILE ||
                                placedObject.GetComponent<Plant>().currentPlantStage == PlantStats.PlantStage.MATURE)
                            {
                                ConvertToGrass(c.neighbours);
                            }
                            
                        }
                        
                    }
                }
                else
                {
                    ConvertToGrass(neighbours);
                }
                
            }
            yield return new WaitForSeconds(0.2f);

        }
    }


    /// <summary>
    /// This runs while the player is still selecting a tile to plant on
    /// It updates the appearance of the plant preview
    /// depending on the plant type and tile they're currently hovering over
    /// </summary>
    public void UpdatePlantPreview()
    {
        //Move the plant position to the center of the currently highlighted tile
        if(earthPlayer.GetSelectedPlant() != null && earthPlayer.GetTileOutline() != null)
        {
            earthPlayer.GetSelectedPlant().transform.position = buildingTarget.transform.position;
            earthPlayer.GetTileOutline().transform.position = buildingTarget.transform.position;
        }
        
        //Update the plant type if the player pans over a different kind of tile
        /////IF TILE IS EARTH
        if ((this.terrainType == TerrainType.GRASS || this.terrainType == TerrainType.DIRT) && earthPlayer.GetTileSelectedType() == EarthPlayer.TileSelectedType.WATER)
        {
            //If it's not a tree, we need to destroy the previous preview
            if (earthPlayer.plantSelectedType != EarthPlayer.PlantSelectedType.TREE)
            {
                Destroy(earthPlayer.GetSelectedPlant());
            }

            //Make a new preview
            if (earthPlayer.plantSelectedType == EarthPlayer.PlantSelectedType.FLOWER)
            {
                earthPlayer.SetSelectedPlant(Instantiate(earthPlayer.landFlowerPreviewPrefab, earthPlayer.plantParent.transform));
            }
            else if (earthPlayer.plantSelectedType == EarthPlayer.PlantSelectedType.GRASS)
            {
                earthPlayer.SetSelectedPlant(Instantiate(earthPlayer.landGrassPreviewPrefab, earthPlayer.plantParent.transform));
            }
            earthPlayer.SetTileSelectedType(EarthPlayer.TileSelectedType.LAND);
        }
        //////IF TILE IS WATER
        else if ((this.terrainType == TerrainType.WATER) && earthPlayer.GetTileSelectedType() == EarthPlayer.TileSelectedType.LAND)
        {
            //If it's not a tree, we need to destroy the previous preview
            if (earthPlayer.plantSelectedType != EarthPlayer.PlantSelectedType.TREE)
            {
                Destroy(earthPlayer.GetSelectedPlant());
            }

            //Make a new preview
            if (earthPlayer.plantSelectedType == EarthPlayer.PlantSelectedType.FLOWER)
            {
                earthPlayer.SetSelectedPlant(Instantiate(earthPlayer.waterFlowerPreviewPrefab, earthPlayer.plantParent.transform));
            }
            else if (earthPlayer.plantSelectedType == EarthPlayer.PlantSelectedType.GRASS)
            {
                earthPlayer.SetSelectedPlant(Instantiate(earthPlayer.waterGrassPreviewPrefab, earthPlayer.plantParent.transform));
            }
            earthPlayer.SetTileSelectedType(EarthPlayer.TileSelectedType.WATER);
        }
    }

    private void CheckTileValidity()
    {
        //Handles indication whether it's a valid position or not
        if (earthPlayer.plantSelectedType == EarthPlayer.PlantSelectedType.TREE)
        {
            if (!tileValid || enviroState == EnviroState.POLLUTED || terrainType == TerrainType.WATER)
            {
                earthPlayer.GetSelectedPlant().GetComponentInChildren<SpriteRenderer>().color = unselectableColour;
                earthPlayer.GetTileOutline().GetComponentInChildren<SpriteRenderer>().color = Color.red;
            }
            else
            {
                earthPlayer.GetSelectedPlant().GetComponentInChildren<SpriteRenderer>().color = selectableColour;
                earthPlayer.GetTileOutline().GetComponentInChildren<SpriteRenderer>().color = Color.green;
            }
        }
        else
        {
            if (!tileValid || enviroState == EnviroState.POLLUTED)
            {
                earthPlayer.GetSelectedPlant().GetComponentInChildren<SpriteRenderer>().color = unselectableColour;
                earthPlayer.GetTileOutline().GetComponentInChildren<SpriteRenderer>().color = Color.red;
            }
            else if (earthPlayer.plantSelectedType == EarthPlayer.PlantSelectedType.GRASS || earthPlayer.plantSelectedType == EarthPlayer.PlantSelectedType.FLOWER)
            {
                earthPlayer.GetSelectedPlant().GetComponentInChildren<SpriteRenderer>().color = selectableColour;
                earthPlayer.GetTileOutline().GetComponentInChildren<SpriteRenderer>().color = Color.green;
            }
        }
    }

    //Modifies the indicators for plant removal while the player selects a tile to remove from
    private void UpdatePlantRemoval()
    {
        if (earthPlayer.GetIsRemovalStarted() && tileIsActivated && earthPlayer.earthControls.controls.RemovingPlant.enabled)
        {
            earthPlayer.GetTileOutline().transform.position = buildingTarget.transform.position;
            if (tileHasBuild)
            {
                earthPlayer.GetTileOutline().GetComponentInChildren<SpriteRenderer>().color = Color.green;
            }
            else
            {
                earthPlayer.GetTileOutline().GetComponentInChildren<SpriteRenderer>().color = Color.red;
            }
        }
    }

    //ensures just one tile will be set to activated
    IEnumerator CheckForPlayer()
    {
        while (true)
        {
            if (earthPlayer.GetSelectedTile() == this.gameObject)
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

    private void ConvertToGrass(Cell[] cellList)
    {
        foreach (Cell cc in cellList)
        {
            if (cc != null)
            {
                cc.shouldBeGrass = true;
            }

        }
    }

    //Identifies the tiles next to this one and stores them in a grid
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

    //Helper function to grab anything that's been placed on this tile
    public GameObject GetPlacedObject()
    {
        return placedObject;
    }
}
