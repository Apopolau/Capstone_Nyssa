using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// DEPRECATED
/// 
/// SEE HUDMANAGER AND HUDMODEL INSTEAD
/// 
/// </summary>
public class LanguageManager : MonoBehaviour
{
    public List<GameObject> englishUI;
    public List<GameObject> frenchUI;
    public UserSettingsManager userSettingsManager;
    
    void Start()
    {
        ActivateUIBasedOnLanguage(englishUI, frenchUI);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateUIBasedOnLanguage(List<GameObject> englishUI, List<GameObject> frenchUI)
    {
        if (userSettingsManager.chosenLanguage == UserSettingsManager.GameLanguage.ENGLISH)
        {
            DeactivateOtherUI(frenchUI);
            ActivateUI(englishUI);
            
        }
        else if (userSettingsManager.chosenLanguage == UserSettingsManager.GameLanguage.FRENCH)
        {
            DeactivateOtherUI(englishUI);
            ActivateUI(frenchUI);
            
        }
    }

    private void ActivateUI(List<GameObject> UIelements)
    {
        foreach (var element in UIelements)
        {
            element.SetActive(true);
        }
    }

    private void DeactivateOtherUI(List<GameObject> UIelements)
    {
         foreach (var element in UIelements)
        {
            element.SetActive(false);
        }
    }
}
