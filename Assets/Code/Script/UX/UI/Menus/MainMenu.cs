using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public UserSettingsManager userSettingsManager; // Reference to your UserSettingsManager object
      public GameObject ENKeyboardInstructions;
    public GameObject ENControllerInstructions;

    public GameObject FRKeyboardInstructions;
    public GameObject FRControllerInstructions;

    private void Start()
    {
        // Initialize UI based on current control type for EarthPlayer
        SetInstructionBasedOnControlType();
    }
    


    public void SetInstructionBasedOnControlType()
    {
        if (userSettingsManager.chosenLanguage == UserSettingsManager.GameLanguage.ENGLISH)
        {
            if ((userSettingsManager.celestialControlType == UserSettingsManager.ControlType.KEYBOARD) && (userSettingsManager.earthControlType == UserSettingsManager.ControlType.KEYBOARD))
            {
                ENKeyboardInstructions.SetActive(true);
                ENControllerInstructions.SetActive(false);
                FRKeyboardInstructions.SetActive(false);
                FRControllerInstructions.SetActive(false);
            }
            else if ((userSettingsManager.celestialControlType == UserSettingsManager.ControlType.CONTROLLER) && (userSettingsManager.earthControlType == UserSettingsManager.ControlType.CONTROLLER))
            {
                ENKeyboardInstructions.SetActive(false);
                ENControllerInstructions.SetActive(true);
                FRKeyboardInstructions.SetActive(false);
                FRControllerInstructions.SetActive(false);
                
            }
        }
        else if (userSettingsManager.chosenLanguage == UserSettingsManager.GameLanguage.FRENCH)
        {
            if ((userSettingsManager.celestialControlType == UserSettingsManager.ControlType.KEYBOARD) && (userSettingsManager.earthControlType == UserSettingsManager.ControlType.KEYBOARD))
            {
                FRKeyboardInstructions.SetActive(true);
                FRControllerInstructions.SetActive(false);
                ENKeyboardInstructions.SetActive(false);
                ENControllerInstructions.SetActive(false);
            }
            else if ((userSettingsManager.celestialControlType == UserSettingsManager.ControlType.CONTROLLER) && (userSettingsManager.earthControlType == UserSettingsManager.ControlType.CONTROLLER))
            {
                FRKeyboardInstructions.SetActive(false);
                FRControllerInstructions.SetActive(true);
                ENKeyboardInstructions.SetActive(false);
                ENControllerInstructions.SetActive(false);
            }
        }
    }
    
    
}
