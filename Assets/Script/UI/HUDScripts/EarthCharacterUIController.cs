using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
 

public class EarthCharacterUIController : MonoBehaviour
{
   //PlantingOverlay
    public Image darkOverlay;

    [SerializeField] private float darkeningAmount = 0.5f; // how much to darken the images

    public UserSettingsManager userSettingsManager;

    //Earth player icons keyboard, celestial player icons keyboard, heal icon, thorn icon
    public GameObject[] keyboardUIObjects; // UI elements for keyboard control
    public GameObject[] controllerUIObjects; // UI elements for controller control
    public GameObject inventory;

    // Function to restore UI elements
    public void RestoreUI(GameObject targetGameObject)
    {
        Image[] images = targetGameObject.GetComponentsInChildren<Image>();
            foreach (Image image in images)
            {
                 // Restore the original color
                 image.material.color = image.color;
            }
        
    }

    public void DarkenOverlay(GameObject targetGameObject)
    {
        if (targetGameObject != null)
        {
            Image[] images = targetGameObject.GetComponentsInChildren<Image>();
            foreach (Image image in images)
            {
                // Create a copy of the current material
                Material darkenedMaterial = new Material(image.material);

                // Darken the material color
                Color darkenedColor = darkenedMaterial.color * darkeningAmount;
                darkenedMaterial.color = darkenedColor;

                // Assign the new material to the image
                image.material = darkenedMaterial;
            }

        }
        else
        {
            Debug.LogWarning("Target GameObject is not assigned.");
        }
    }



    // Method to toggle the visibility of other UI elements
    public void ToggleOtherUIElements(bool isActive)
    {
        ToggleInventory(isActive);
         // Check the current control type from the UserSettingsManager
        UserSettingsManager.ControlType currentControlType = userSettingsManager.earthControlType;
        
        // Log the current control type
        Debug.Log("Current Control Type: " + currentControlType);

        // Determine which array of UI objects to use based on the current control type
        GameObject[] uiObjectsToToggle = (currentControlType == UserSettingsManager.ControlType.KEYBOARD) ? keyboardUIObjects : controllerUIObjects;

        foreach (GameObject uiObject in uiObjectsToToggle)
        {
            Debug.Log("Toggling UI object: " + uiObject.name);
            // Toggle the visibility of the UI object based on the isActive parameter
            uiObject.SetActive(isActive);
        }

    }

    public void ToggleInventory(bool isActive)
    {
        if (isActive)
        {
            Image[] inventoryImages = inventory.GetComponentsInChildren<Image>();

            foreach(Image image in inventoryImages)
            {
                if (image.GetComponent<ItemSlot>())
                {
                    if(image.GetComponent<ItemSlot>().Item != null)
                    {
                        image.enabled = true;

                    }
                }
                else
                {
                    image.enabled = true;
                }
            }
            TextMeshPro[] text = inventory.GetComponentsInChildren<TextMeshPro>();
            foreach(TextMeshPro txt in text)
            {
                if(txt.text != "")
                {
                    txt.gameObject.SetActive(true);
                }
            }
            
        }
        else if (!isActive)
        {
            Image[] inventoryImages = inventory.GetComponentsInChildren<Image>();

            foreach (Image image in inventoryImages)
            {
                if (image.GetComponent<ItemSlot>())
                {
                    if (image.GetComponent<ItemSlot>().Item != null)
                    {
                        image.enabled = false;

                    }
                }
                else
                {
                    image.enabled = false;
                }
            }
            TextMeshPro[] text = inventory.GetComponentsInChildren<TextMeshPro>();
            foreach (TextMeshPro txt in text)
            {
                if (txt.text != "")
                {
                    txt.gameObject.SetActive(false);
                }
            }

        }
    }
}