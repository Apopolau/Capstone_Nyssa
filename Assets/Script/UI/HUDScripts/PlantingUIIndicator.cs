using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlantingUIIndicator : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] TextMeshProUGUI quantityText; // text field for quantity
    [SerializeField] string itemType;

    [SerializeField] private GameObjectRuntimeSet buildRuntimeSet;

    public Image grassImage;
    public Image treeImage;
    public Image flowerImage;

    [SerializeField] private GameObject shovelUIOverlay;

    private WaitForSeconds refreshTimer = new WaitForSeconds(0.5f);

    private void Start()
    {
        StartCoroutine(UpdateQuantityText());
    }

    private void Update()
    {
        //UpdateQuantityText();

        
    }

    public IEnumerator UpdateQuantityText()
    {
        while (true)
        {
            yield return refreshTimer;
            // Update the quantity text with the item's quantity.
            if (inventory != null && quantityText != null)
            {
                int quantity = inventory.GetQuantityByItemType(itemType);
                quantityText.text = $"{quantity}";

                //Debug.Log($"Item type: {itemType}, Quantity: {quantity}");

                // Check if the quantity is above 0
                if (quantity > 0)
                {
                    // Activate the UI element based on the itemType
                    ActivateUIByItemType(itemType);

                }
                else if (quantity == 0)
                {

                    DeactivateUIByItemType(itemType);
                }
            }
            else
            {
                quantityText.text = "0";
                //Debug.LogWarning("Inventory or quantityText is null.");
            }

            if (buildRuntimeSet.Items.Count == 0)
            {
                // Activate the UI if the build set count is 0
                shovelUIOverlay.SetActive(true);
            }
            else
            {
                // Deactivate the UI if the build set count is greater than 0
                shovelUIOverlay.SetActive(false);
            }
        }
        
    }
    // Function to activate UI element based on itemType
    private void ActivateUIByItemType(string itemType)
    {
        // Check itemType and activate corresponding UI element
        switch (itemType)
        {
            case "Grass Seed":
                ToggleIconsOverlay(grassImage, false); // Activate grass UI
                break;
            case "Tree Seed":
                ToggleIconsOverlay(treeImage, false); // Activate tree UI
                break;
            case "Flower Seed":
                ToggleIconsOverlay(flowerImage, false);
                break;
            default:
                break;
        }
    }
    // Function to deactivate UI element based on itemType
    private void DeactivateUIByItemType(string itemType)
    {
        // Check itemType and deactivate corresponding UI element
        switch (itemType)
        {
            case "Grass Seed":
                ToggleIconsOverlay(grassImage, true); // Deactivate grass UI
                break;
            case "Tree Seed":
                ToggleIconsOverlay(treeImage, true); // Deactivate tree UI
                break;
            case "Flower Seed":
                ToggleIconsOverlay(flowerImage, true);
                break;
            default:
                break;
        }
        //Debug.Log($"Item type: {itemType} overlay deactivated");
    }

    // Function to toggle visibility of UI element
    private void ToggleIconsOverlay(Image image, bool isActive)
    {
        // Set the active state of the image GameObject based on isActive parameter
        image.gameObject.SetActive(isActive);
    }


}
