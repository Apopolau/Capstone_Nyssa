using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Audio/Music Event", fileName = "MUS_")]
public class MusicEvent : SoundEvent
{
    [SerializeField] AudioClip[] soundLayers;
    [SerializeField] SoundLayerType soundLayerType = SoundLayerType.ADDITIVE;
    [SerializeField] AudioMixerGroup mixer;

    public AudioClip[] SoundLayers => soundLayers;
    public SoundLayerType SoundLayerType => soundLayerType;
    public AudioMixerGroup Mixer => mixer;
}
