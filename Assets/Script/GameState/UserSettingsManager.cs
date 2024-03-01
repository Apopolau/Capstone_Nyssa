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

    private void SetControls()
    {
        if (Gamepad.all.Count == 0)
        {
            earthControlType = UserSettingsManager.ControlType.KEYBOARD;
            celestialControlType = UserSettingsManager.ControlType.KEYBOARD;
        }
        else if (Gamepad.all.Count == 1)
        {
            earthControlType = UserSettingsManager.ControlType.KEYBOARD;
            celestialControlType= UserSettingsManager.ControlType.CONTROLLER;
        }
        else
        {
            earthControlType = UserSettingsManager.ControlType.CONTROLLER;
            celestialControlType = UserSettingsManager.ControlType.CONTROLLER;
        }
    }


    
}
