using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthUIManager : MonoBehaviour
{
    public GameObject keyboardUI;
    public GameObject controlsKeyboardUI;
    public GameObject controllerUI;
    public GameObject controlsControllerUI;

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
            controlsKeyboardUI.SetActive(true);
            controllerUI.SetActive(false);
            controlsControllerUI.SetActive(false);
        }
        else if (userSettingsManager.earthControlType == UserSettingsManager.ControlType.CONTROLLER)
        {
            keyboardUI.SetActive(false);
            controlsKeyboardUI.SetActive(false);
            controllerUI.SetActive(true);
            controlsControllerUI.SetActive(true);
        }
    }

}
