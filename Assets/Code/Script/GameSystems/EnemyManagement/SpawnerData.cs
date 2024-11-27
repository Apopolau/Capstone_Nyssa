using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataType/Spawner/SpawnerData", fileName = "SpawnerData")]
public class SpawnerData : ScriptableObject
{
    [SerializeField] int spawnIndex;
    [SerializeField] private List<SpawnData> spawnData = new List<SpawnData>();

    [SerializeField] bool startsOn;

    public List<SpawnData> GetInvadingEnemyList()
    {
        return spawnData;
    }

    public int GetSpawnIndex()
    {
        return spawnIndex;
    }

    public bool GetStartsOn()
    {
        return startsOn;
    }

    public GameObject GetMonsterSpawn()
    {
        int index = Random.Range(0, 10);
        foreach(SpawnData spawn in spawnData)
        {
            if(index >= spawn.GetLowerSpawnChance() && index <= spawn.GetUpperSpawnChance())
            {
                return spawn.GetEnemyType();
            }
        }
        Debug.Log("Didn't find a monster matching that index");
        return null;
    }
}
