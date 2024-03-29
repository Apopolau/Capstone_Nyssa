using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public string levelName;
    [SerializeField] UserSettingsManager userSettingsManager;
    public List<GameObject> sceneListEN;
    private Queue<GameObject> q_sceneListEN;
    public List<GameObject> sceneListFR;
    private Queue<GameObject> q_sceneListFR;
    public GameObject currentImg;
    public GameObject nextImg;

    private void Start()
    {
        q_sceneListEN = new Queue<GameObject>();
        q_sceneListFR = new Queue<GameObject>();
        //Draw up a queue we can use for each scene slide
        q_sceneListEN.Clear();
        q_sceneListFR.Clear();

        foreach (GameObject scene in sceneListEN)
        {
            q_sceneListEN.Enqueue(scene);
        }
        foreach (GameObject scene in sceneListFR)
        {
            q_sceneListFR.Enqueue(scene);
        }

        //Set the current scene based on the language chosen
        if (userSettingsManager.chosenLanguage == UserSettingsManager.GameLanguage.ENGLISH)
        {
            currentImg = q_sceneListEN.Dequeue();
        }
        else if(userSettingsManager.chosenLanguage == UserSettingsManager.GameLanguage.FRENCH)
        {
            currentImg = q_sceneListFR.Dequeue();
        }
        currentImg.SetActive(true);
    }

    public void NextImage()
    {
        //Update the next scene
        if (userSettingsManager.chosenLanguage == UserSettingsManager.GameLanguage.ENGLISH)
        {
            Debug.Log("Dequeueing in English");
            nextImg = q_sceneListEN.Dequeue();
            if(nextImg != null && q_sceneListEN.Count > 0)
            {
                currentImg = nextImg;
            }
            else
            {
                LoadNewScene();
            }
        }
        else if (userSettingsManager.chosenLanguage == UserSettingsManager.GameLanguage.FRENCH)
        {
            Debug.Log("Dequeueing in French");
            nextImg = q_sceneListFR.Dequeue();
            if (nextImg != null && q_sceneListFR.Count > 0)
            {
                currentImg = nextImg;
            }
            else
            {
                LoadNewScene();
            }
        }
        currentImg.SetActive(true);
    }

    public void LoadNewScene()
    {
        SceneManager.LoadScene(levelName);
    }

}
