using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Power Stats", fileName = "PowerStats")]


public class PowerStats : ScriptableObject
{
    [Header("Basic info")]
    [SerializeField] public string powerType;
    [SerializeField] public int maxDamage;
    [SerializeField] public int minDamage;
    [SerializeField] public int airPollutionEffect;
    [SerializeField] public enum surfaceAreaEffect { MIN, MED, MAX };
    [SerializeField] public bool isWeakness;
    [SerializeField] public bool effectsTiles;
    [SerializeField] public float rechargeTimer;
    [SerializeField] public GameObject visualDisplay;


}