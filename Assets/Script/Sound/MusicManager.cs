using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AmbienceManager : MonoBehaviour
{
    int activeLayerIndex = 0;
    public int ActiveLayerIndex => activeLayerIndex;

    SoundPlayer soundPlayer;
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

    private static AmbienceManager instance;
    public static AmbienceManager Instance
    {
        get
        {
            if(instance == null){
                instance = FindObjectOfType<AmbienceManager>();
                if(instance == null){
                    GameObject singletonGO = new GameObject("MusicManager_singleton");
                    instance = singletonGO.AddComponent<AmbienceManager>();

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

        SetupSoundPlayers();
    }

    void SetupSoundPlayers()
    {
        soundPlayer = gameObject.AddComponent<SoundPlayer>();
    }

    public void PlayMusic(SoundEvent soundEvent, float fadeTime)
    {
        if(soundEvent == null)
        {
            return;
        }
        soundPlayer.Play(soundEvent, fadeTime);
    }

    public void StopMusic(float fadeTime)
    {
        soundPlayer.Stop(fadeTime);
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
        soundPlayer.FadeVolume(Volume, fadeTime);
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
        soundPlayer.FadeVolume(Volume, fadeTime);
    }
}
