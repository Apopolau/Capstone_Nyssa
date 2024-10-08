using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasticBagEnemySpawner : MonoBehaviour
{
    
    [SerializeField] private GameObject plasticBagMonsterPrefab;
    private bool startedSpawns;
    private bool spawnsOn;
    GameObject currSpawnedEnemy;
    [SerializeField] private float spawnInterval;
    public GameObjectRuntimeSet enemySet;

    void Start()
    {
        currSpawnedEnemy = plasticBagMonsterPrefab;
        StartCoroutine(spawnEnemy(currSpawnedEnemy, spawnInterval));

    }

    private void Update()
    {
      
    }

    //Make sure the inside if statements don't somehow get called repeatedly
    private void CheckSpawnOn()
    {



        if (spawnsOn)
        {
         
                spawnsOn = false;
                StopAllCoroutines();
     
        }
        else if (!spawnsOn)
        {
            
                spawnsOn = true;

                StartCoroutine(spawnEnemy(currSpawnedEnemy, spawnInterval));
            
        }
    }

    private IEnumerator spawnEnemy(GameObject enemy, float interval)
    {
        while (true)
        {


            //if daynight cycle.nights passed is bigger  that 2 interval decrease
            yield return new WaitForSeconds(interval);

            if (enemySet.Items.Count < 5)
            { GameObject newEnemy = Instantiate(currSpawnedEnemy, this.transform.position, Quaternion.identity); }
        }
    }

   
    //Check the Day 
    //If day is furth alonge e.g. its day 3 
    ///////make spawn interval lesser
    ///////make it so that smog monsters and oil monsters also appear
    ///

}
