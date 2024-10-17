using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private LevelManagerObject levelManager;
    [SerializeField] private WeatherState weatherState;
    [SerializeField] private EnemyInvadingPath enemyInvadingPath;
    [SerializeField] private GameObject plasticBagMonsterPrefab;
    [SerializeField] private GameObject enemyOilInvaderPrefab;
    [SerializeField] private GameObject smogMonsterInvaderPrefab;
    public GameObjectRuntimeSet enemySet;
    GameObject currSpawnedEnemy;

    int level = 0;

    public bool isFromSawMill;

    private bool startedSpawns;
    private bool spawnsOn = true;
    private bool dayTime = true;

    //must be equal to a total of 10
    private int plasticProbability=6;

    //Range is minimum or maximum time between waves/spawns
    [SerializeField, Range(15, 60)] private float spawnInterval;

    void Start()
    {
        //StartCoroutine(spawnEnemy(currSpawnedEnemy, spawnInterval));
        level = levelManager.currentLevel;
        if(!isFromSawMill)
            spawnsOn = false;

        if (spawnsOn)
        {
            SetSpawns();
            StartCoroutine(spawnEnemy());
        }
            
    }

    private void Update()
    {
        /*
        int index = Random.Range(0, 10);
        if (isFromSawMill)
        {
            if (index < 6)
            {
                currSpawnedEnemy = plasticBagMonsterPrefab;

            }
            else
            {
                currSpawnedEnemy = smogMonsterInvaderPrefab;
            }
        }
        else
        {
            if (index < 2)
            {
                currSpawnedEnemy = plasticBagMonsterPrefab;

            }
            else if (2 <= index  && index < 6)
            {
                currSpawnedEnemy = smogMonsterInvaderPrefab;
            }
            else if (6 <= index && index < 10)
            {
                currSpawnedEnemy = enemyOilInvaderPrefab;
            }
        }
        */
        CheckTimeOfDay();
        SetSpawns();
    }

    //Make sure the inside if statements don't somehow get called repeatedly
    private void CheckTimeOfDay()
    {
        if (spawnsOn)
        {
            if (weatherState.dayTime)
            {
                //spawnsOn = false;
                //StopAllCoroutines();
                dayTime = true;
            }
            else
            {
                dayTime = false;
            }
        }
    }

    private void SetSpawns()
    {
        if (spawnsOn)
        {
            if (dayTime)
            {
                currSpawnedEnemy = plasticBagMonsterPrefab;
            }
            if (!dayTime)
            {
                if( level == 2)
                {
                    if (isFromSawMill)
                    {
                        currSpawnedEnemy = enemyOilInvaderPrefab;
                    }
                    else
                    {
                        int index = Random.Range(0, 10);
                        if (index < 7)
                        {
                            currSpawnedEnemy = enemyOilInvaderPrefab;
                        }
                        else if (index > 10)
                        {
                            currSpawnedEnemy = smogMonsterInvaderPrefab;
                        }
                    }
                }
                

            }
        }
    }

    private IEnumerator spawnEnemy() 
    {
        while (true)
        {
            //if daynight cycle.nights passed is bigger  that 2 interval decrease
            //yield return new WaitForSeconds(interval);
            //GameObject newEnemy = Instantiate(currSpawnedEnemy, this.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);

            if(enemySet.Items.Count < 10)
            {
                Instantiate(currSpawnedEnemy, this.transform);
                currSpawnedEnemy.GetComponent<Enemy>().SetInvaderEnemyRoutes(enemyInvadingPath);
            }
                
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

    public void ToggleSpawns(bool on)
    {
        if (on)
        {
            spawnsOn = true;
            StartCoroutine(spawnEnemy());
        }
        if (!on)
        {
            spawnsOn = false;
            //StopCoroutine(spawnEnemy());
            StopAllCoroutines();
        }
    }

    public bool GetSpawnsOn()
    {
        return spawnsOn;
    }

    //Check the Day 
    //If day is furth alonge e.g. its day 3 
    ///////make spawn interval lesser
    ///////make it so that smog monsters and oil monsters also appear
    ///

}
