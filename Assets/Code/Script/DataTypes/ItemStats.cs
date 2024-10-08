using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Stats", menuName = "Data Type/Stats/Item Stats")]
public class ItemStats : ScriptableObject
{
    [SerializeField] public string ItemName;
    [SerializeField] public Sprite Icon;
    //public int Quantity;
    //public GameObject itemPrefab;
    
}
