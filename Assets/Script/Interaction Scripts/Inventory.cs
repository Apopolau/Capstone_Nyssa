using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New Inventory", menuName = "ManagerObject/Inventory")]
public class Inventory : ScriptableObject
{
    int inventorySize = 4;
    [SerializeField] public List<Item> items;
    [SerializeField] public List<ItemSlot> itemSlots;

    public PlantingUIIndicator plantingUI;

    private void OnEnable()
    {
        RefreshUI();
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
        if (IsFull())
        {
            return false;
        }

        // Check if the item already exists in the inventory.
        foreach (var existingItem in items)
        {
            if (existingItem.stats.ItemName == item.stats.ItemName)
            {
                existingItem.quantity += quantity; // Increase the quantity.
                RefreshUI();
                return true;
            }
        }

        // If the item is not in the inventory, add a new one.
        Item itemToAdd = new Item(item);
        items.Add(itemToAdd);

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
            if (item.stats.ItemName == itemName && item.quantity >= quantity)
            {
                itemToRemove = item;
            }
        }
        
        if (itemToRemove != null)
        {
            //Debug.Log($"Removing item {itemToRemove.stats.ItemName} based on key press");
            RemoveItem(itemToRemove, quantity);

            // Add a log message to check the current inventory after removal
            foreach (var item in items)
            {
                //Debug.Log($"Item: {item.stats.ItemName}, Quantity: {item.quantity}");
            }
        }
        else
        {
            //Debug.LogWarning($"You do not have the required item in the inventory.");
        }
    }

    //This is called in RemoveItemByName, so you should generally call that instead
    public bool RemoveItem(Item item, int quantity)
    {
        int i = 0;
        foreach (var existingItem in items)
        {
            if (existingItem.stats.ItemName == item.stats.ItemName)
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
            uiIndicator.UpdateQuantityText();
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
                
                if (itemSlots[i].Item == null)
                {
                    itemSlots[i].SetItem(items[i]);
                }
                
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

        // Display quantity changes in the console log.
        foreach (Item item in items)
        {
            //Debug.Log($"Item: {item.stats.ItemName}, Quantity: {item.quantity}");
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
        Debug.Log(this.name);
        return items.Count >= itemSlots.Count;
    }

    //Check how many of a particular item they have
    public int GetQuantityByItemType(string itemType)
    {
       // Debug.Log($"GetQuantityByItemType() called for itemType: {itemType}");

        int currentQuantity = 0;
        foreach (var item in items)
        {
            if (item.stats.ItemName == itemType)
            {
                currentQuantity += item.quantity;
                //Debug.Log($"Item type: {itemType}, Quantity: {quantity}");
            }
        }
        return currentQuantity;
    }
    
    //Has at least a certain amount of items
    public bool HasEnoughItems(string itemName, int quantity)
    {

        foreach (var item in items)
        {
            if (item.stats.ItemName == itemName && item.quantity >= quantity)
            {
                return true;
            }
        }
        return false;
    }

}
