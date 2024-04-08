using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundEvent : ScriptableObject
{
    [SerializeField] AudioClip[] soundLayers;
    [SerializeField] SoundLayerType soundLayerType = SoundLayerType.ADDITIVE;
    [SerializeField] AudioMixerGroup mixer;

    public AudioClip[] SoundLayers => soundLayers;
    public SoundLayerType SoundLayerType => soundLayerType;
    public AudioMixerGroup Mixer => mixer;
}
