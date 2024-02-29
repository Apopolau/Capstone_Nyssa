using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EarthCharacterUIController : MonoBehaviour
{
    // References to UI elements
    //public Image[] uiImages;
    //public Canvas[] uiCanvases;
    //[SerializeField] private GameObject dialogueBox; // Assign the DialogueBox GameObject in the inspector

    // Colors for normal and suspended states
    public Color normalColor = Color.white;
    public Color suspendedColor = Color.black;
    public Image darkOverlay;

    // Function to suspend UI elements
   /* public void SuspendUI()
    {
        // Change color or other properties of UI elements to indicate suspension
        foreach (Image image in uiImages)
        {
            image.color = suspendedColor;
        }
       
    } */

    // Function to restore UI elements
    public void RestoreUI()
    {
        if(darkOverlay.gameObject.activeSelf)
        {darkOverlay.gameObject.SetActive(false);}
        
    }

    public void DarkenOverlay()
    {
        // Check if the Image component is disabled
        if (!darkOverlay.enabled)
        {
            // Enable the Image component
            darkOverlay.enabled = true;
        }

        // Activate the GameObject
        darkOverlay.gameObject.SetActive(true);

        Debug.Log("Overlay is being activated");
    }


    public GameObject[] uiObjectToToggle; // Array of UI images to toggle

    // Method to toggle the visibility of other UI elements
    public void ToggleOtherUIElements(bool isActive)
    {
        foreach (GameObject uiObject in uiObjectToToggle)
        {
            uiObject.SetActive(isActive);
            //Debug.Log("ui is being deactivated");
        }
    }

    
}