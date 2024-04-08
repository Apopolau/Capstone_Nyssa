using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusicManager : MonoBehaviour
{
    int activeLayerIndex = 0;
    public int ActiveLayerIndex => activeLayerIndex;

    MusicPlayer musicPlayer;
    public const int MaxLayerCount = 3;

    float volume = 1;
    public float Volume
    {
        get => volume;
        private set
        {
            value = Mathf.Clamp(value, 0, 1);
            volume = value;
        }
    }

    private static MusicManager instance;
    public static MusicManager Instance
    {
        get
        {
            if(instance == null){
                instance = FindObjectOfType<MusicManager>();
                if(instance == null){
                    GameObject singletonGO = new GameObject("MusicManager_singleton");
                    instance = singletonGO.AddComponent<MusicManager>();

                    DontDestroyOnLoad(singletonGO);
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        SetupMusicPlayers();
    }

    void SetupMusicPlayers()
    {
        musicPlayer = gameObject.AddComponent<MusicPlayer>();
    }

    public void PlayMusic(MusicEvent musicEvent, float fadeTime)
    {
        if(musicEvent == null)
        {
            return;
        }
        musicPlayer.Play(musicEvent, fadeTime);
    }

    public void StopMusic(float fadeTime)
    {
        musicPlayer.Stop(fadeTime);
    }

    public void IncreaseLayerIndex(float fadeTime)
    {
        int newLayerIndex = activeLayerIndex + 1;
        newLayerIndex = Mathf.Clamp(newLayerIndex, 0, MaxLayerCount - 1);

        if(newLayerIndex == activeLayerIndex)
        {
            return;
        }

        activeLayerIndex = newLayerIndex;
        musicPlayer.FadeVolume(Volume, fadeTime);
    }

    public void DecreaseLayerIndex(float fadeTime)
    {
        int newLayerIndex = activeLayerIndex - 1;
        newLayerIndex = Mathf.Clamp(newLayerIndex, 0, MaxLayerCount - 1);

        if (newLayerIndex == activeLayerIndex)
        {
            return;
        }

        activeLayerIndex = newLayerIndex;
        musicPlayer.FadeVolume(Volume, fadeTime);
    }
}
