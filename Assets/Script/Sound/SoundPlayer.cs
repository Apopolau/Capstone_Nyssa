using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    List<AudioSource> layerSources = new List<AudioSource>();

    private void Awake()
    {
        CreateLayerSources();
    }

    void CreateLayerSources()
    {
        for (int i = 0; i < MusicManager.MaxLayerCount; i++)
        {
            layerSources.Add(gameObject.AddComponent<AudioSource>());
            layerSources[i].playOnAwake = false;
            layerSources[i].loop = true;
        }
    }

    public void Play(SoundEvent soundEvent, float fadeTime)
    {
        Debug.Log("Play music");
        /*
        for(int i = 0; (i < layerSources.Count) && (i < soundEvent.SoundLayers.Length); i++)
        {

        }
        */
    }
    public void Play(MusicEvent musicEvent, float fadeTime)
    {
        Debug.Log("Play music");
        
        for(int i = 0; (i < layerSources.Count) && (i < musicEvent.MusicLayers.Length); i++)
        {
            if (musicEvent.MusicLayers[i] != null)
            {
                layerSources[i].clip = musicEvent.MusicLayers[i];
                layerSources[i].Play();
            }
        }
        
    }
}


