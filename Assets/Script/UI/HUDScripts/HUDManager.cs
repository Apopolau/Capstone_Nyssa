using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [Header("Set these in the scene")]

    [Header("Individual UI elements")]
    [Tooltip("Drag Objectives here")]
    public GameObject objectives;
    [Tooltip("Drag InventoryPanel here")]
    public GameObject inventory;
    [Tooltip("Drag PollutionBar here")]
    public GameObject pollutionBar;
    [Tooltip("Drag DayNightCycle here")]
    public GameObject dayNightCycle;

    [Header("Celeste's UI")]
    //Celeste's power buttons
    [Tooltip("Icons for Celeste's powers on Keyboard")]
    public GameObject celesteKeyboardUI;
    [Tooltip("Icons for Celeste's powers on Controller")]
    public GameObject celesteControllerUI;
    [Tooltip("Movement control guide for Celeste on Keyboard")]
    public GameObject moveCelesteKeyboard;
    [Tooltip("Movement control guide for Celeste on Controller")]
    public GameObject moveCelesteController;
    [Tooltip("Drag EnergyBar here")]
    public GameObject energyBar;
    private GameObject activeCelesteUI;

    [Header("Sprout's UI")]
    //Celeste's power buttons
    [Tooltip("Icons for Celeste's powers on Keyboard")]
    public GameObject sproutKeyboardUI;
    [Tooltip("Icons for Celeste's powers on Controller")]
    public GameObject sproutControllerUI;
    [Tooltip("Movement control guide for Celeste on Keyboard")]
    public GameObject moveSproutKeyboard;
    [Tooltip("Movement control guide for Celeste on Controller")]
    public GameObject moveSproutController;
    private GameObject activeSproutUI;

    //Global settings manager
    public UserSettingsManager userSettingsManager;

    private void Start()
    {
        // Initialize UI based on current control type for EarthPlayer
        SetCelesteUIBasedOnControlType();
        SetSproutUIBasedOnControlType();
    }

    private void SetCelesteUIBasedOnControlType()
    {
        if (userSettingsManager.celestialControlType == UserSettingsManager.ControlType.KEYBOARD)
        {
            celesteKeyboardUI.SetActive(true);
            moveCelesteKeyboard.SetActive(true);
            celesteControllerUI.SetActive(false);
            moveCelesteController.SetActive(false);
            activeCelesteUI = celesteKeyboardUI;
        }
        else if (userSettingsManager.celestialControlType == UserSettingsManager.ControlType.CONTROLLER)
        {
            celesteKeyboardUI.SetActive(false);
            moveCelesteKeyboard.SetActive(false);
            celesteControllerUI.SetActive(true);
            moveCelesteController.SetActive(true);
            activeCelesteUI = celesteKeyboardUI;
        }
    }

    private void SetSproutUIBasedOnControlType()
    {
        if (userSettingsManager.earthControlType == UserSettingsManager.ControlType.KEYBOARD)
        {
            sproutKeyboardUI.SetActive(true);
            moveSproutKeyboard.SetActive(true);
            sproutControllerUI.SetActive(false);
            moveSproutController.SetActive(false);
            activeSproutUI = sproutKeyboardUI;
        }
        else if (userSettingsManager.earthControlType == UserSettingsManager.ControlType.CONTROLLER)
        {
            sproutKeyboardUI.SetActive(false);
            moveSproutKeyboard.SetActive(false);
            sproutControllerUI.SetActive(true);
            moveSproutController.SetActive(true);
            activeSproutUI = sproutKeyboardUI;
        }
    }



    public GameObject GetActiveCelesteUI()
    {
        return activeCelesteUI;
    }

    public GameObject GetActiveSproutUI()
    {
        return activeSproutUI;
    }
}
