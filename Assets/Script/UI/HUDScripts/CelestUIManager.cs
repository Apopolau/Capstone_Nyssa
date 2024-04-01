using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestUIManager : MonoBehaviour
{
    public GameObject keyboardUI;
    public GameObject controllerUI;

    public UserSettingsManager userSettingsManager;

    private void Start()
    {
        // Initialize UI based on current control type for EarthPlayer
        SetUIBasedOnControlType();
    }

    private void SetUIBasedOnControlType()
    {
        if (userSettingsManager.earthControlType == UserSettingsManager.ControlType.KEYBOARD)
        {
            keyboardUI.SetActive(true);
            controllerUI.SetActive(false);
        }
        else if (userSettingsManager.earthControlType == UserSettingsManager.ControlType.CONTROLLER)
        {
            keyboardUI.SetActive(false);
            controllerUI.SetActive(true);
        }
    }
}
