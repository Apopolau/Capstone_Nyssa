using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // Start is called before the first frame update
    void Start()
    {
        treeButton.onClick.AddListener(() => OnPlantSelected(plantSelectedType));
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
            OnPlantPlanted();
        }
    }

    public void OnPlantSelected(PlantSelectedType plantButtonPressed)
    {
        Cell activeTileCell = selectedTile.GetComponent<Cell>();
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

    public void OnPlantPlanted()
    {
        //Have to add checks to make sure they are on a tile
        if (isPlantSelected)
        {
            Cell activeTileCell = selectedTile.GetComponent<Cell>();
            Destroy(plantSelected);
            //We will want to add checks to make sure the tile type is valid, and check whether they are selecting a water or land tile
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
            //plantSelected.transform.position = selectedTile.GetComponent<Cell>().buildingTarget.transform.position;
            isPlantSelected = false;
        }
    }
}
