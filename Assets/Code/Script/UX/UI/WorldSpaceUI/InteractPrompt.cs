using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractPrompt : MonoBehaviour
{
    // References to outside data
    [SerializeField] private UserSettingsManager settingsManager;

    // Internal References
    [SerializeField] private TextMeshProUGUI pickupLetter;
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] string englishPrompt;
    [SerializeField] string frenchPrompt;

    [SerializeField] int renderOrder;

    // Start is called before the first frame update
    void Start()
    {
        AssignRenderOrder();
        AssignLanguage();
        InitializeBoxSettings();
    }

    private void InitializeBoxSettings()
    {
        GameObject layoutObject = transform.GetChild(1).gameObject;
        LayoutRebuilder.MarkLayoutForRebuild(layoutObject.GetComponent<RectTransform>());
    }

    // Method to update the text based on the current settings
    public void SetButtonPrompt(Player player)
    {
        // Check if the settingsManager and textMeshPro are valid
        if (settingsManager != null && pickupLetter != null)
        {
            if (player.GetComponent<CelestialPlayer>())
            {
                if (settingsManager.celestialControlType == UserSettingsManager.ControlType.KEYBOARD)
                {
                    pickupLetter.text = "E";
                }
                else if (settingsManager.celestialControlType == UserSettingsManager.ControlType.CONTROLLER)
                {
                    pickupLetter.text = "A";
                }
            }
            else if (player.GetComponent<EarthPlayer>())
            {
                if (settingsManager.earthControlType == UserSettingsManager.ControlType.KEYBOARD)
                {
                    pickupLetter.text = "P";
                }
                else if (settingsManager.earthControlType == UserSettingsManager.ControlType.CONTROLLER)
                {
                    pickupLetter.text = "A";
                }
            }
        }
    }

    private void AssignLanguage()
    {
        if(settingsManager.chosenLanguage == UserSettingsManager.GameLanguage.ENGLISH)
        {
            promptText.text = englishPrompt;
        }
        else if(settingsManager.chosenLanguage == UserSettingsManager.GameLanguage.FRENCH)
        {
            promptText.text = frenchPrompt;
        }
    }

    private void AssignRenderOrder()
    {
        foreach (SpriteRenderer r in GetComponentsInChildren<SpriteRenderer>())
        {
            r.material.renderQueue += renderOrder;
        }
    }

    public void ChangeText(string newText)
    {
        promptText.text = newText;
    }
}
