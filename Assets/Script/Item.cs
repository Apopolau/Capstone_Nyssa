using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item", fileName = "Item")]
public class Item : ScriptableObject
{
    [SerializeField] string itemName;
    [SerializeField] int itemID;
    [SerializeField] Sprite image;

}
