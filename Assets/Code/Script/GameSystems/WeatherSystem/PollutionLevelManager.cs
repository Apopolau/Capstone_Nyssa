using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PollutionLevelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObjectRuntimeSet buildSet;
    [SerializeField] GameObjectRuntimeSet monsterSet;
    [SerializeField] GameObject rainParticleSystem;
    private ParticleSystemRenderer rainRenderer;
    Material rainMaterial;

    [Header("Pollution Icon")]
    [SerializeField] private Image earthIcon;
    [SerializeField] private Sprite happyLevelImage;
    [SerializeField] private Sprite sadLevelImage;
    [SerializeField] private Sprite desperateLevelImage;

    [Header("Bar References")]
    [SerializeField] private Image pollutionLevelBar;

    [Header("Stats")]
    [SerializeField] private int basePollutionLevel;
    [SerializeField] private int maxPollution;
    [SerializeField] private float acidRainThreshold;
    [SerializeField] private float acidRainHardThreshold;
    [SerializeField] private Color c_cleanRain;
    [SerializeField] private Color c_lightAcid;
    [SerializeField] private Color c_heavyAcid;

    WaitForSeconds checkDelay = new WaitForSeconds(0.5f);

    [SerializeField] WeatherState weatherState;

    //Haven't decided if acid rain should be a bool or enum for multiple stages
    
    // Start is called before the first frame update
    void Start()
    {
        rainRenderer = rainParticleSystem.GetComponent<ParticleSystemRenderer>();
        rainMaterial = rainRenderer.material;
        CalculatePollutionLevel();
        StartCoroutine(PollutionCalcRoutine());
        
    }

    private IEnumerator PollutionCalcRoutine()
    {
        while (true)
        {
            yield return checkDelay;
            CalculatePollutionLevel();
        }
        
    }

    private void CalculatePollutionLevel()
    {
        float pollutionCounteractment = CalculatePlantContribution();
        float monsterPollution = CalculateMonsterContribution();
        float pollutionLevel = basePollutionLevel + monsterPollution - pollutionCounteractment;
        float pollutionPercent = (float)pollutionLevel / maxPollution;

        GetComponent<HUDManager>().UpdatePollutionDisplay(pollutionPercent);
        
        //Change colour of rain to reflect acid levels
        //Add a change of sound effect here when the time arises
        if (weatherState.GetAcidRainState() == WeatherState.AcidRainState.NONE)
        {
            rainMaterial.color = c_cleanRain;
        }
        else if (weatherState.GetAcidRainState() == WeatherState.AcidRainState.LIGHT)
        {
            rainMaterial.color = c_lightAcid;
        }
        else if(weatherState.GetAcidRainState() == WeatherState.AcidRainState.HEAVY)
        {
            rainMaterial.color = c_heavyAcid;
        }
        

    }

    private float CalculatePlantContribution()
    {
        List<Plant> plants = new List<Plant>();
        float totalPlantContribution = 0;
        foreach(GameObject build in buildSet.Items)
        {
            if (build.GetComponent<Plant>())
            {
                plants.Add(build.GetComponent<Plant>());
            }
        }
        foreach(Plant plant in plants)
        {
            totalPlantContribution += plant.currentPollutionContribution ;
        }

        return totalPlantContribution;
    }

    private float CalculateMonsterContribution()
    {
        List<Enemy> enemies = new List<Enemy>();
        float totalMonsterContribution = 0;
        foreach (GameObject enemy in monsterSet.Items)
        {
            if (enemy.GetComponent<Enemy>())
            {
                enemies.Add(enemy.GetComponent<Enemy>());
            }
        }
        foreach (Enemy enemy in enemies)
        {
            totalMonsterContribution -= enemy.GetEnemyStats().airPollutionEffect;
        }

        return totalMonsterContribution;
    }

    //Should be called before a wave of monsters attacks
    public void StartMonsterSurge()
    {
        
    }
}
