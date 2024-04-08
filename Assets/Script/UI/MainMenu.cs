using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public UserSettingsManager userSettingsManager; // Reference to your UserSettingsManager object
    public GameObject keyboardInstructions;
    public GameObject controllerInstructions;

    private void Start()
    {
        // Initialize UI based on current control type for EarthPlayer
        SetInstructionBasedOnControlType();
    }
    
    

    public void SetInstructionBasedOnControlType()
    {
        if ((userSettingsManager.celestialControlType == UserSettingsManager.ControlType.KEYBOARD) && (userSettingsManager.earthControlType == UserSettingsManager.ControlType.KEYBOARD))
        {
           Debug.Log("display keyboard instructions");
           keyboardInstructions.SetActive(true);
           controllerInstructions.SetActive(false);
        }
        else if ((userSettingsManager.celestialControlType == UserSettingsManager.ControlType.CONTROLLER) && (userSettingsManager.earthControlType == UserSettingsManager.ControlType.CONTROLLER))
        {
            Debug.Log("display controller instructions");
            keyboardInstructions.SetActive(false);
            controllerInstructions.SetActive(true);
        }
    }
    
}
