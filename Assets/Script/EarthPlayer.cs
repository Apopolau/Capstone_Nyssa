using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.AI;

public class EarthPlayer : MonoBehaviour
{
    //[SerializeField] Button treeButton;
    [SerializeField] public GameObject plantParent;

    public bool isPlantSelected = false;
    public GameObject plantSelected;
    public List<GameObject> plantsPlanted;
    public GameObject tempPlantPlanted;
    public GameObject plantPreview;

    private NavMeshAgent earthAgent;

    [Header("Plant Objects")]
    [SerializeField] private GameObject treePrefab;
    [SerializeField] private GameObject landGrassPrefab;
    [SerializeField] private GameObject waterGrassPrefab;
    [SerializeField] private GameObject landFlowerPrefab;
    [SerializeField] private GameObject waterFlowerPrefab;

    [Header("Preview Objects")]
    [SerializeField] public GameObject treePreviewPrefab;
    [SerializeField] public GameObject landGrassPreviewPrefab;
    [SerializeField] public GameObject waterGrassPreviewPrefab;
    [SerializeField] public GameObject landFlowerPreviewPrefab;
    [SerializeField] public GameObject waterFlowerPreviewPrefab;

    public enum PlantSelectedType {NONE, TREE, FLOWER, GRASS }
    public PlantSelectedType plantSelectedType;
    public GameObject selectedTile;
    public enum TileSelectedType { LAND, WATER};
    public TileSelectedType currentTileSelectedType;

    private WaitForSeconds plantTime;

    [SerializeField] TextMeshProUGUI displayText;

    private PlayerInput playerInput;

    [SerializeField] private VirtualMouseInput virtualMouseInput;
    [SerializeField] public Camera mainCamera;
    [SerializeField] private LayerMask tileMask;
    Vector2 virtualMousePosition;
    public bool enrouteToPlant = false;

    public Inventory inventory; // hold a reference to the Inventory script
    private void Awake()
    {
        earthAgent = GetComponent<NavMeshAgent>();
        earthAgent.enabled = false;
        virtualMouseInput.gameObject.GetComponentInChildren<Image>().enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        plantTime = new WaitForSeconds(2);
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        ActivateTile();
        if (enrouteToPlant && Mathf.Abs((this.transform.position - selectedTile.transform.position).magnitude) < 10.5f)
        {
            enrouteToPlant = false;
            earthAgent.ResetPath();
            earthAgent.enabled = false;
            PlantPlantingHandler();
        }
        
    }

    private void LateUpdate()
    {
        virtualMousePosition = virtualMouseInput.virtualMouse.position.value;
    }

    public void OnTreeSelected()
    {
        //We're going to want to check if they even have the seed for the plant they selected before we do anything else
        //Cell activeTileCell = selectedTile.GetComponent<Cell>();
        if (isPlantSelected)
        {
            isPlantSelected = false;
            Destroy(plantSelected);
        }
        if (!isPlantSelected)
        {
            //We select a type of plant from the input and make a transparent version of it with no stats
            
            plantSelectedType = PlantSelectedType.TREE;
            plantSelected = Instantiate(treePreviewPrefab, plantParent.transform);
        }
        OnPlantSelectedWrapUp();
    }

    public void OnGrassSelected()
    {
        if (isPlantSelected)
        {
            isPlantSelected = false;
            Destroy(plantSelected);
        }
        if (!isPlantSelected)
        {
            //We select a type of plant from the input and make a transparent version of it with no stats
            plantSelectedType = PlantSelectedType.GRASS;
            plantSelected = Instantiate(landGrassPreviewPrefab, plantParent.transform);
        }
        OnPlantSelectedWrapUp();
    }

    public void OnFlowerSelected()
    {
        if (isPlantSelected)
        {
            isPlantSelected = false;
            Destroy(plantSelected);
        }
        if (!isPlantSelected)
        {
            //We select a type of plant from the input and make a transparent version of it with no stats
            plantSelectedType = PlantSelectedType.FLOWER;
            plantSelected = Instantiate(landFlowerPreviewPrefab, plantParent.transform);
        }
        OnPlantSelectedWrapUp();
    }

    private void OnPlantSelectedWrapUp()
    {
        isPlantSelected = true;
        plantSelected.transform.position = this.transform.position;
        playerInput.SwitchCurrentActionMap("PlantIsSelected");
        virtualMouseInput.cursorTransform.position = new Vector2(Screen.width /2, Screen.height / 2);
        virtualMousePosition = virtualMouseInput.cursorTransform.position;
        virtualMouseInput.gameObject.GetComponentInChildren<Image>().enabled = true;
    }

    public void PlantPlantingHandler(){
        StartCoroutine(OnPlantPlanted());
    }
    
    private IEnumerator OnPlantPlanted()
    {
        //Have to add checks to make sure they are on a tile
        if (isPlantSelected && selectedTile.GetComponent<Cell>().tileValid)
        {
            if(Mathf.Abs((this.transform.position - selectedTile.transform.position).magnitude) < 12)
            {
                //enrouteToPlant = false;
                isPlantSelected = false;
                Cell activeTileCell = selectedTile.GetComponent<Cell>();
                Destroy(plantSelected);
                //This is a good place to initiate a planting animation
                yield return plantTime;
                PlantPlant(activeTileCell);
                plantSelectedType = PlantSelectedType.NONE;
                playerInput.SwitchCurrentActionMap("EarthPlayerDefault");
                virtualMouseInput.gameObject.GetComponentInChildren<Image>().enabled = false;
            }
            else
            {
                ApproachPlant();
            }
        }
        else if(isPlantSelected && !selectedTile.GetComponent<Cell>().tileValid)
        {
            //Display error message
            StartCoroutine(InvalidPlantLocation());
            yield break;
        }
        else
        {
            yield break;
        }
    }

    private IEnumerator InvalidPlantLocation()
    {
        displayText.text = "Invalid plant placement";
        yield return plantTime;
        displayText.text = "";
    }

    private void ApproachPlant()
    {
        earthAgent.enabled = true;
        earthAgent.SetDestination(selectedTile.transform.position);
        enrouteToPlant = true;
    }

    private void PlantPlant(Cell activeTileCell)
    {
        //We will want to add checks to make sure the tile type is valid, and check whether they are selecting a water or land tile
        //Pick the right plant based on the type of plant selected, and the tile selected, and then consume the appropriate seed
        if (plantSelectedType == PlantSelectedType.TREE)
        {
            tempPlantPlanted = Instantiate(treePrefab, activeTileCell.buildingTarget.transform);
            inventory.RemoveItemByName("TreeSeed"); //remove the item "TreeSeed"
        }
        else if (plantSelectedType == PlantSelectedType.FLOWER)
        {
            if(currentTileSelectedType == TileSelectedType.LAND)
            {
                tempPlantPlanted = Instantiate(landFlowerPrefab, activeTileCell.buildingTarget.transform);
                inventory.RemoveItemByName("TreeLog"); //remove the item "TreeSeed"
            }
            else if (currentTileSelectedType == TileSelectedType.WATER)
            {
                tempPlantPlanted = Instantiate(waterFlowerPrefab, activeTileCell.buildingTarget.transform);
            }

        }
        else if (plantSelectedType == PlantSelectedType.GRASS)
        {
            if (currentTileSelectedType == TileSelectedType.LAND)
            {
                tempPlantPlanted = Instantiate(landGrassPrefab, activeTileCell.buildingTarget.transform);
            }
            else if (currentTileSelectedType == TileSelectedType.WATER)
            {
                tempPlantPlanted = Instantiate(waterGrassPrefab, activeTileCell.buildingTarget.transform);
            }
        }
        plantsPlanted.Add(tempPlantPlanted);
        activeTileCell.placedObject = plantSelected;
        activeTileCell.tileHasBuild = true;
    }

    public void CancelPlant()
    {
        if (isPlantSelected)
        {
            isPlantSelected = false;
            plantSelectedType = PlantSelectedType.NONE;
            Destroy(plantSelected);
            playerInput.SwitchCurrentActionMap("EarthPlayerDefault");
        }
    }

    public void RemovePlant()
    {

    }

    private void ActivateTile()
    {
        Ray cameraRay = mainCamera.ScreenPointToRay(virtualMousePosition);
        RaycastHit hit;
        if (Physics.Raycast(cameraRay, out hit, 1000, tileMask))
        {
            selectedTile = hit.transform.gameObject.GetComponentInParent<Cell>().gameObject;
        }
        
    }

    public void OnInteract()
    {

    }
}
