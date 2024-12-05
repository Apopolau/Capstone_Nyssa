using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Data Type/Stats/Enemy Stats" )]

public class EnemyStats : ScriptableObject
{
    public enum enemyTypes { OilMonster, PlasticBag, Smog };
    [Header("Basic info")]
    public enemyTypes enemyType;
    [SerializeField] public int maxHealth;
    [SerializeField] public float attackRate;
    [SerializeField] public int maxDamage;
    [SerializeField] public int airPollutionEffect;
    [SerializeField] public int plantEffect;
    [SerializeField] public bool canKidnap;
    [SerializeField] public bool canSmother;
    [SerializeField] public GameObject cost;
    [SerializeField] public float attackAnimTime;
    [SerializeField] public float takeHitAnimTime;
    [SerializeField] public float deathAnimTime;

    [SerializeField] public float sightRange;
    [SerializeField] public float attackRange;
    [SerializeField] public float kidnapRange;

}