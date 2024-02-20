using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyOilPrefab;
    // Start is called before the first frame update

    private float enemyOilSpawnInterval = 5.5f;
    void Start()
    {
        StartCoroutine(spawnEnemy(enemyOilPrefab, enemyOilSpawnInterval));
        
    }

   private IEnumerator spawnEnemy(GameObject enemy, float interval) 
   {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(0f, 0f, 0f), Quaternion.identity);
        StartCoroutine(spawnEnemy(enemy, interval));
    }
}
