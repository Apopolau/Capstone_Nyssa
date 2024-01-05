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
    public GameObject plantPreview;
    private GameObject treePrefab;
    private GameObject landGrassPrefab;
    private GameObject waterGrassPrefab;
    private GameObject landFlowerPrefab;
    private GameObject waterFlowerPrefab;

    public enum PlantSelectedType { TREE, FLOWER, GRASS}
    public PlantSelectedType plantSelectedType;

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
        if (Input.GetKeyDown("Space"))
        {
            //Have to add checks to make sure they are on a tile
            if (isPlantSelected)
            {
                //We will want to add checks to make sure the tile type is valid, and check whether they are selecting a water or land tile
                if(plantSelectedType == PlantSelectedType.TREE)
                {
                    plantSelected = Instantiate(treePrefab, plantParent.transform);
                }
                if (plantSelectedType == PlantSelectedType.FLOWER)
                {
                    plantSelected = Instantiate(landFlowerPrefab, plantParent.transform);
                }
                if (plantSelectedType == PlantSelectedType.GRASS)
                {
                    plantSelected = Instantiate(landGrassPrefab, plantParent.transform);
                }
                isPlantSelected = false;
            }
        }
    }

    public void OnPlantSelected(PlantSelectedType plantButtonPressed)
    {
        if (isPlantSelected)
        {

            isPlantSelected = false;
        }
        if (!isPlantSelected)
        {
            //We select a type of plant and make a transparent version of it with no stats
            isPlantSelected = true;
            plantSelectedType = plantButtonPressed;
        }
    }

    public void OnPlantPlanted()
    {

    }
}
