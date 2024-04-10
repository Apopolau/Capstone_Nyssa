using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public List<GameObject> englishUI;
    public List<GameObject> frenchUI;
    public UserSettingsManager userSettingsManager;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ActivateUIBasedOnLanguage(englishUI,frenchUI);
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
