using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Stats/Enemy Stats", fileName = "EnemyStats")]


public class EnemyStats : ScriptableObject
{
    [Header("Basic info")]
    [SerializeField] public string enemyType;
    [SerializeField] public int maxHealth;
    [SerializeField] public float attackRate;
    [SerializeField] public int maxDamage;
    [SerializeField] public int airPollutionEffect;
    [SerializeField] public int plantEffect;
    [SerializeField] public bool canKidnap;
    [SerializeField] public bool canSmother;
  [SerializeField] public GameObject cost;
    

}