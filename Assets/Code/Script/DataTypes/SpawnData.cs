using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataType/Spawner/SpawnData", fileName = "SpawnData")]
public class SpawnData : ScriptableObject
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private float minSpawnInterval;
    [SerializeField] private float maxSpawnInterval;

    public GameObject GetEnemyType()
    {
        return enemy;
    }

    public void SetLowerSpawnChance(float chance)
    {
        minSpawnInterval = Mathf.Clamp(chance, 0, 10);
    }

    public void SetUpperSpawnChance(float chance)
    {
        maxSpawnInterval = Mathf.Clamp(chance, 0, 10);
    }

    public float GetLowerSpawnChance()
    {
        return minSpawnInterval;
    }

    public float GetUpperSpawnChance()
    {
        return maxSpawnInterval;
    }
}
