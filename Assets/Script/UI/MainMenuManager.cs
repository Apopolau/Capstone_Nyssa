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

    public List<GameObject> englishButtons;
    public List<GameObject> frenchButtons;

    public UserSettingsManager userSettingsManager;
    public EarthPlayerControl earthControls;
    public CelestialPlayerControls celestialPlayerControls;
    //public VirtualMouseInput virtualMouseInputC;
    //public Vector2 virtualMousePositionC;
    public VirtualMouseInput virtualMouseInputS;
    public Vector2 virtualMousePositionS;

    [SerializeField] private UISoundLibrary soundLibrary;

    private void Awake()
    {   // always reset to english
        SetLanguageToEnglish();
    }

    public void PlayGame()
    {
        soundLibrary.PlaySubmitClips();
        SceneManager.LoadScene("CutSceneIntro");

    }

    public void TurnOnMainPage()
    {
        soundLibrary.PlayBackClips();
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
            soundLibrary.PlayClickClips();
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
            soundLibrary.PlayClickClips();
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
        soundLibrary.PlayClickClips();
        userSettingsManager.chosenLanguage = UserSettingsManager.GameLanguage.ENGLISH;
        mainMenu.SetInstructionBasedOnControlType();
        languageMenu.SetInstructionBasedOnControlType();
        instructionsMenu.SetInstructionBasedOnControlType();

        DeactivateAllButtons();
        ActivateButtonsBasedOnLanguage(englishButtons, frenchButtons);

        
    }

    public void SetLanguageToFrench()
    {
        soundLibrary.PlayClickClips();
        userSettingsManager.chosenLanguage = UserSettingsManager.GameLanguage.FRENCH;
        mainMenu.SetInstructionBasedOnControlType();
        languageMenu.SetInstructionBasedOnControlType();
        instructionsMenu.SetInstructionBasedOnControlType();

         DeactivateAllButtons();
        ActivateButtonsBasedOnLanguage(englishButtons, frenchButtons);

       
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

    public void ActivateButtonsBasedOnLanguage(List<GameObject> englishButtons, List<GameObject> frenchButtons)
    {
        if (userSettingsManager.chosenLanguage == UserSettingsManager.GameLanguage.ENGLISH)
        {
            ActivateButtons(englishButtons);
        }
        else if (userSettingsManager.chosenLanguage == UserSettingsManager.GameLanguage.FRENCH)
        {
            ActivateButtons(frenchButtons);
        }
    }

    private void ActivateButtons(List<GameObject> buttons)
    {
        foreach (var button in buttons)
        {
            button.SetActive(true);
        }
    }

    private void DeactivateAllButtons()
    {
        foreach (var button in englishButtons)
        {
            button.SetActive(false);
        }
        foreach (var button in frenchButtons)
        {
            button.SetActive(false);
        }
    }
}
