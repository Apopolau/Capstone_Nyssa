using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Creatable
{
    [SerializeField] PlantStats stats;
    float plantTime;
    PlantStats.PlantStage currentPlantStage;
    //Sprite currentImage;
    public GameObject spriteObject;

    // Start is called before the first frame update
    void Awake()
    {
        plantTime = Time.deltaTime;
        currentPlantStage = PlantStats.PlantStage.SEEDLING;
        spriteObject.transform.position = new Vector3(this.gameObject.transform.position.x, stats.seedlingYOffset, this.gameObject.transform.position.z);
        spriteObject.transform.localScale = new Vector3(stats.seedlingScale, stats.seedlingScale, 1);
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlantGrowth();
    }

    private void FixedUpdate()
    {
        
    }

    void HandlePlantGrowth()
    {
        if(currentPlantStage == PlantStats.PlantStage.SEEDLING)
        {
            if(Time.deltaTime > plantTime + stats.seedlingGrowTime)
            {
                currentPlantStage = PlantStats.PlantStage.SPROUT;
                this.GetComponentInChildren<SpriteRenderer>().sprite = stats.sproutImage;
                spriteObject.transform.position = new Vector3(this.gameObject.transform.position.x, stats.seedlingYOffset, this.gameObject.transform.position.z);
                spriteObject.transform.localScale = new Vector3(stats.sproutScale, stats.seedlingScale, 1);
            }
        }
        else if (currentPlantStage == PlantStats.PlantStage.SPROUT)
        {
            if (Time.deltaTime > plantTime + stats.sproutGrowTime)
            {
                currentPlantStage = PlantStats.PlantStage.JUVENILE;
                this.GetComponentInChildren<SpriteRenderer>().sprite = stats.juvenileImage;
                spriteObject.transform.position = new Vector3(this.gameObject.transform.position.x, stats.juvenileYOffset, this.gameObject.transform.position.z);
                spriteObject.transform.localScale = new Vector3(stats.sproutScale, stats.juvenileScale, 1);
            }
        }
        else if (currentPlantStage == PlantStats.PlantStage.JUVENILE)
        {
            if (Time.deltaTime > plantTime + stats.juvenileGrowTime)
            {
                currentPlantStage = PlantStats.PlantStage.MATURE;
                this.GetComponentInChildren<SpriteRenderer>().sprite = stats.matureImage;
                spriteObject.transform.position = new Vector3(this.gameObject.transform.position.x, stats.matureYOffset, this.gameObject.transform.position.z);
                spriteObject.transform.localScale = new Vector3(stats.sproutScale, stats.matureScale, 1);
            }
        }
    }
}
