using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Creatable
{
    private int currentHealth;
    [SerializeField] public PlantStats stats;
    public GameObject plantObject;
    public SpriteRenderer[] plantVisuals;
    private Cell tilePlantedOn;

    public int currentPollutionContribution;
    private int storedSunlight;
    private int storedWater;
    private int growthPoints;
    PlantStats.PlantStage currentPlantStage;

    // Start is called before the first frame update
    void Awake()
    {
        tilePlantedOn = this.gameObject.transform.parent.GetComponentInParent<Cell>();
        plantVisuals = plantObject.GetComponentsInChildren<SpriteRenderer>();
        currentPlantStage = PlantStats.PlantStage.SEEDLING;
        plantObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        currentPollutionContribution = stats.seedlingAirPollutionBonus;
        
        PlacePlant(stats.seedlingScale, stats.seedlingTileOffset);
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
        //If it's a water plant it doesn't need rain to grow
        if (tilePlantedOn.terrainType == Cell.TerrainType.WATER)
        {
            if (storedSunlight > 0)
            {
                growthPoints++;
                storedSunlight--;
                //we want our plants to heal if they're not full health while they're growing
                //We may choose to add a check where they don't heal if they're actively being affected by a monster
                if (currentHealth < stats.maxHealth)
                {
                    currentHealth++;
                }
            }
        }
        else
        {
            if (storedSunlight > 0 && storedWater > 0)
            {
                growthPoints++;
                storedSunlight--;
                storedWater--;
                //we want our plants to heal if they're not full health while they're growing
                //We may choose to add a check where they don't heal if they're actively being affected by a monster
                if (currentHealth < stats.maxHealth)
                {
                    currentHealth++;
                }
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
                currentPollutionContribution = stats.sproutAirPollutionBonus;
                SpriteRenderer[] spriteRenderers = this.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer spriteRenderer in spriteRenderers)
                {
                    spriteRenderer.sprite = stats.sproutImage;
                }
                PlacePlant(stats.sproutScale, stats.sproutTileOffset);
                HandleTreeColliders(0.5f, 5);
            }
            else if(currentPlantStage == PlantStats.PlantStage.SPROUT)
            {
                yield return new WaitForSeconds(stats.sproutGrowTime);
                currentPlantStage = PlantStats.PlantStage.JUVENILE;
                currentPollutionContribution = stats.juvenileAirPollutionBonus;
                SpriteRenderer[] spriteRenderers = this.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer spriteRenderer in spriteRenderers)
                {
                    spriteRenderer.sprite = stats.juvenileImage;
                }
                PlacePlant(stats.juvenileScale, stats.juvenileTileOffset);
                HandleTreeColliders(1f, 5);
            }
            else if(currentPlantStage == PlantStats.PlantStage.JUVENILE)
            {
                yield return new WaitForSeconds(stats.juvenileGrowTime);
                currentPlantStage = PlantStats.PlantStage.MATURE;
                currentPollutionContribution = stats.matureAirPollutionBonus;
                SpriteRenderer[] spriteRenderers = this.GetComponentsInChildren<SpriteRenderer>();
                foreach(SpriteRenderer spriteRenderer in spriteRenderers)
                {
                    spriteRenderer.sprite = stats.matureImage;
                }
                PlacePlant(stats.matureScale, stats.matureTileOffset);
                HandleTreeColliders(2f, 5);
            }
            else if(currentPlantStage == PlantStats.PlantStage.MATURE)
            {
                StopCoroutine(AdvancePlantStage());
                yield break;
            }
        }
        
    }

    private void HandleTreeColliders(float colliderRadius, float colliderHeight)
    {
        if (this.GetComponent<CapsuleCollider>() != null)
        {
            CapsuleCollider collider = this.GetComponent<CapsuleCollider>();
            collider.radius = colliderRadius;
            collider.height = colliderHeight;
            //collider.center = colliderYPosition;
        }
    }

    private void PlacePlant(float scale, float tileOffset)
    {
        plantObject.transform.rotation = Quaternion.Euler(0, 45, 0);
        if(plantVisuals.Length > 1)
        {
            int i = 0;
            foreach (SpriteRenderer plantVisual in plantVisuals)
            {
                plantVisual.transform.localScale = new Vector3(scale, scale, 1);
                float yOffset = (plantVisual.GetComponent<SpriteRenderer>().sprite.bounds.extents.y * scale);
                plantVisual.transform.localPosition = new Vector3(plantVisual.transform.localPosition.x, yOffset, plantVisual.transform.localPosition.z);
                i++;
            }
        }
        else
        {
            plantVisuals[0].transform.localScale = new Vector3(scale, scale, 1);
            float yOffset = (plantVisuals[0].GetComponent<SpriteRenderer>().sprite.bounds.extents.y * scale);
            plantVisuals[0].gameObject.transform.parent.gameObject.transform.localPosition = new Vector3(-tileOffset, yOffset, -tileOffset);
            //plantVisuals[0].GetComponentInParent<GameObject>().transform.localPosition = new Vector3(-tileOffset, yOffset, -tileOffset);
        }
    }
}
