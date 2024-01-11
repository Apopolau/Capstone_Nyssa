using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Creatable
{
    private int currentHealth;
    [SerializeField] PlantStats stats;
    public GameObject plantVisual;

    private int storedSunlight;
    private int storedWater;
    private int growthPoints;
    PlantStats.PlantStage currentPlantStage;

    // Start is called before the first frame update
    void Awake()
    {
        currentPlantStage = PlantStats.PlantStage.SEEDLING;
        plantVisual.transform.position = new Vector3(this.gameObject.transform.position.x, stats.seedlingYOffset, this.gameObject.transform.position.z);
        plantVisual.transform.localScale = new Vector3(stats.seedlingScale, stats.seedlingScale, 1);
        StartCoroutine(AdvancePlantStage());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    private void growPlant()
    {
        //Add a check to see if it is currently sunny or rainy when these states exist
        //Put the check so that stored resources are not drained if the resource is still active
        if(storedSunlight > 0 && storedWater > 0){
            growthPoints++;
            storedSunlight--;
            storedWater--;
            //we want our plants to heal if they're not full health while they're growing
            //We may choose to add a check where they don't heal if they're actively being affected by a monster
            if(currentHealth < stats.maxHealth)
            {
                currentHealth++;
            }
        }
        if(storedSunlight < 0)
        {
            storedSunlight = 0;
        }
        if(storedWater < 0)
        {
            storedWater = 0;
        }
    }

    private void storeNutrients()
    {
        //if sunny, store sunlight
        //May need to check factors like pollution, time of day, etc to figure out how much you get

        //if rainy, store water
    }

    //Refactor this back out of being an IEnumerator, use growth points instead
    private IEnumerator AdvancePlantStage()
    {
        while (true)
        {
            if (currentPlantStage == PlantStats.PlantStage.SEEDLING)
            {
                yield return new WaitForSeconds(stats.seedlingGrowTime);
                Debug.Log("Plant should be growing");
                currentPlantStage = PlantStats.PlantStage.SPROUT;
                this.GetComponentInChildren<SpriteRenderer>().sprite = stats.sproutImage;
                plantVisual.transform.position = new Vector3(this.gameObject.transform.position.x, stats.seedlingYOffset, this.gameObject.transform.position.z);
                plantVisual.transform.localScale = new Vector3(stats.seedlingScale, stats.seedlingScale, 1);
            }
            else if(currentPlantStage == PlantStats.PlantStage.SPROUT)
            {
                yield return new WaitForSeconds(stats.sproutGrowTime);
                currentPlantStage = PlantStats.PlantStage.JUVENILE;
                this.GetComponentInChildren<SpriteRenderer>().sprite = stats.juvenileImage;
                plantVisual.transform.position = new Vector3(this.gameObject.transform.position.x, stats.juvenileYOffset, this.gameObject.transform.position.z);
                plantVisual.transform.localScale = new Vector3(stats.juvenileScale, stats.juvenileScale, 1);
            }
            else if(currentPlantStage == PlantStats.PlantStage.JUVENILE)
            {
                yield return new WaitForSeconds(stats.juvenileGrowTime);
                currentPlantStage = PlantStats.PlantStage.MATURE;
                this.GetComponentInChildren<SpriteRenderer>().sprite = stats.matureImage;
                plantVisual.transform.position = new Vector3(this.gameObject.transform.position.x, stats.matureYOffset, this.gameObject.transform.position.z);
                plantVisual.transform.localScale = new Vector3(stats.matureScale, stats.matureScale, 1);
            }
            else if(currentPlantStage == PlantStats.PlantStage.MATURE)
            {
                StopCoroutine(AdvancePlantStage());
                yield break;
            }
        }
        
    }
}
