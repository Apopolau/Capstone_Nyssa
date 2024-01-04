using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EarthPlayer : MonoBehaviour
{
    [SerializeField] Button treeButton;

    public bool isPlantSelected = false;
    public GameObject plantSelected;

    // Start is called before the first frame update
    void Start()
    {
        treeButton.onClick.AddListener(() => OnPlantSelected(plantSelected));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlantSelected(GameObject buttonPlantSelected)
    {
        Debug.Log("Player selected a plant");
        if (isPlantSelected)
        {
            Debug.Log("Switching plant selected");
            Destroy(plantSelected);
            isPlantSelected = false;
        }
        if (!isPlantSelected)
        {
            Debug.Log("Selected new plant");
            isPlantSelected = true;
            plantSelected = Instantiate(buttonPlantSelected, this.transform);
        }
    }


}
