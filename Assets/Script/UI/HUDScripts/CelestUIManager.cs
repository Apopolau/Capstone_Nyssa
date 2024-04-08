using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestUIManager : MonoBehaviour
{
    public GameObject keyboardUI;
    public GameObject controlsKeyboardUI;
    public GameObject controllerUI;
    public GameObject controlsControllerUI;
    private GameObject activeUI;

    public UserSettingsManager userSettingsManager;

    private void Start()
    {
        // Initialize UI based on current control type for EarthPlayer
        SetUIBasedOnControlType();
    }

    private void SetUIBasedOnControlType()
    {
        if (userSettingsManager.celestialControlType == UserSettingsManager.ControlType.KEYBOARD)
        {
            keyboardUI.SetActive(true);
            controlsKeyboardUI.SetActive(true);
            controllerUI.SetActive(false);
            controlsControllerUI.SetActive(false);
            activeUI = keyboardUI;
        }
        else if (userSettingsManager.celestialControlType == UserSettingsManager.ControlType.CONTROLLER)
        {
            keyboardUI.SetActive(false);
            controlsKeyboardUI.SetActive(false);
            controllerUI.SetActive(true);
            controlsControllerUI.SetActive(true);
            activeUI = controllerUI;
        }
    }

    public GameObject GetActiveUI()
    {
        return activeUI;
    }
}
