using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
 

public class EarthCharacterUIController : MonoBehaviour
{
   
    public Image darkOverlay;

    [SerializeField] private float darkeningAmount = 0.5f; // how much to darken the images

    public UserSettingsManager userSettingsManager;

    public GameObject[] keyboardUIObjects; // UI elements for keyboard control
    public GameObject[] controllerUIObjects; // UI elements for controller control

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
        /* // Check if the Image component is disabled
        if (!darkOverlay.enabled)
        {
            // Enable the Image component
            darkOverlay.enabled = true;
        }

        // Activate the GameObject
        darkOverlay.gameObject.SetActive(true);

        Debug.Log("Overlay is being activated"); */

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


    //public GameObject[] uiObjectToToggle; // Array of UI images to toggle

    // Method to toggle the visibility of other UI elements
    public void ToggleOtherUIElements(bool isActive)
    {
         // Check the current control type from the UserSettingsManager
        UserSettingsManager.ControlType currentControlType = userSettingsManager.earthControlType;

        // Determine which array of UI objects to use based on the current control type
        GameObject[] uiObjectsToToggle = (currentControlType == UserSettingsManager.ControlType.KEYBOARD) ? keyboardUIObjects : controllerUIObjects;

        // Loop through the UI objects in the selected array
        foreach (GameObject uiObject in uiObjectsToToggle)
        {
              Debug.Log("Toggling UI object: " + uiObject.name);
            // Toggle the visibility of the UI object based on the isActive parameter
            uiObject.SetActive(isActive);
        }
    }

    
}