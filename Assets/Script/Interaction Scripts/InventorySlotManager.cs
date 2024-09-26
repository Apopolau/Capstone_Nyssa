using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotManager : MonoBehaviour
{
    [SerializeField] private List<ItemSlot> itemSlots;
    [SerializeField] private Inventory inventory;

    private void Awake()
    {
        //inventory.AddItemSlots(itemSlots);
    }

    public void SetInventory(Inventory incInventory)
    {
        inventory = incInventory;
        inventory.AddItemSlots(itemSlots);
    }
}
