using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Creatable
{

    [Header("Set these on the prefab")]
    [SerializeField] public PlantStats stats;
    [SerializeField] List<LevelManagerObject> levelManagers;
    [SerializeField] private GameObjectRuntimeSet playerSet;
    private CelestialPlayer celestialPlayer;
    public GameObject plantObject;
    [SerializeField] WeatherState weatherState;
    [SerializeField] GameObject energyPrefab;
    [SerializeField] GameObjectRuntimeSet seedSet;
    [SerializeField] GameObjectRuntimeSet energySet;
    Inventory inventory;

    [Header("These set themselves")]
    [SerializeField] private Stat storedSunlight;
    [SerializeField] private Stat storedWater;

    // Reference to the UI elements
    public GameObject waterUI;
    public GameObject sunlightUI;

    public SpriteRenderer[] plantVisuals;
    private Cell tilePlantedOn;
    bool waterPlant = false;

    public int currentPollutionContribution;
    [SerializeField] private int growthPoints;
    private WaitForSeconds growthRate = new WaitForSeconds(1);
    private bool isSmothered = false;
    private bool isDying = false;
    public PlantStats.PlantStage currentPlantStage;

    private GameObject seed;
    //private GameObject energyDrop;
    [SerializeField] private GameObject energyNode;
    private GameObject logs;

    public event System.Action<int, int> OnHealthChanged;

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

    private void Start()
    {
        StartCoroutine(GrowPlant());
        StartCoroutine(StoreNutrients());
        StartCoroutine(HandleAcidRain());

        foreach (GameObject go in playerSet.Items)
        {
            if (go.GetComponent<CelestialPlayer>())
            {
                celestialPlayer = go.GetComponent<CelestialPlayer>();
            }
        }
    }


    private void FixedUpdate()
    {
        AdvancePlantStage();
        ResolveStats();
    }

    private IEnumerator GrowPlant()
    {
        while (true)
        {
            yield return growthRate;
            //Add a check to see if it is currently sunny or rainy when these states exist
            //Put the check so that stored resources are not drained if the resource is still active
            //If it's a water plant it doesn't need rain to grow
            if (waterPlant)
            {
                if (weatherState.dayTime && !isSmothered)
                {
                    growthPoints++;
                    HandleHealthChanges(1);
                }
                else
                {
                    UseReserveNutrients();
                }
            }
            else
            {
                if (weatherState.dayTime && weatherState.GetSkyState() == WeatherState.SkyState.RAINY && !isSmothered)
                {
                    growthPoints++;
                    HandleHealthChanges(1);
                }
                else
                {
                    UseReserveNutrients();
                }
            }
        }

    }

    private void UseReserveNutrients()
    {

        if (waterPlant)
        {
            if (storedSunlight.current > 0)
            {
                growthPoints++;
                storedSunlight.current -= Mathf.Clamp(1, 0, storedSunlight.max);
                //we want our plants to heal if they're not full health while they're growing
                //We may choose to add a check where they don't heal if they're actively being affected by a monster
                HandleHealthChanges(1);
            }
        }
        else
        {
            if (storedSunlight.current > 0 && storedWater.current > 0)
            {
                growthPoints++;
                if (!weatherState.dayTime)
                {
                    storedSunlight.current -= Mathf.Clamp(1, 0, storedSunlight.max);
                }
                if (weatherState.GetSkyState() == WeatherState.SkyState.CLEAR)
                {
                    storedWater.current -= Mathf.Clamp(1, 0, storedWater.max);
                }
                //we want our plants to heal if they're not full health while they're growing
                //We may choose to add a check where they don't heal if they're actively being affected by a monster
                HandleHealthChanges(1);
            }
        }

    }

    private IEnumerator StoreNutrients()
    {
        while (true)
        {
            yield return growthRate;

            if (weatherState.dayTime && !isSmothered)
            {
                storedSunlight.current += Mathf.Clamp(1, 0, storedSunlight.max);
            }

            if (weatherState.GetSkyState() == WeatherState.SkyState.RAINY && !waterPlant && !isSmothered)
            {
                storedWater.current += Mathf.Clamp(3, 0, storedWater.max);
            }
        }
    }

    private void HandleHealthChanges(int amount)
    {
        health.current += amount;

        health.current = Mathf.Clamp(health.current, 0, health.max);

        if (OnHealthChanged != null)
        {
            OnHealthChanged(health.max, health.current);
        }
    }

    //Handles what to do when the plant has been in each stage of growth with enough nutrients
    private void AdvancePlantStage()
    {

        if (currentPlantStage == PlantStats.PlantStage.SEEDLING && growthPoints >= stats.seedlingGrowTime)
        {
            currentPlantStage = PlantStats.PlantStage.SPROUT;
            currentPollutionContribution = stats.sproutAirPollutionBonus;
            foreach (SpriteRenderer spriteRenderer in plantVisuals)
            {
                spriteRenderer.sprite = stats.sproutImage;
            }
            PlacePlant(stats.sproutScale, stats.sproutTileOffset);
            HandleTreeColliders(0.5f, 1, 5, 0, Vector3.zero);

            int energyQuantity = stats.seedlingEnergy;
            celestialPlayer.IncreaseEnergy(energyQuantity);
            SpawnEnergyNode();

            growthPoints = 0;
        }
        else if (currentPlantStage == PlantStats.PlantStage.SPROUT && growthPoints >= stats.sproutGrowTime)
        {
            currentPlantStage = PlantStats.PlantStage.JUVENILE;
            currentPollutionContribution = stats.juvenileAirPollutionBonus;
            foreach (SpriteRenderer spriteRenderer in plantVisuals)
            {
                spriteRenderer.sprite = stats.juvenileImage;
            }
            PlacePlant(stats.juvenileScale, stats.juvenileTileOffset);
            HandleTreeColliders(1f, 50, 1, 0, Vector3.zero);

            int energyQuantity = stats.sproutEnergy;
            celestialPlayer.IncreaseEnergy(energyQuantity);
            SpawnEnergyNode();


            growthPoints = 0;
        }
        else if (currentPlantStage == PlantStats.PlantStage.JUVENILE && growthPoints >= stats.juvenileGrowTime)
        {
            currentPlantStage = PlantStats.PlantStage.MATURE;
            currentPollutionContribution = stats.matureAirPollutionBonus;
            foreach (SpriteRenderer spriteRenderer in plantVisuals)
            {
                spriteRenderer.sprite = stats.matureImage;
            }
            PlacePlant(stats.matureScale, stats.matureTileOffset);
            HandleTreeColliders(2.5f, 25, 10, 14, new Vector3(0, 18.5f, -1.5f));

            int energyQuantity = stats.juvenileEnergy;
            celestialPlayer.IncreaseEnergy(energyQuantity);
            SpawnEnergyNode();

            DropSeed();

            growthPoints = 0;
        }
        else if (currentPlantStage == PlantStats.PlantStage.MATURE && growthPoints >= stats.matureSeedDropTime)
        {
            int energyQuantity = stats.matureEnergy;
            celestialPlayer.IncreaseEnergy(energyQuantity);
            SpawnEnergyNode();

            DropSeed();
            growthPoints = 0;
        }
    }

    private void DropSeed()
    {
        List<GameObject> thisTypeSeed = new List<GameObject>();
        foreach (GameObject go in seedSet.Items)
        {
            if (go.GetComponent<PickupObject>().GetItemName() == stats.seedPrefab.GetComponent<PickupObject>().GetItemName())
            {
                thisTypeSeed.Add(go);
            }
        }
        if (thisTypeSeed.Count < 5)
        {
            seed = Instantiate(stats.seedPrefab, this.transform);
            //seed.GetComponentInChildren<SpriteRenderer>().material.renderQueue = this.GetComponentInChildren<SpriteRenderer>().material.renderQueue + 1;
            seed.GetComponent<PickupObject>().SetInventory(inventory);
        }
    }

    private void SpawnEnergyNode()
    {
        Vector3 spawnOffset = new Vector3(0, 3, 0);
        Vector3 spawnPos = this.transform.position + spawnOffset;
        GameObject node = Instantiate(energyNode, this.transform.GetChild(1).GetChild(0).GetChild(0).transform);
        node.GetComponent<EnergyNode>().TurnCounterOff();
        node.GetComponent<RectTransform>().localScale = new Vector3(0.1f, 0.1f, 0.1f);
        node.transform.position = spawnPos;
    }

    private void HandleTreeColliders(float c_ColliderRadius, float c_ColliderHeight, float c_ColliderCenter, float sphereRadius, Vector3 sphereCenter)
    {
        if (this.GetComponent<CapsuleCollider>() != null)
        {
            CapsuleCollider collider = this.GetComponent<CapsuleCollider>();
            collider.radius = c_ColliderRadius;
            collider.height = c_ColliderHeight;
            collider.center = new Vector3(collider.center.x, c_ColliderCenter, collider.center.z);
            //collider.center = colliderYPosition;
        }
        if (this.GetComponent<SphereCollider>() != null)
        {
            SphereCollider collider = this.GetComponent<SphereCollider>();
            collider.radius = sphereRadius;
            collider.center = sphereCenter;
        }
    }

    private void PlacePlant(float scale, float tileOffset)
    {

        if (plantVisuals.Length > 1)
        {
            int i = 0;
            foreach (SpriteRenderer plantVisual in plantVisuals)
            {
                plantVisual.transform.rotation = Quaternion.Euler(0, 45, 0);
                plantVisual.transform.localScale = new Vector3(scale, scale, 1);
                float yOffset = (plantVisual.GetComponent<SpriteRenderer>().sprite.bounds.extents.y * scale);
                plantVisual.transform.localPosition = new Vector3(plantVisual.transform.localPosition.x, yOffset, plantVisual.transform.localPosition.z);
                i++;
            }
        }
        else
        {
            plantObject.transform.rotation = Quaternion.Euler(0, 45, 0);
            plantVisuals[0].transform.localScale = new Vector3(scale, scale, 1);
            float yOffset = (plantVisuals[0].GetComponent<SpriteRenderer>().sprite.bounds.extents.y * scale);
            plantVisuals[0].gameObject.transform.parent.gameObject.transform.localPosition = new Vector3(-tileOffset, yOffset, -tileOffset);
            //plantVisuals[0].GetComponentInParent<GameObject>().transform.localPosition = new Vector3(-tileOffset, yOffset, -tileOffset);
        }
    }

    private void ResolveStats()
    {

        bool waterLow = storedWater.current < storedWater.max / 4;
        bool sunlightLow = storedSunlight.current < storedSunlight.max / 4;

        if (waterLow && !sunlightLow)
        {
            // Water is low but sunlight is not, prioritize water UI
            waterUI.SetActive(true);
            sunlightUI.SetActive(false);
        }
        else if (!waterLow && sunlightLow && !weatherState.dayTime)
        {
            // Sunlight is low but water is not, prioritize sunlight UI
            waterUI.SetActive(false);
            sunlightUI.SetActive(true);
        }
        else if (waterLow && sunlightLow)
        {
            // Both water and sunlight are low, prioritize water UI
            waterUI.SetActive(true);
            sunlightUI.SetActive(false);
        }
        else
        {
            // Both water and sunlight are not low, deactivate both UIs
            waterUI.SetActive(false);
            sunlightUI.SetActive(false);
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

    private IEnumerator HandleAcidRain()
    {
        while (true)
        {
            yield return growthRate;

            if (weatherState.GetSkyState() == WeatherState.SkyState.RAINY)
            {
                if (weatherState.GetAcidRainState() == WeatherState.AcidRainState.LIGHT)
                {
                    TakeDamage(10);
                }
                else if (weatherState.GetAcidRainState() == WeatherState.AcidRainState.HEAVY)
                {
                    TakeDamage(15);
                }
            }
        }

    }

    public override void TakeDamage(int damageTaken)
    {
        
        HandleHealthChanges(-damageTaken);

        if (health.current <= 0)
        {
            isDying = true;
            PlantDies();
        }
    }

    public void PlantDies()
    {
        if (stats.plantName == "Tree")
        {
            if (currentPlantStage == PlantStats.PlantStage.JUVENILE)
            {
                logs = Instantiate(stats.treeLogPrefab, tilePlantedOn.transform);
                logs.transform.localPosition.Set(logs.transform.localPosition.x, logs.transform.localPosition.y + 1f, logs.transform.localPosition.z);
                logs.GetComponent<PickupObject>().SetItemQuantity(1);
                logs.GetComponent<PickupObject>().SetInventory(inventory);
            }
            else if (currentPlantStage == PlantStats.PlantStage.MATURE)
            {
                logs = Instantiate(stats.treeLogPrefab, tilePlantedOn.transform);
                logs.transform.localPosition.Set(logs.transform.localPosition.x, logs.transform.localPosition.y + 1f, logs.transform.localPosition.z);
                logs.GetComponent<PickupObject>().SetItemQuantity(3);
                logs.GetComponent<PickupObject>().SetInventory(inventory);
            }
        }
        if (currentPlantStage == PlantStats.PlantStage.SEEDLING || currentPlantStage == PlantStats.PlantStage.SPROUT)
        {
            seed = Instantiate(stats.seedPrefab, tilePlantedOn.transform);
            seed.transform.localPosition.Set(seed.transform.localPosition.x, seed.transform.localPosition.y + 1f, seed.transform.localPosition.z);
            seed.GetComponent<PickupObject>().SetItemQuantity(1);
            seed.GetComponent<PickupObject>().SetInventory(inventory);
        }
        tilePlantedOn.tileHasBuild = false;
        tilePlantedOn.placedObject = null;
        Destroy(this.gameObject);
    }

    public void SetInventory(Inventory newInventory)
    {
        inventory = newInventory;
    }

    public void SetSmothered(bool isSmothered)
    {
        this.isSmothered = isSmothered;
    }

    public bool GetIsSmothered()
    {
        return isSmothered;
    }

    public bool GetIsDying()
    {
        return isDying;
    }
}
