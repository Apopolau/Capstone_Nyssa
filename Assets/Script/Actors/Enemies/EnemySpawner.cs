using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]private GameObject enemyOilPrefab;
    // Start is called before the first frame update

    [SerializeField] private float enemyOilSpawnInterval;
    void Start()
    {
        StartCoroutine(spawnEnemy(enemyOilPrefab, enemyOilSpawnInterval));
        
    }

   private IEnumerator spawnEnemy(GameObject enemy, float interval) 
   {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, this.transform.position, Quaternion.identity);
        StartCoroutine(spawnEnemy(enemy, interval));
    }
}
