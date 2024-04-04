using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private WeatherState weatherState;
    [SerializeField] private GameObject enemyOilPrefab;

    private bool startedSpawns;
    private bool spawnsOn;

    [SerializeField] private float enemyOilSpawnInterval;

    void Start()
    {
        StartCoroutine(spawnEnemy(enemyOilPrefab, enemyOilSpawnInterval));
        
    }

    private void Update()
    {
        CheckTimeOfDay();
    }

    //Make sure the inside if statements don't somehow get called repeatedly
    private void CheckTimeOfDay()
    {
        if (spawnsOn)
        {
            if (weatherState.dayTime)
            {
                spawnsOn = false;
                StopAllCoroutines();
            }
        }
        else if(!spawnsOn)
        {
          //  if (!weatherState.dayTime)
            {
                spawnsOn = true;
                StartCoroutine(spawnEnemy(enemyOilPrefab, enemyOilSpawnInterval));
            }
        }
    }

    private IEnumerator spawnEnemy(GameObject enemy, float interval) 
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            GameObject newEnemy = Instantiate(enemy, this.transform.position, Quaternion.identity);
        }
    }
}
