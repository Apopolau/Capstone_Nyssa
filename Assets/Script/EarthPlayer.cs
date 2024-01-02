using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthPlayer : MonoBehaviour
{
    public bool isPlantSelected;
    public GameObject plantSelected;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlantSelected(GameObject buttonPlantSelected)
    {
        if (isPlantSelected)
        {
            Destroy(plantSelected);
            isPlantSelected = false;
        }
        if (!isPlantSelected)
        {
            isPlantSelected = true;
            plantSelected = Instantiate(buttonPlantSelected, this.transform);
        }
        
    }
}
