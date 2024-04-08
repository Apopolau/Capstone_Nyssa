using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New User Settings Manager", menuName = "ManagerObject/UserSettingsManager")]
public class UserSettingsManager : ScriptableObject
{
    public enum GameLanguage { ENGLISH, FRENCH};
    public GameLanguage chosenLanguage;

    public enum ControlType { KEYBOARD, CONTROLLER};
    public ControlType earthControlType;
    public ControlType celestialControlType;

    public void Initialize()
    {
        SetControls();
    }

    private void OnEnable()
    {
        SetControls();
    }

    public void SetControls()
    {
        if (Gamepad.all.Count == 0)
        {
            earthControlType = ControlType.KEYBOARD;
            celestialControlType = ControlType.KEYBOARD;
        }
        else if (Gamepad.all.Count == 1)
        {
            earthControlType = ControlType.KEYBOARD;
            celestialControlType = ControlType.CONTROLLER;
        }
        else
        {
            earthControlType = ControlType.CONTROLLER;
            celestialControlType = ControlType.CONTROLLER;
        }
    }

    // Method to set the chosen language
    public void SetLanguage(GameLanguage language)
    {
        chosenLanguage = language;
    }

    
}
