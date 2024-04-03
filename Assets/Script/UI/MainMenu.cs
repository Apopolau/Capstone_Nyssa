using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public UserSettingsManager userSettingsManager; // Reference to your UserSettingsManager object
    public GameObject keyboardInstrcutions;
    public GameObject controllerInstructions;

    private void Start()
    {
        // Initialize UI based on current control type for EarthPlayer
        SetInstructionBasedOnControlType();
    }

    public void SetLanguageEnglish()
    {
        userSettingsManager.SetLanguage(UserSettingsManager.GameLanguage.ENGLISH);
    }

    public void SetLanguageFrench()
    {
        userSettingsManager.SetLanguage(UserSettingsManager.GameLanguage.FRENCH);
    }
    
    public void PlayGame(){
        SceneManager.LoadScene("CutSceneIntro");

    }

    private void SetInstructionBasedOnControlType()
    {
        if ((userSettingsManager.celestialControlType == UserSettingsManager.ControlType.KEYBOARD) && (userSettingsManager.earthControlType == UserSettingsManager.ControlType.KEYBOARD))
        {
           Debug.Log("display keyboard instructions");
           keyboardInstrcutions.SetActive(true);
           controllerInstructions.SetActive(false);
        }
        else if ((userSettingsManager.celestialControlType == UserSettingsManager.ControlType.CONTROLLER) && (userSettingsManager.earthControlType == UserSettingsManager.ControlType.CONTROLLER))
        {
            Debug.Log("display controller instructions");
            keyboardInstrcutions.SetActive(false);
            controllerInstructions.SetActive(true);
        }
    }
    
}
