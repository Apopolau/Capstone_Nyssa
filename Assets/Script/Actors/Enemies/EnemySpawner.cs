using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private WeatherState weatherState;
    [SerializeField] private GameObject plasticBagMonsterPrefab;
    [SerializeField] private GameObject enemyOilInvaderPrefab;
    [SerializeField] private GameObject smogMonsterInvaderPrefab;
    GameObject currSpawnedEnemy;

    private bool startedSpawns;
    private bool spawnsOn;

    [SerializeField] private float spawnInterval;

    void Start()
    {
        //StartCoroutine(spawnEnemy(currSpawnedEnemy, spawnInterval));
        
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
           if (!weatherState.dayTime)
            {
                spawnsOn = true;
                StartCoroutine(spawnEnemy(currSpawnedEnemy, spawnInterval));
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


    //Check the Day 
    //If day is furth alonge e.g. its day 3 
    ///////make spawn interval lesser
    ///////make it so that smog monsters and oil monsters also appear
    ///

}
