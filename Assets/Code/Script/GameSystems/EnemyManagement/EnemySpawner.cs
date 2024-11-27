using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private LevelManagerObject levelManager;
    [SerializeField] private WeatherState weatherState;
    [SerializeField] private EnemyInvadingPath enemyInvadingPath;
    [SerializeField] private GameObject plasticBagMonsterPrefab;
    //[SerializeField] private GameObject enemyOilInvaderPrefab;
    //[SerializeField] private GameObject smogMonsterInvaderPrefab;
    public GameObjectRuntimeSet enemySet;
    GameObject currSpawnedEnemy;

    //Match this to the path you want the monsters to take
    [SerializeField] private int spawnerIndex;
    [SerializeField] SpawnerData spawnerData;
    [SerializeField] private int maxEnemies;

    //private bool startedSpawns;
    private bool spawnsOn = true;
    private bool dayTime = true;

    //must be equal to a total of 10
    //private int plasticProbability=6;

    //Range is minimum or maximum time between waves/spawns
    [SerializeField, Range(15, 60)] private float spawnInterval;

    void Start()
    {
        if(!spawnerData.GetStartsOn())
            spawnsOn = false;

        if (spawnsOn)
        {
            StartCoroutine(spawnEnemy());
        }
            
    }

    private void Update()
    {
        CheckTimeOfDay();
        //SetSpawns();
    }

    //Make sure the inside if statements don't somehow get called repeatedly
    private void CheckTimeOfDay()
    {
        if (spawnsOn)
        {
            if (weatherState.dayTime)
            {
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
                currSpawnedEnemy = spawnerData.GetMonsterSpawn();
            }
        }
    }

    private IEnumerator spawnEnemy() 
    {

        while (true)
        {
            SetSpawns();
            //if daynight cycle.nights passed is bigger  that 2 interval decrease
            //yield return new WaitForSeconds(interval);
            //GameObject newEnemy = Instantiate(currSpawnedEnemy, this.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);

            if(enemySet.Items.Count < maxEnemies)
            {
                Instantiate(currSpawnedEnemy, this.transform);
                currSpawnedEnemy.GetComponent<Enemy>().SetInvasionPath(enemyInvadingPath.GetPath(spawnerIndex));

                Debug.Log(currSpawnedEnemy.GetComponent<KidnappingEnemy>());

                if (currSpawnedEnemy.GetComponent<KidnappingEnemy>())
                    currSpawnedEnemy.GetComponent<KidnappingEnemy>().SetEscapeWaypoint(enemyInvadingPath.GetEscapePoint(spawnerIndex));
            }
                
        }
    }

    /*
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
    */

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
