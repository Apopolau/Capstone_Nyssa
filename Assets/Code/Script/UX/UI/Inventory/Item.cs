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
        stats.SetIcon(oldItem.stats.GetIcon());
        stats.SetItemNameEN(oldItem.stats.GetItemNameEN());
        stats.SetItemNameFR(oldItem.stats.GetItemNameFR());
        quantity = oldItem.quantity;
    }

    //Use this to make an item prefab item
    public Item(ItemStats startStats, int startQuantity)
    {
        stats = startStats;
        quantity = startQuantity;
    }
}
