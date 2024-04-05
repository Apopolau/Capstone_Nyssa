using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private WeatherState weatherState;
    [SerializeField] private GameObject plasticBagMonsterPrefab;
   // [SerializeField] private GameObject enemyOilInvaderPrefab;
    [SerializeField] private GameObject smogMonsterInvaderPrefab;
    GameObject currSpawnedEnemy;

    private bool startedSpawns;
    private bool spawnsOn;

    //must be equal to a total of 10
    private int plasticProbability=6;

    [SerializeField] private float spawnInterval;

    void Start()
    {
        //StartCoroutine(spawnEnemy(currSpawnedEnemy, spawnInterval));
        
    }

    private void Update()
    {
        int index = Random.Range(0, 10);
        if (index < 6)
        {
            currSpawnedEnemy = plasticBagMonsterPrefab;

        }
        else
        {
            currSpawnedEnemy = smogMonsterInvaderPrefab;
        }
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

         
            //if daynight cycle.nights passed is bigger  that 2 interval decrease
            yield return new WaitForSeconds(interval);
            GameObject newEnemy = Instantiate(currSpawnedEnemy, this.transform.position, Quaternion.identity);
        }
    }

    private void getRandomizedEnemy()
    {
        int index = Random.Range(0, 10);
        if (index < plasticProbability)
        {
            currSpawnedEnemy = plasticBagMonsterPrefab;

        }
        else         
        {
            currSpawnedEnemy = smogMonsterInvaderPrefab;
        }
        
    }

    //Check the Day 
    //If day is furth alonge e.g. its day 3 
    ///////make spawn interval lesser
    ///////make it so that smog monsters and oil monsters also appear
    ///

}
