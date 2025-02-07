using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New User Settings Manager", menuName = "Manager Object/User Settings Manager")]
public class UserSettingsManager : ScriptableObject
{
    public enum GameLanguage { ENGLISH, FRENCH};
    public GameLanguage chosenLanguage;

    public enum ControlType { KEYBOARD, CONTROLLER};
    public ControlType earthControlType;
    public ControlType celestialControlType;

    public event System.Action<GameLanguage> OnLanguageChanged;
    public event System.Action<ControlType, int> OnControlTypeChanged; //0 will be Celeste, 1 will be Sprout

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

        if (OnControlTypeChanged != null)
        {
            OnControlTypeChanged(celestialControlType, 0);
            OnControlTypeChanged(earthControlType, 1);
        }
            
    }

    public void UpdateCelesteControls()
    {
        if (OnControlTypeChanged != null)
            OnControlTypeChanged(celestialControlType, 0);
    }

    public void UpdateSproutControls()
    {
        if (OnControlTypeChanged != null)
            OnControlTypeChanged(earthControlType, 1);
    }

    // Method to set the chosen language
    public void SetLanguage(GameLanguage language)
    {
        chosenLanguage = language;
        if (OnLanguageChanged != null)
            OnLanguageChanged(language);
    }

    
}
