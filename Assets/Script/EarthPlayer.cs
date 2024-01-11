using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class EarthPlayer : MonoBehaviour
{
    //[SerializeField] Button treeButton;
    [SerializeField] GameObject plantParent;

    public bool isPlantSelected = false;
    public GameObject plantSelected;
    public List<GameObject> plantsPlanted;
    public GameObject tempPlantPlanted;
    public GameObject plantPreview;

    [Header("Plant Objects")]
    [SerializeField] private GameObject treePrefab;
    [SerializeField] private GameObject landGrassPrefab;
    [SerializeField] private GameObject waterGrassPrefab;
    [SerializeField] private GameObject landFlowerPrefab;
    [SerializeField] private GameObject waterFlowerPrefab;

    [Header("Preview Objects")]
    [SerializeField] private GameObject treePreviewPrefab;
    [SerializeField] private GameObject landGrassPreviewPrefab;
    [SerializeField] private GameObject waterGrassPreviewPrefab;
    [SerializeField] private GameObject landFlowerPreviewPrefab;
    [SerializeField] private GameObject waterFlowerPreviewPrefab;

    public enum PlantSelectedType {NONE, TREE, FLOWER, GRASS }
    public PlantSelectedType plantSelectedType;
    public GameObject selectedTile;

    private WaitForSeconds plantTime;

    [SerializeField] TextMeshProUGUI displayText;

    private PlayerInput playerInput;

    [SerializeField] private VirtualMouseInput virtualMouseInput;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask tileMask;
    Vector2 virtualMousePosition;

    private void Awake()
    {
        //virtualMouseInput = GetComponent<VirtualMouseInput>();
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
    }

    private void LateUpdate()
    {
        if (isPlantSelected)
        {
            virtualMousePosition = virtualMouseInput.virtualMouse.position.value;
        }
        
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
            isPlantSelected = true;
            plantSelectedType = PlantSelectedType.TREE;
            plantSelected = Instantiate(treePreviewPrefab, plantParent.transform);
            
            plantSelected.transform.position = this.transform.position;
        }
        playerInput.SwitchCurrentActionMap("PlantIsSelected");
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
            isPlantSelected = true;
            plantSelectedType = PlantSelectedType.GRASS;
            plantSelected = Instantiate(landGrassPreviewPrefab, plantParent.transform);
        }
        playerInput.SwitchCurrentActionMap("PlantIsSelected");
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
            isPlantSelected = true;
            plantSelectedType = PlantSelectedType.FLOWER;
            plantSelected = Instantiate(landFlowerPreviewPrefab, plantParent.transform);
        }
        playerInput.SwitchCurrentActionMap("PlantIsSelected");
    }

    public void PlantPlantingHandler(){
        StartCoroutine(OnPlantPlanted());
    }
    
    private IEnumerator OnPlantPlanted()
    {
        //Have to add checks to make sure they are on a tile
        if (isPlantSelected && selectedTile.GetComponent<Cell>().tileValid)
        {
            isPlantSelected = false;
            Cell activeTileCell = selectedTile.GetComponent<Cell>();
            Destroy(plantSelected);
            //This is a good place to initiate a planting animation
            yield return plantTime;
            PlantPlant(activeTileCell);
            plantSelectedType = PlantSelectedType.NONE;
            playerInput.SwitchCurrentActionMap("EarthPlayerDefault");
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

    private void PlantPlant(Cell activeTileCell)
    {
        //We will want to add checks to make sure the tile type is valid, and check whether they are selecting a water or land tile
        //Pick the right plant based on the type of plant selected, and the tile selected, and then consume the appropriate seed
        if (plantSelectedType == PlantSelectedType.TREE)
        {
            tempPlantPlanted = Instantiate(treePrefab, activeTileCell.buildingTarget.transform);
        }
        else if (plantSelectedType == PlantSelectedType.FLOWER)
        {
            tempPlantPlanted = Instantiate(landFlowerPrefab, activeTileCell.buildingTarget.transform);
        }
        else if (plantSelectedType == PlantSelectedType.GRASS)
        {
            tempPlantPlanted = Instantiate(landGrassPrefab, activeTileCell.buildingTarget.transform);
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

    public void MoveCursor()
    {

    }

    private void ActivateTile()
    {
        //Debug.Log("Checking to activate tile");
        Ray cameraRay = mainCamera.ScreenPointToRay(virtualMousePosition);
        RaycastHit hit;
        if (Physics.Raycast(cameraRay, out hit, 50, tileMask))
        {
            //if(HitInfo.rigidbody.gameObject.layer == tileMask)
            //{
                Debug.Log("Found a tile");
                //HitInfo.transform.GetComponent<Cell>().tileIsActivated = true;
                selectedTile = hit.transform.gameObject;
            //}
        }
        
    }
}
