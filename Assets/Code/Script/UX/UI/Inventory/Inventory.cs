using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Manager Object/Inventory")]
public class Inventory : ScriptableObject
{
    //int inventorySize = 4;
    [SerializeField] private UserSettingsManager userSettingsManager;
    [SerializeField] public List<Item> items;
    [SerializeField] public List<ItemSlot> itemSlots;

    public PlantingUIIndicator plantingUI;

    public event System.Action OnItemChange;

    private void OnEnable()
    {
        //RefreshUI();
        ChangeLanguage(userSettingsManager.chosenLanguage);
    }

    private void OnDisable()
    {
        if (itemSlots.Count != 0)
        {
            itemSlots.Clear();
        }
        if (items.Count != 0)
        {
            items.Clear();
        }
    }

    public bool AddItem(Item item, int quantity)
    {
        // Check if the item already exists in the inventory.
        foreach (var existingItem in items)
        {
            if (existingItem.stats.GetItemNameEN() == item.stats.GetItemNameEN())
            {
                existingItem.quantity += quantity; // Increase the quantity.
                if (OnItemChange != null)
                    OnItemChange();
                RefreshUI();
                return true;
            }
        }

        if (IsFull())
        {
            return false;
        }

        // If the item is not in the inventory, add a new one.
        Item itemToAdd = new Item(item);
        if(userSettingsManager.chosenLanguage == UserSettingsManager.GameLanguage.ENGLISH)
        {
            itemToAdd.stats.SetCurrentName(itemToAdd.stats.GetItemNameEN());
        }
        else
        {
            itemToAdd.stats.SetCurrentName(itemToAdd.stats.GetItemNameFR());
        }
        
        items.Add(itemToAdd);
        if (OnItemChange != null)
            OnItemChange();

        //item.stats.Icon = item.stats.Icon;
        RefreshUI();
        return true;
    }

    /// <summary>
    /// THIS
    /// NEEDS
    /// FIXED
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="quantity"></param>
    //Call this version if you want to remove a specific amount
    public void RemoveItemByName(string itemName, int quantity)
    {
        Item itemToRemove = null;
        // Find the item with the specified name and remove it
        foreach (Item item in items)
        {
            if (item.stats.GetItemNameEN() == itemName && item.quantity >= quantity)
            {
                itemToRemove = item;
            }
        }
        
        if (itemToRemove != null)
        {
            RemoveItem(itemToRemove, quantity);
        }
    }

    //This is called in RemoveItemByName, so you should generally call that instead
    public bool RemoveItem(Item item, int quantity)
    {
        int i = 0;
        foreach (var existingItem in items)
        {
            if (existingItem.stats.GetItemNameEN() == item.stats.GetItemNameEN())
            {
                existingItem.quantity -= quantity;

                // Remove the item from the list if its quantity is zero or less.
                if (existingItem.quantity <= 0)
                {
                    itemSlots[i].Item = null;
                    items.Remove(existingItem);
                }
                UpdateUIText(); // Call UpdateUIText() after item removal
                RefreshUI();
                if (OnItemChange != null)
                    OnItemChange();

                return true;
            }
            i++;
        }
        return false;
    }

    //Update the text on the ui to match current item quantities
    private void UpdateUIText()
    {
        // Find the PlantingUIIndicator script in the scene
        PlantingUIIndicator uiIndicator = FindObjectOfType<PlantingUIIndicator>();
        if (uiIndicator != null)
        {
            // Call the UpdateQuantityText() method
            uiIndicator.UpdateQuantityTextOnce();
        }
    }

    //Update display of items to show current item sprites
    private void RefreshUI()
    {
        int i = 0;
        for (; i < items.Count && i < itemSlots.Count; i++)
        {
            if (itemSlots[i] != null)
            {
                /*
                if (itemSlots[i].Item == null)
                {
                    itemSlots[i].SetItem(items[i]);
                }
                */
                itemSlots[i].SetItem(items[i]);

                itemSlots[i].SetItemDisplay();
                itemSlots[i].UpdateQuantityText();
            }
        }

        for (; i < itemSlots.Count; i++)
        {
            if (itemSlots[i] != null)
            {
                itemSlots[i].Item = null;
            }
        }
    }

    //Increase number of item slots when a player picks up a new type of item
    public void AddItemSlots(List<ItemSlot> slots)
    {
        itemSlots = slots;
    }

    public void RemoveItemFromItems(Item itemToRemove)
    {
        items.Remove(itemToRemove);
    }

    public void RemoveItemSlot(ItemSlot slot)
    {
        if (itemSlots.Contains(slot))
            itemSlots.Remove(slot);
    }

    //No more slots left
    public bool IsFull()
    {
        return items.Count >= itemSlots.Count;
    }

    //Check how many of a particular item they have
    public int GetQuantityByItemType(string itemType)
    {

        int currentQuantity = 0;
        foreach (var item in items)
        {
            if (item.stats.GetItemNameEN() == itemType)
            {
                currentQuantity += item.quantity;
            }
        }
        return currentQuantity;
    }
    
    //Has at least a certain amount of items
    public bool HasEnoughItems(string itemName, int quantity)
    {

        foreach (var item in items)
        {
            if (item.stats.GetItemNameEN() == itemName && item.quantity >= quantity)
            {
                return true;
            }
        }
        return false;
    }


    public void ChangeLanguage(UserSettingsManager.GameLanguage gameLanguage)
    {
        if(gameLanguage == UserSettingsManager.GameLanguage.ENGLISH)
        {
            foreach(var item in items)
            {
                item.stats.SetCurrentName(item.stats.GetItemNameEN());
            }
        }
        else
        {
            foreach (var item in items)
            {
                item.stats.SetCurrentName(item.stats.GetItemNameFR());
            }
        }
        RefreshUI();
    }
}
