using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public GameObject currentImg;
    public GameObject nextImg;

      void Update()
    {
        // Check for input from keyboard , can then change it to controllor input
        if ((Input.GetKeyDown(KeyCode.Return) && nextImg != null))
        {
            NextImage();
        }
        else if ((Input.GetKeyDown(KeyCode.Return) && nextImg == null))
        {
            LoadNewScene();
        }
    }
    public void NextImage()
    {
       nextImg.SetActive(true);
    }
    public void LoadNewScene()
    {
        if(nextImg != null)
        {
            currentImg.SetActive(false);
        }
        if (nextImg == null)
        {

            //SceneManager.LoadScene("LevelOne");
            SceneManager.LoadScene("FatimaUITest");
        }
    }

}
