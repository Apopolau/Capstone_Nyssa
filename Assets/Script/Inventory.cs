using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Inventory : MonoBehaviour
{
    [SerializeField] List<Item> items;
    
    [SerializeField] Transform itemsParent;
    [SerializeField] ItemSlot[] itemSlots;
    [SerializeField] KeyCode removeKeySeed = KeyCode.O; // Change this to the Dpad
    [SerializeField] KeyCode removeKeyTreeLog = KeyCode.L; // Change this to the Dpad

    private void OnValidate()
    {
        if (itemsParent != null)
            itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>();

        RefreshUI();
    }

    private void Update()
    {
        // Check if the remove key for Seed is pressed
        if (Input.GetKeyDown(removeKeySeed))
        {
            RemoveItemByName("TreeSeed");
        }

        // Check if the remove key for TreeLog is pressed
        if (Input.GetKeyDown(removeKeyTreeLog))
        {
            RemoveItemByName("TreeLog");
        }

        // Add more conditions for other keys/items as needed
    }

    private void RemoveItemByName(string itemName)
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

   private void RefreshUI()
{
    int i = 0;
    for (; i < items.Count && i < itemSlots.Length; i++)
    {
        if (itemSlots[i] != null)
        {
            itemSlots[i].Item = items[i];
            itemSlots[i].UpdateQuantityText();
        }
    }

    for (; i < itemSlots.Length; i++)
    {
        if (itemSlots[i] != null)
        {
            itemSlots[i].Item = null;
        }
    }

    // Display quantity changes in the console log.
    foreach (var item in items)
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


public bool IsFull(){
        return items.Count >= itemSlots.Length;
    }



}
