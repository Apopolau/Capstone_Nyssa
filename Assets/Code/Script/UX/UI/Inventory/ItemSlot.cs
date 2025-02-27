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
                image.sprite = _item.stats.GetIcon();
                image.enabled = true;
                UpdateItemNameText();
                UpdateQuantityText();
            }
        }
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

        _item = null;
    }

    private void OnDisable()
    {
        if (inventory.items.Contains(_item))
        {
            inventory.RemoveItemFromItems(_item);
        }
        _item = null;
        inventory.RemoveItemSlot(this);
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
            image.sprite = _item.stats.GetIcon();
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
            itemNameText.gameObject.SetActive(true);
            itemNameText.text = _item.stats.GetCurrentName();
        }
    }

    public void UpdateQuantityText()
    {
        // Update the quantity text with the item's quantity.
        if (_item != null)
        {
            quantityText.gameObject.SetActive(true);
            quantityText.text = $"{_item.quantity}";
        }
    }
}
