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
    [Header("These need to be set up in each scene")]
    [SerializeField] public GameObject plantParent;
    [SerializeField] public VirtualMouseInput virtualMouseInput;
    [SerializeField] public Camera mainCamera;
    [SerializeField] public TextMeshProUGUI displayText;

    [Header("Info for selecting plants")]
    public bool isPlantSelected = false;
    public GameObject plantSelected;
    public List<GameObject> plantsPlanted;
    public GameObject tempPlantPlanted;
    public GameObject plantPreview;
    [SerializeField] GameObject tileOutlinePrefab;
    public GameObject tileOutline;

    //Data for when the player picks a plant to plant
    public enum PlantSelectedType { NONE, TREE, FLOWER, GRASS }
    public PlantSelectedType plantSelectedType;

    //Data for the tile they're trying to plant on
    public enum TileSelectedType { LAND, WATER };
    public TileSelectedType currentTileSelectedType;
    public GameObject selectedTile;
    [SerializeField] private LayerMask tileMask;
    public Vector2 virtualMousePosition;
    
    private WaitForSeconds plantTime;

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

    [Header("Misc")]
    private NavMeshAgent earthAgent;
    public bool enrouteToPlant = false;
    private PlayerInput playerInput;
    private EarthPlayerControl earthControls;

    public bool interacting = false;
    public Inventory inventory; // hold a reference to the Inventory scriptable object

    private void Awake()
    {
        earthControls = GetComponent<EarthPlayerControl>();
        earthAgent = GetComponent<NavMeshAgent>();
        earthAgent.enabled = false;
        virtualMouseInput.gameObject.GetComponentInChildren<Image>().enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        plantTime = new WaitForSeconds(2);
        playerInput = GetComponent<PlayerInput>();
        //actions = new PlayerInputActions();
    }

    // Update is called once per frame
    void Update()
    {
        ActivateTile();
        if (enrouteToPlant && Mathf.Abs((this.transform.position - selectedTile.transform.position).magnitude) < 10.5f)
        {
            this.GetComponent<playerMovement>().ResetNavAgent();
            PlantPlantingHandler();
        }

    }

    private void LateUpdate()
    {
        if(earthControls.thisDevice == EarthPlayerControl.DeviceUsed.CONTROLLER)
        {
            virtualMousePosition = virtualMouseInput.virtualMouse.position.value;
        }
        else if(earthControls.thisDevice == EarthPlayerControl.DeviceUsed.KEYBOARD)
        {
            virtualMousePosition = Mouse.current.position.value;
        }
        
    }

    public void OnTreeSelected(InputAction.CallbackContext context)
    {
        //Debug.Log("oonnnniit");
     //   if (context.phase == InputActionPhase.Started)
      //  {
            //Debug.Log("innnnnniit");
            //We're going to want to check if they even have the seed for the plant they selected before we do anything else
            if (inventory.HasTypeSeed("Tree Seed"))
            {

                if (isPlantSelected)
                {
                    isPlantSelected = false;
                    Destroy(plantSelected);
                    Destroy(tileOutline);
                }
                if (!isPlantSelected)
                {
                    //We select a type of plant from the input and make a transparent version of it with no stats
                    plantSelectedType = PlantSelectedType.TREE;
                    plantSelected = Instantiate(treePreviewPrefab, plantParent.transform);
                    OnPlantSelectedWrapUp();
                }
            }
            else
            {
                StartCoroutine(InsufficientSeeds());
            }
       // }
    }

    public void OnGrassSelected(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
           
            if (inventory.HasTypeSeed("Grass Seed"))
            {
                if (isPlantSelected)
                {
                    isPlantSelected = false;
                    Destroy(plantSelected);
                    Destroy(tileOutline);
                }
                if (!isPlantSelected)
                {
                    //We select a type of plant from the input and make a transparent version of it with no stats
                    plantSelectedType = PlantSelectedType.GRASS;
                    plantSelected = Instantiate(landGrassPreviewPrefab, plantParent.transform);
                    OnPlantSelectedWrapUp();
                }
            }
            else
            {
                StartCoroutine(InsufficientSeeds());
            }
        }
    }
    public void SetCamera(Camera switchCam)
    {
        mainCamera = switchCam;

    }
    public void OnFlowerSelected(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (inventory.HasTypeSeed("Flower Seed"))
            {
                if (isPlantSelected)
                {
                    isPlantSelected = false;
                    Destroy(plantSelected);
                    Destroy(tileOutline);
                }
                if (!isPlantSelected)
                {
                    //We select a type of plant from the input and make a transparent version of it with no stats
                    plantSelectedType = PlantSelectedType.FLOWER;
                    plantSelected = Instantiate(landFlowerPreviewPrefab, plantParent.transform);
                    OnPlantSelectedWrapUp();
                }
            }
            else
            {
                StartCoroutine(InsufficientSeeds());
            }
        }
    }

    private void OnPlantSelectedWrapUp()
    {
        //Debug.Log("Wrapping up plant selection");
        isPlantSelected = true;
        plantSelected.transform.position = this.transform.position;
        playerInput.SwitchCurrentActionMap("PlantIsSelected");

        if (earthControls.thisDevice == EarthPlayerControl.DeviceUsed.CONTROLLER)
        {
            virtualMouseInput.cursorTransform.position = new Vector2(Screen.width / 2, Screen.height / 2);
            virtualMousePosition = virtualMouseInput.cursorTransform.position;
        }
        else if (earthControls.thisDevice == EarthPlayerControl.DeviceUsed.KEYBOARD)
        {
            virtualMousePosition = Mouse.current.position.value;
        }
        
        virtualMouseInput.gameObject.GetComponentInChildren<Image>().enabled = true;
        tileOutline = Instantiate(tileOutlinePrefab, this.transform);
    }

    public void PlantPlantingHandler()
    {
        StartCoroutine(OnPlantPlanted());
    }

    private IEnumerator OnPlantPlanted()
    {
        //Have to add checks to make sure they are on a tile
        if (isPlantSelected && selectedTile.GetComponent<Cell>().tileValid)
        {
            if (Mathf.Abs((this.transform.position - selectedTile.transform.position).magnitude) < 12)
            {
                //enrouteToPlant = false;
                isPlantSelected = false;
                Cell activeTileCell = selectedTile.GetComponent<Cell>();
                Destroy(plantSelected);
                Destroy(tileOutline);
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
        else if (isPlantSelected && !selectedTile.GetComponent<Cell>().tileValid)
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

    private IEnumerator InsufficientSeeds()
    {
        displayText.text = "Insufficient seeds of that type";
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
            inventory.RemoveItemByName("Tree Seed"); //remove the item "TreeSeed"
        }
        else if (plantSelectedType == PlantSelectedType.FLOWER)
        {
            inventory.RemoveItemByName("Flower Seed"); //remove the item "Flower Seed"
            if (currentTileSelectedType == TileSelectedType.LAND)
            {
                tempPlantPlanted = Instantiate(landFlowerPrefab, activeTileCell.buildingTarget.transform);
            }
            else if (currentTileSelectedType == TileSelectedType.WATER)
            {
                tempPlantPlanted = Instantiate(waterFlowerPrefab, activeTileCell.buildingTarget.transform);
            }

        }
        else if (plantSelectedType == PlantSelectedType.GRASS)
        {
            inventory.RemoveItemByName("Grass Seed"); //remove the item "Flower Seed"
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
            Destroy(tileOutline);
            virtualMouseInput.gameObject.GetComponentInChildren<Image>().enabled = false;
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

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("Interacting");
            interacting = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Debug.Log("Not interacting anymore");
            interacting = false;
        }
    }
}
