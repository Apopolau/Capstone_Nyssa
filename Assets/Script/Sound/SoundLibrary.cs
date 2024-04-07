using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundLayerType
{
    ADDITIVE,
    SINGLE,
    UNIQUE
}

public class SoundLibrary : ScriptableObject
{
    [SerializeField] AudioClip[] soundLayers;
    [SerializeField] SoundLayerType layerType = SoundLayerType.ADDITIVE;
    [SerializeField] AudioMixerGroup mixer;

    public Vector2 volume = new Vector2(0.5f, 0.5f);
    public Vector2 pitch = new Vector2(1, 1);

    public AudioClip[] SoundLayers => soundLayers;
    public SoundLayerType SoundLayerType => layerType;
    public AudioMixerGroup Mixer => mixer;

    public void Play(MusicEvent musicEvent, float fadeTime)
    {
        MusicManager.Instance.PlayMusic(musicEvent, fadeTime);
    }

    public void Play()
    {

    }
}
