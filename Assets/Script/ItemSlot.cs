using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI itemNameText;  // New field for displaying item name.
    [SerializeField] TextMeshProUGUI quantityText;   // Field for displaying quantity.

    private Item _item;

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
                image.sprite = _item.Icon;
                image.enabled = true;
                UpdateItemNameText();
                UpdateQuantityText();
            }
        }
    }

    private void OnValidate()
    {
        if (image == null)
            image = GetComponent<Image>();

        // Ensure that itemNameText and quantityText are set via the Unity Editor.
        if (itemNameText == null)
            itemNameText = GetComponentInChildren<TextMeshProUGUI>(); // Use TextMeshProUGUI here

        if (quantityText == null)
            quantityText = GetComponentInChildren<TextMeshProUGUI>(); // Use TextMeshProUGUI here
    }

    public void UpdateItemNameText()
    {
        // Update the item name text with the item's name.
        if (_item != null)
        {
            itemNameText.text = _item.ItemName;
        }
    }

    public void UpdateQuantityText()
    {
        // Update the quantity text with the item's quantity.
        if (_item != null)
        {
            quantityText.text = $"{_item.Quantity}";
        }
    }
}
