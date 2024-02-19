using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
   public static bool GameIsPaused = false;


    // Update is called once per frame
    void Update()
    {
        if (GameIsPaused)
        {
            Pause(); 
        }
        else
        {
            Resume(); 
        }
    }

    void Resume()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}
