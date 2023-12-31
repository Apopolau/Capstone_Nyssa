using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EarthPlayer : MonoBehaviour
{
    [SerializeField] Button treeButton;
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

    public enum PlantSelectedType { TREE, FLOWER, GRASS}
    public PlantSelectedType plantSelectedType;
    public GameObject selectedTile;

    private WaitForSeconds plantTime;

    [SerializeField] TMPro.TextMeshProUGUI displayText;

    // Start is called before the first frame update
    void Start()
    {
        treeButton.onClick.AddListener(() => OnPlantSelected(plantSelectedType));
        plantTime = new WaitForSeconds(2);
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();
    }

    public void HandleInputs()
    {
        //Want to set this to controller button
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(OnPlantPlanted());
        }
    }

    public void OnPlantSelected(PlantSelectedType plantButtonPressed)
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
            plantSelectedType = plantButtonPressed;

            //Handles what kind of plant we're planting
            if (plantSelectedType == PlantSelectedType.TREE)
            {
                plantSelected = Instantiate(treePreviewPrefab, plantParent.transform);
            }
            else if (plantSelectedType == PlantSelectedType.FLOWER)
            {
                plantSelected = Instantiate(landFlowerPreviewPrefab, plantParent.transform);
            }
            else if (plantSelectedType == PlantSelectedType.GRASS)
            {
                plantSelected = Instantiate(landGrassPreviewPrefab, plantParent.transform);
            }
            plantSelected.transform.position = this.transform.position;
        }
    }

    public IEnumerator OnPlantPlanted()
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
}
