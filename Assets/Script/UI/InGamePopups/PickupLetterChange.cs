using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PickupLetterChange : MonoBehaviour
{
     // Reference to the UserSettingsManager
    public UserSettingsManager settingsManager;

    // Reference to the TextMeshProUGUI component
    private TextMeshProUGUI pickupLetter;

    // Start is called before the first frame update
    void Start()
    {
        // Get the TextMeshProUGUI component attached to this GameObject
        pickupLetter = GetComponent<TextMeshProUGUI>();

        // If the UserSettingsManager reference is not set, try to find it in the scene
        if (settingsManager == null)
        {
            settingsManager = FindObjectOfType<UserSettingsManager>();
        }

    }

    void Update()
    {
        UpdateText();
    }

    // Method to update the text based on the current settings
    public void UpdateText()
    {
        // Check if the settingsManager and textMeshPro are valid
        if (settingsManager != null && pickupLetter != null)
        {
            // Update the text based on the control type from the user settings manager
            if (settingsManager.earthControlType == UserSettingsManager.ControlType.KEYBOARD)
            {
                pickupLetter.text = "F"; // Set text to "K" for keyboard
            }
            else if (settingsManager.earthControlType == UserSettingsManager.ControlType.CONTROLLER)
            {
                pickupLetter.text = "A"; // Set text to "C" for controller
            }
        }
    }
}
