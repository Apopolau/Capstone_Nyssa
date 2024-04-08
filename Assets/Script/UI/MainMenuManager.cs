using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainPage;
    [SerializeField] GameObject languagePage;
    [SerializeField] GameObject instructions;

    [SerializeField] MainMenu mainMenu;
    [SerializeField] MainMenu languageMenu;
    [SerializeField] MainMenu instructionsMenu;

    public UserSettingsManager userSettingsManager;
    public EarthPlayerControl earthControls;
    public CelestialPlayerControls celestialPlayerControls;
    //public VirtualMouseInput virtualMouseInputC;
    //public Vector2 virtualMousePositionC;
    public VirtualMouseInput virtualMouseInputS;
    public Vector2 virtualMousePositionS;

    private void Awake()
    {
        
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("CutSceneIntro");

    }

    public void TurnOnMainPage()
    {
        Debug.Log("Going back to main menu");
        mainPage.SetActive(true);
        ToggleLanguagePage(false);
        ToggleInstructions(false);
    }

    public void ToggleLanguagePage(bool turnOn)
    {
        Debug.Log("Toggling language page");
        if (turnOn)
        {
            languagePage.SetActive(true);
            mainPage.SetActive(false);
        }
        else
        {
            languagePage.SetActive(false);
        }
    }

    public void ToggleInstructions(bool turnOn)
    {
        Debug.Log("Toggling instructions");
        if (turnOn)
        {
            instructions.SetActive(true);
            mainPage.SetActive(false);
        }
        else
        {
            instructions.SetActive(false);
        }
    }

    public void SetLanguageToEnglish()
    {
        userSettingsManager.chosenLanguage = UserSettingsManager.GameLanguage.ENGLISH;
        mainMenu.SetInstructionBasedOnControlType();
        languageMenu.SetInstructionBasedOnControlType();
        instructionsMenu.SetInstructionBasedOnControlType();
    }

    public void SetLanguageToFrench()
    {
        userSettingsManager.chosenLanguage = UserSettingsManager.GameLanguage.FRENCH;
        mainMenu.SetInstructionBasedOnControlType();
        languageMenu.SetInstructionBasedOnControlType();
        instructionsMenu.SetInstructionBasedOnControlType();
    }

    private void LateUpdate()
    {
        if (earthControls.userSettingsManager.earthControlType == UserSettingsManager.ControlType.CONTROLLER)
        {
            virtualMouseInputS.cursorTransform.position = virtualMouseInputS.virtualMouse.position.value;
            virtualMousePositionS = virtualMouseInputS.cursorTransform.position;
        }
        else if (earthControls.userSettingsManager.earthControlType == UserSettingsManager.ControlType.KEYBOARD)
        {
            virtualMouseInputS.cursorTransform.position = Mouse.current.position.value;
            virtualMousePositionS = Mouse.current.position.value;
        }
    }
}
