using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


[CreateAssetMenu(menuName = "Stats/Power Stats", fileName = "PowerStats")]


public class PowerStats : ScriptableObject
{
    [Header("Basic info")]
    [SerializeField] public string powerType;
    [SerializeField] public int maxDamage;
    [SerializeField] public int minDamage;
    [SerializeField] public int airPollutionEffect;
    /// <summary>
    /// try without serialized
    /// </summary>
    [SerializeField] public enum surfaceAreaEffect { MIN, MED, MAX };
    [SerializeField] public bool isWeakness;
    [SerializeField] public bool effectsTiles;
    [SerializeField] public bool isEnabled;
    public float speed;
    public float rechargeFloatTimer;
    public WaitForSeconds rechargeTimer;
    [SerializeField] public VisualEffect visualDisplay;
    [SerializeField] public GameObject visualGameObj;
    [SerializeField] public GameObject powerDropPrefab;

}