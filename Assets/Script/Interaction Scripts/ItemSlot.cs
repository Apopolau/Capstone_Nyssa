using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI itemNameText;  // New field for displaying item name.
    [SerializeField] TextMeshProUGUI quantityText;   // Field for displaying quantity.

    [SerializeField] private Item _item;

    public Item Item
    {
        get { return _item; }
        set
        {
            _item = value;
            if (_item == null)
            {
                image.enabled = false;
                itemNameText.text = "";
                quantityText.text = "";
            }
            else
            {
                image.sprite = _item.stats.Icon;
                image.enabled = true;
                UpdateItemNameText();
                UpdateQuantityText();
            }
        }
    }

    private void OnValidate()
    {
        
    }

    private void Awake()
    {
        if (image == null)
            image = GetComponent<Image>();

        // Ensure that itemNameText and quantityText are set via the Unity Editor.
        if (itemNameText == null)
            itemNameText = GetComponentInChildren<TextMeshProUGUI>(); // Use TextMeshProUGUI here

        if (quantityText == null)
            quantityText = GetComponentInChildren<TextMeshProUGUI>(); // Use TextMeshProUGUI here

        inventory.AddItemSlot(this);
    }

    public void SetItem(Item newItem)
    {
        _item = newItem;
    }

    public void SetItemDisplay()
    {
        
        if (_item == null)
        {
            image.enabled = false;
            itemNameText.text = "";
            quantityText.text = "";
        }
        else
        {
            image.sprite = _item.stats.Icon;
            image.enabled = true;
            UpdateItemNameText();
            UpdateQuantityText();
        }
    }

    public void UpdateItemNameText()
    {
        // Update the item name text with the item's name.
        if (_item != null)
        {
            itemNameText.text = _item.stats.ItemName;
        }
    }

    public void UpdateQuantityText()
    {
        // Update the quantity text with the item's quantity.
        if (_item != null)
        {
            quantityText.text = $"{_item.quantity}";
        }
    }
}
