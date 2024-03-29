using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    [SerializeField] public ItemStats stats;
    [SerializeField] public int quantity;
    
    public Item()
    {

    }

    //Use this to make an inventory item
    public Item(Item oldItem)
    {
        stats = oldItem.stats;
        stats.Icon = oldItem.stats.Icon;
        stats.name = oldItem.stats.name;
        quantity = oldItem.quantity;
    }

    //Use this to make an item prefab item
    public Item(ItemStats startStats, int startQuantity)
    {
        stats = startStats;
        quantity = startQuantity;
    }
}
