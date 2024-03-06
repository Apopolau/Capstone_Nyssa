using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public UserSettingsManager userSettingsManager; // Reference to your UserSettingsManager object

    public void SetLanguageEnglish()
    {
        userSettingsManager.SetLanguage(UserSettingsManager.GameLanguage.ENGLISH);
    }

    public void SetLanguageFrench()
    {
        userSettingsManager.SetLanguage(UserSettingsManager.GameLanguage.FRENCH);
    }
    
    public void PlayGame(){
        SceneManager.LoadScene("LevelOne");

    }
    
}
