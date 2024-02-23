using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory")]
public class Inventory : ScriptableObject
{
    int inventorySize = 4;
    [SerializeField] public List<Item> items;

    //[SerializeField] Transform itemsParent;
    [SerializeField] public List<ItemSlot> itemSlots;
    //[SerializeField] KeyCode removeKeySeed = KeyCode.O; // Change this to the Dpad
    //[SerializeField] KeyCode removeKeyTreeLog = KeyCode.L; // Change this to the Dpad

    private void OnValidate()
    {
        
    }

    public void Initialize()
    {
        if (itemSlots != null)
        {
            items.Clear();
            itemSlots.Clear();
            RefreshUI();
        }
    }

    private void OnEnable()
    {

    }

    public void AddItemSlot(ItemSlot slot)
    {
        itemSlots.Insert(0, slot);
    }

    public void RemoveItemByName(string itemName)
    {
        // Find the item with the specified name and remove it
        Item itemToRemove = items.Find(item => item.ItemName == itemName);
        if (itemToRemove != null)
        {
            Debug.Log($"Removing item {itemToRemove.ItemName} based on key press");
            RemoveItem(itemToRemove);

            // Add a log message to check the current inventory after removal
            Debug.Log("Current inventory:");
            foreach (var item in items)
            {
                Debug.Log($"Item: {item.ItemName}, Quantity: {item.Quantity}");
            }
        }
        else
        {
            Debug.LogWarning($"You do not have the required item in the inventory.");
        }
    }

    public void RemoveItemByName(string itemName, int quantity)
    {
        // Find the item with the specified name and remove it
        Item itemToRemove = items.Find(item => item.ItemName == itemName);
        if (itemToRemove != null)
        {
            Debug.Log($"Removing item {itemToRemove.ItemName} based on key press");
            RemoveItem(itemToRemove, quantity);

            // Add a log message to check the current inventory after removal
            Debug.Log("Current inventory:");
            foreach (var item in items)
            {
                Debug.Log($"Item: {item.ItemName}, Quantity: {item.Quantity}");
            }
        }
        else
        {
            Debug.LogWarning($"You do not have the required item in the inventory.");
        }
    }

    private void RefreshUI()
    {
        int i = 0;
        for (; i < items.Count && i < itemSlots.Count; i++)
        {
            if (itemSlots[i] != null)
            {
                itemSlots[i].Item = items[i];
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
            Debug.Log($"Item: {item.ItemName}, Quantity: {item.Quantity}");
        }
    }

    public bool AddItem(Item item, int quantity = 1)
    {
        if (IsFull())
        {
            return false;
        }

        // Check if the item already exists in the inventory.
        foreach (var existingItem in items)
        {
            if (existingItem.ItemName == item.ItemName)
            {
                existingItem.Quantity += quantity; // Increase the quantity.
                RefreshUI();
                return true;
            }
        }

        // If the item is not in the inventory, add a new one.
        Item newItem = Instantiate(item);
        newItem.Quantity = quantity;
        items.Add(newItem);

        RefreshUI();
        return true;
    }


    public bool RemoveItem(Item item, int quantity = 1)
    {
        foreach (var existingItem in items)
        {
            if (existingItem.ItemName == item.ItemName)
            {
                existingItem.Quantity -= quantity;

                // Remove the item from the list if its quantity is zero or less.
                if (existingItem.Quantity <= 0)
                {
                    items.Remove(existingItem);
                }

                RefreshUI();
                return true;
            }
        }
        return false;
    }


    public bool IsFull()
    {
        return items.Count >= itemSlots.Count;
    }

    public bool HasTypeSeed(string itemName)
    {
        
        foreach(var item in items)
        {
            if(item.ItemName == itemName && item.Quantity > 0)
            {
                return true;
            }
        }
        return false;
    }

    public bool HasEnoughItems(string itemName, int quantity)
    {

        foreach (var item in items)
        {
            if (item.ItemName == itemName && item.Quantity >= quantity)
            {
                return true;
            }
        }
        return false;
    }

}
