using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField]  TextMeshProUGUI quantityText;  // New field for displaying quantity.

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
                quantityText.text = "";
            }
            else
            {
                image.sprite = _item.Icon;
                image.enabled = true;
                UpdateQuantityText();
            }
        }
    }


  private void OnValidate()
{
    if (image == null)
        image = GetComponent<Image>();
    
    // Ensure that quantityText is set via the Unity Editor.
    if (quantityText == null)
        quantityText = GetComponentInChildren<TextMeshProUGUI>(); // Use TextMeshProUGUI here
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
