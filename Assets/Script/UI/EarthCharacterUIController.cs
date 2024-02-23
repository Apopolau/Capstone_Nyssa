using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EarthCharacterUIController : MonoBehaviour
{
    // References to UI elements
    public Image[] uiImages;
    //public Canvas[] uiCanvases;
    [SerializeField] private GameObject dialogueBox; // Assign the DialogueBox GameObject in the inspector

    // Colors for normal and suspended states
    public Color normalColor = Color.white;
    public Color suspendedColor = Color.black;

    // Function to suspend UI elements
    public void SuspendUI()
    {
        // Change color or other properties of UI elements to indicate suspension
        foreach (Image image in uiImages)
        {
            image.color = suspendedColor;
        }
       
    }

    // Function to restore UI elements
    public void RestoreUI()
    {
        // Restore UI elements to their normal appearance
        foreach (Image image in uiImages)
        {
            image.color = normalColor;
        }
        
    }

    public void DeactivateAllUI()
    {
        

        foreach (Image image in uiImages)
        { // Check if the dialogue box is active
        bool dialogueActive = dialogueBox.activeSelf;
       
            if (dialogueActive)
            {
                image.gameObject.SetActive(false);
            }
            
            if (!dialogueActive )
            {
                image.gameObject.SetActive(true);
                Debug.Log("re activate the ui");
            }
        } 
    }
}