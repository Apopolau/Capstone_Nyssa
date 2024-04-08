using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    List<AudioSource> layerSources = new List<AudioSource>();
    List<float> sourceStartVolumes = new List<float>();
    MusicEvent cMusicEvent = null;

    Coroutine fadeVolumeRoutine = null;
    Coroutine stopRoutine = null;

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
        if(musicEvent == null)
        {
            return;
        }

        cMusicEvent = musicEvent;
        
        for(int i = 0; (i < layerSources.Count) && (i < musicEvent.SoundLayers.Length); i++)
        {
            if (musicEvent.SoundLayers[i] != null)
            {
                layerSources[i].volume = 0;
                layerSources[i].clip = musicEvent.SoundLayers[i];
                layerSources[i].outputAudioMixerGroup = musicEvent.Mixer;
                layerSources[i].Play();
            }
        }

        FadeVolume(MusicManager.Instance.Volume, fadeTime);
    }

    public void Stop(float fadeTime)
    {
        if(stopRoutine != null)
        {
            StopCoroutine(stopRoutine);
        }
        stopRoutine = StartCoroutine(StopRoutine(fadeTime));
    }

    IEnumerator StopRoutine(float fadeTime)
    {
        if(fadeVolumeRoutine != null)
        {
            StopCoroutine(fadeVolumeRoutine);
        }

        //blend the volume to 0, depending on blend type
        if(cMusicEvent.SoundLayerType == SoundLayerType.ADDITIVE)
        {
            fadeVolumeRoutine = StartCoroutine(LerpSourceAdditiveRoutine(0, fadeTime));
        }
        else if (cMusicEvent.SoundLayerType == SoundLayerType.SINGLE)
        {
            fadeVolumeRoutine = StartCoroutine(LerpSourceSingleRoutine(0, fadeTime));
        }

        //wait for blend to finish
        yield return fadeVolumeRoutine;
        //stop all audio sources
        foreach(AudioSource source in layerSources)
        {
            source.Stop();
        }
    }

    public void FadeVolume(float targetVolume, float fadeTime)
    {
        targetVolume = Mathf.Clamp(targetVolume, 0f, 1f);
        if (fadeTime < 0) fadeTime = 0;

        if(fadeVolumeRoutine != null)
        {
            StopCoroutine(fadeVolumeRoutine);
        }

        if(cMusicEvent.SoundLayerType == SoundLayerType.SINGLE)
        {
            fadeVolumeRoutine = StartCoroutine(LerpSourceSingleRoutine(targetVolume, fadeTime));
        }
        if(cMusicEvent.SoundLayerType == SoundLayerType.ADDITIVE)
        {
            fadeVolumeRoutine = StartCoroutine(LerpSourceAdditiveRoutine(targetVolume, fadeTime));
        }
    }

    IEnumerator LerpSourceSingleRoutine(float targetVolume, float fadeTime)
    {
        SaveSourceStartVolumes();

        float newVolume;
        float startVolume;

        for (float elapsedTime = 0; elapsedTime <= fadeTime; elapsedTime += Time.deltaTime)
        {
            for (int i = 0; i < layerSources.Count; i++)
            {
                if(i == MusicManager.Instance.ActiveLayerIndex)
                {
                    startVolume = sourceStartVolumes[i];
                    newVolume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / fadeTime);
                    layerSources[i].volume = newVolume;
                }
                else
                {
                    startVolume = sourceStartVolumes[i];
                    newVolume = Mathf.Lerp(startVolume, 0, elapsedTime / fadeTime);
                    layerSources[i].volume = newVolume;
                }
            }

            yield return null;
        }

        for (int i = 0; i < layerSources.Count; i++)
        {
            if (i == MusicManager.Instance.ActiveLayerIndex)
            {
                layerSources[i].volume = targetVolume;
            }
            else
            {
                layerSources[i].volume = 0;
            }
        }
    }

    IEnumerator LerpSourceAdditiveRoutine(float targetVolume, float fadeTime)
    {
        SaveSourceStartVolumes();

        float newVolume;
        float startVolume;

        for(float elapsedTime = 0; elapsedTime <= fadeTime; elapsedTime += Time.deltaTime)
        {
            for(int i = 0; i < layerSources.Count; i++)
            {
                //If in an active layer, fade in to target
                if(i <= MusicManager.Instance.ActiveLayerIndex)
                {
                    startVolume = sourceStartVolumes[i];
                    newVolume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / fadeTime);
                    layerSources[i].volume = newVolume;
                }
                //otherwise fade to 0 from current volume
                else
                {
                    startVolume = sourceStartVolumes[i];
                    newVolume = Mathf.Lerp(startVolume, 0, elapsedTime / fadeTime);
                    layerSources[i].volume = newVolume;
                }
            }

            yield return null;
        }

        for(int i = 0; i < layerSources.Count; i++)
        {
            if (i <= MusicManager.Instance.ActiveLayerIndex)
            {
                layerSources[i].volume = targetVolume;
            }
            else
            {
                layerSources[i].volume = 0;
            }
        }
    }

    private void SaveSourceStartVolumes()
    {
        sourceStartVolumes.Clear();
        for (int i = 0; i < layerSources.Count; i++)
        {
            sourceStartVolumes.Add(layerSources[i].volume);
        }
    }
}


