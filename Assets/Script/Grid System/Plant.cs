using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Creatable
{

    [Header("Set these on the prefab")]
    [SerializeField] public PlantStats stats;
    [SerializeField] List<LevelManagerObject> levelManagers;
    public GameObject plantObject;
    [SerializeField] WeatherState weatherState;

    [Header("These set themselves")]
    
    private Stat storedSunlight;
    private Stat storedWater;
    public SpriteRenderer[] plantVisuals;
    private Cell tilePlantedOn;
    bool waterPlant = false;

    public int currentPollutionContribution;
    private int growthPoints;
    private int growthRate = 1;
    public bool isSmothered;
    PlantStats.PlantStage currentPlantStage;

    private GameObject seed;

    public new event System.Action<int, int> OnHealthChanged;

    // Start is called before the first frame update
    void Awake()
    {
        //Gather our references
        tilePlantedOn = this.gameObject.transform.parent.GetComponentInParent<Cell>();
        plantVisuals = plantObject.GetComponentsInChildren<SpriteRenderer>();

        //Initialize Seedling stats
        currentPlantStage = PlantStats.PlantStage.SEEDLING;
        plantObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        currentPollutionContribution = stats.seedlingAirPollutionBonus;
        PlacePlant(stats.seedlingScale, stats.seedlingTileOffset);

        //Initialize our stats
        health = new Stat(stats.maxHealth, stats.maxHealth, false);
        storedSunlight = new Stat(100, 0, true);
        storedWater = new Stat(100, 0, true);

        //Special considerations for water plants
        if (tilePlantedOn.terrainType == Cell.TerrainType.WATER)
        {
            waterPlant = true;
            storedWater.current = 100;
            storedWater.low = false;
        }

        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        StoreNutrients();
        GrowPlant();
        AdvancePlantStage();
        ResolveStats();
        HandleAcidRain();
    }

    private void GrowPlant()
    {
        //Add a check to see if it is currently sunny or rainy when these states exist
        //Put the check so that stored resources are not drained if the resource is still active
        //If it's a water plant it doesn't need rain to grow
        if (waterPlant)
        {
            if (storedSunlight.current > 0)
            {
                growthPoints++;
                if (!weatherState.dayTime)
                {
                    storedSunlight.current -= Mathf.Clamp(-growthRate, 0, storedSunlight.max);
                }
                //we want our plants to heal if they're not full health while they're growing
                //We may choose to add a check where they don't heal if they're actively being affected by a monster
                if (health.current < health.max)
                {
                    health.current++;
                }
            }
        }
        else
        {
            if (storedSunlight.current > 0 && storedWater.current > 0)
            {
                growthPoints++;
                if (!weatherState.dayTime)
                {
                    storedSunlight.current -= Mathf.Clamp(-growthRate, 0, storedSunlight.max);
                }
                if (weatherState.skyState == WeatherState.SkyState.CLEAR)
                {
                    storedWater.current -= Mathf.Clamp(-growthRate, 0, storedWater.max);
                }
                //we want our plants to heal if they're not full health while they're growing
                //We may choose to add a check where they don't heal if they're actively being affected by a monster
                if (health.current < health.max)
                {
                    health.current++;
                }
            }
        }
    }

    private void StoreNutrients()
    {
        
        if (weatherState.dayTime && !isSmothered)
        {
            storedSunlight.current += Mathf.Clamp(growthRate, 0, 100);
        }
        
        if(weatherState.skyState == WeatherState.SkyState.RAINY && !waterPlant)
        {
            storedSunlight.current += Mathf.Clamp(growthRate, 0, 100);
        }
    }

    //Refactor this back out of being an IEnumerator, use growth points instead
    private void AdvancePlantStage()
    {

        if (currentPlantStage == PlantStats.PlantStage.SEEDLING && growthPoints >= stats.seedlingGrowTime)
        {
            //yield return new WaitForSeconds(stats.seedlingGrowTime);
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
            growthPoints = 0;
        }
        else if (currentPlantStage == PlantStats.PlantStage.SPROUT && growthPoints >= stats.sproutGrowTime)
        {
            //yield return new WaitForSeconds(stats.sproutGrowTime);
            currentPlantStage = PlantStats.PlantStage.JUVENILE;
            currentPollutionContribution = stats.juvenileAirPollutionBonus;
            SpriteRenderer[] spriteRenderers = this.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                spriteRenderer.sprite = stats.juvenileImage;
            }
            PlacePlant(stats.juvenileScale, stats.juvenileTileOffset);
            HandleTreeColliders(1f, 5);
            growthPoints = 0;
        }
        else if (currentPlantStage == PlantStats.PlantStage.JUVENILE && growthPoints >= stats.juvenileGrowTime)
        {
            //yield return new WaitForSeconds(stats.juvenileGrowTime);
            currentPlantStage = PlantStats.PlantStage.MATURE;
            currentPollutionContribution = stats.matureAirPollutionBonus;
            SpriteRenderer[] spriteRenderers = this.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                spriteRenderer.sprite = stats.matureImage;
            }
            PlacePlant(stats.matureScale, stats.matureTileOffset);
            HandleTreeColliders(2f, 5);
            growthPoints = 0;
        }
        else if (currentPlantStage == PlantStats.PlantStage.MATURE && growthPoints >= stats.matureSeedDropTime)
        {
            DropSeed();
            growthPoints = 0;
        }


    }

    private void DropSeed()
    {
        seed = Instantiate(stats.seedPrefab, this.transform);
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
        if (plantVisuals.Length > 1)
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

    private void ResolveStats()
    {
        //Change water state
        if(storedWater.current < storedWater.max / 4)
        {
            storedWater.low = true;
        }
        else
        {
            storedWater.low = false;
        }

        //Change sunlight state
        if (storedSunlight.current < storedSunlight.max / 4)
        {
            storedSunlight.low = true;
        }
        else
        {
            storedSunlight.low = false;
        }

        //Change health state
        if (health.current < health.max / 4)
        {
            health.low = true;
        }
        else
        {
            health.low = false;
        }
    }

    private void HandleAcidRain()
    {
        if(weatherState.skyState == WeatherState.SkyState.RAINY)
        {
            if (weatherState.acidRainState == WeatherState.AcidRainState.LIGHT)
            {
                TakeDamage(1);
            }
            else if(weatherState.acidRainState == WeatherState.AcidRainState.HEAVY)
            {
                TakeDamage(2);
            }
        } 
    }

    public override void TakeDamage(int damageTaken)
    {
        health.current -= Mathf.Clamp(damageTaken, 0, health.max);

        if (OnHealthChanged != null)
            OnHealthChanged(health.max, health.current);

        if (health.current <= 0)
        {
            PlantDies();
        }
    }

    public void PlantDies()
    {
        tilePlantedOn.tileHasBuild = false;
        tilePlantedOn.placedObject = null;
        Destroy(this.gameObject);
    }
}
