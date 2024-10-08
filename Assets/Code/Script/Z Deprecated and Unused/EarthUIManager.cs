using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthUIManager : MonoBehaviour
{
   /* public GameObject keyboardUI;
    public GameObject controlsKeyboardUI;
    public GameObject controllerUI;
    public GameObject controlsControllerUI; */

    [SerializeField] private GameObject[] keyboardUIObjects;
    [SerializeField] private GameObject[] controllerUIObjects;

    public UserSettingsManager userSettingsManager;

    private void Start()
    {
        // Initialize UI based on current control type for EarthPlayer
        SetUIBasedOnControlType();
    }

   

    private void SetUIBasedOnControlType()
    {
        // Check the control type and activate/deactivate UI objects accordingly
        if (userSettingsManager.earthControlType == UserSettingsManager.ControlType.KEYBOARD)
        {
            SetUIObjectsActive(keyboardUIObjects, true);
            SetUIObjectsActive(controllerUIObjects, false);
        }
        else if (userSettingsManager.earthControlType == UserSettingsManager.ControlType.CONTROLLER)
        {
            SetUIObjectsActive(keyboardUIObjects, false);
            SetUIObjectsActive(controllerUIObjects, true);
        }
    }

    private void SetUIObjectsActive(GameObject[] uiObjects, bool setActive)
    {
        // Loop through each UI object in the array and set its active state
        foreach (var obj in uiObjects)
        {
            obj.SetActive(setActive);
        }
    }

}
