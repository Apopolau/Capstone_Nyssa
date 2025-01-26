using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class InGameMenuManager : MonoBehaviour
{
    private GameObject activeButtonSet;
    private GameObject activeLanguageScreen;
    private GameObject activeControlScreen;
    private GameObject activeConfirmScreen;

    private GameObject activeControlsGuide;

    [Header("English UI elements")]
    [SerializeField] private GameObject buttonEN;
    [SerializeField] private GameObject languageEN;
    [SerializeField] private GameObject controlsEN;
    [SerializeField] private GameObject confirmEN;
    [SerializeField] private GameObject keyboardGuideEN;
    [SerializeField] private GameObject controllerGuideEN;

    [Header("French UI elements")]
    [SerializeField] private GameObject buttonFR;
    [SerializeField] private GameObject languageFR;
    [SerializeField] private GameObject controlsFR;
    [SerializeField] private GameObject confirmFR;
    [SerializeField] private GameObject keyboardGuideFR;
    [SerializeField] private GameObject controllerGuideFR;

    [Header("External references")]
    [SerializeField] private HUDManager hudManager;
    [SerializeField] private UserSettingsManager userSettingsManager;
    [SerializeField] private EarthPlayerControl earthControls;
    [SerializeField] private CelestialPlayerControls celestialPlayerControls;
    //[SerializeField] private VirtualMouseInput virtualMouseInputS;
    //[SerializeField] private Vector2 virtualMousePositionS;

    [Header("Sound Effects")]
    [SerializeField] private UISoundLibrary soundLibrary;

    private void Awake()
    {
        if (userSettingsManager.chosenLanguage == UserSettingsManager.GameLanguage.ENGLISH)
        {
            SetLanguageToEnglish();
        }
        else if (userSettingsManager.chosenLanguage == UserSettingsManager.GameLanguage.FRENCH)
        {
            SetLanguageToFrench();
        }
        TurnOnMainPage();
    }

    public void PlayGame()
    {
        soundLibrary.PlaySubmitClips();
    }

    public void TurnOnMainPage()
    {
        //soundLibrary.PlayBackClips();
        activeButtonSet.SetActive(true);
        activeControlScreen.SetActive(false);
        activeLanguageScreen.SetActive(false);
        activeConfirmScreen.SetActive(false);
    }

    public void ToggleLanguagePage(bool turnOn)
    {

        if (turnOn)
        {
            soundLibrary.PlayClickClips();
            activeButtonSet.SetActive(false);
            activeControlScreen.SetActive(false);
            activeLanguageScreen.SetActive(true);
            activeConfirmScreen.SetActive(false);
        }
        else
        {
            activeLanguageScreen.SetActive(false);
        }
    }

    public void ToggleControlsGuide(bool turnOn)
    {
        if (turnOn)
        {
            soundLibrary.PlayClickClips();
            activeButtonSet.SetActive(false);
            activeControlScreen.SetActive(true);
            activeControlsGuide.SetActive(true);
            activeLanguageScreen.SetActive(false);
            activeConfirmScreen.SetActive(false);
        }
        else
        {
            activeControlScreen.SetActive(false);
        }
    }

    public void SetLanguageToEnglish()
    {
        soundLibrary.PlayClickClips();
        userSettingsManager.chosenLanguage = UserSettingsManager.GameLanguage.ENGLISH;
        activeButtonSet = buttonEN;
        activeLanguageScreen = languageEN;
        activeControlScreen = controlsEN;
        activeConfirmScreen = confirmEN;

        if(userSettingsManager.celestialControlType == UserSettingsManager.ControlType.KEYBOARD)
        {
            activeControlsGuide = keyboardGuideEN;
        }
        else if(userSettingsManager.celestialControlType == UserSettingsManager.ControlType.CONTROLLER)
        {
            activeControlsGuide = controllerGuideEN;
        }

        UpdateControlsGuide();

        DeactivateAllButtons();

        //buttonFR.SetActive(false);
        languageFR.SetActive(false);
        controlsFR.SetActive(false);

        //activeButtonSet.SetActive(true);
        activeLanguageScreen.SetActive(true);
        //activeControlScreen.SetActive(true);
    }

    public void SetLanguageToFrench()
    {
        soundLibrary.PlayClickClips();
        userSettingsManager.chosenLanguage = UserSettingsManager.GameLanguage.FRENCH;
        activeButtonSet = buttonFR;
        activeLanguageScreen = languageFR;
        activeControlScreen = controlsFR;
        activeConfirmScreen = confirmFR;

        if (userSettingsManager.celestialControlType == UserSettingsManager.ControlType.KEYBOARD)
        {
            activeControlsGuide = keyboardGuideFR;
        }
        else if (userSettingsManager.celestialControlType == UserSettingsManager.ControlType.CONTROLLER)
        {
            activeControlsGuide = controllerGuideFR;
        }

        UpdateControlsGuide();

        DeactivateAllButtons();

        //buttonEN.SetActive(false);
        languageEN.SetActive(false);
        controlsEN.SetActive(false);

        //activeButtonSet.SetActive(true);
        activeLanguageScreen.SetActive(true);
        //activeControlScreen.SetActive(true);


    }

    private void UpdateControlsGuide()
    {
        keyboardGuideEN.SetActive(false);
        keyboardGuideFR.SetActive(false);
        controllerGuideEN.SetActive(false);
        controllerGuideFR.SetActive(false);
        activeControlsGuide.SetActive(true);
    }

    private void DeactivateAllButtons()
    {
        buttonEN.SetActive(false);
        buttonFR.SetActive(false);
    }

    public void WarnPlayer()
    {
        activeConfirmScreen.SetActive(true);
        activeButtonSet.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void CloseMenu()
    {
        hudManager.ToggleMenuPanel(false);
    }

    public void SetHUDManager(HUDManager manager)
    {
        hudManager = manager;
    }
}
