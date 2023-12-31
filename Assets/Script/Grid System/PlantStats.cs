using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Plant Stats", fileName = "PlantStats")]
public class PlantStats : ScriptableObject
{
    [SerializeField] int maxHealth;
    [SerializeField] Sprite image;
    [SerializeField] int airPollutionBonus;
    [SerializeField] GameObject cost;
}
