using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PollutionLevelManager : MonoBehaviour
{
    [SerializeField] GameObjectRuntimeSet buildSet;
    [SerializeField] GameObjectRuntimeSet monsterSet;

    [SerializeField] private Image pollutionLevelBar;
    [SerializeField] private int basePollutionLevel;
    [SerializeField] private int maxPollution;
    [SerializeField] private float acidRainThreshold;

    WaitForSeconds checkDelay = new WaitForSeconds(1);

    //Haven't decided if acid rain should be a bool or enum for multiple stages
    
    // Start is called before the first frame update
    void Start()
    {
        CalculatePollutionLevel();
        StartCoroutine(PollutionCalcRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
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
        pollutionLevelBar.fillAmount = pollutionPercent;
        /*
        if (pollutionPercent >= acidRainThreshold)
        {

        }
        */
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
            totalMonsterContribution -= enemy.enemyStats.airPollutionEffect;
        }

        return totalMonsterContribution;
    }

    //Should be called before a wave of monsters attacks
    public void StartMonsterSurge()
    {
        
    }
}
