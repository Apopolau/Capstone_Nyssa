using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    [SerializeField] List<Item> items;
    
    [SerializeField] Transform itemsParent;
    [SerializeField] ItemSlot[] itemSlots;

    private void OnValidate()
    {
        if (itemsParent != null)
            itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>();

        RefreshUI();
    }

   private void RefreshUI()
   {
        int i = 0;
        for (; i < items.Count && i < itemSlots.Length; i++)
        {
            if (itemSlots[i] != null)
            {
                itemSlots[i].Item = items[i];
            }
        }

        for (; i < itemSlots.Length; i++)
        {
            if (itemSlots[i] != null)
            {
                itemSlots[i].Item = null;
            }
        }
    }   

    public bool AddItem (Item item) 
    {
        if (IsFull())
           { return false;}

        items.Add(item);
        Debug.Log("Item added to inventory: ");
        RefreshUI();
        return true;
            
    }

    public bool RemoveItem(Item item)
    {
        if(items.Remove(item))
        {
            RefreshUI();
            return true;
        }
        return false;
    }
    public bool IsFull(){
        return items.Count >= itemSlots.Length;
    }


}
