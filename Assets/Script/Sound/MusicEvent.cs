using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SoundSystem/Music Event", fileName = "MUS_")]
public class MusicEvent : SoundEvent
{
    AudioClip[] musicLayers;

    public AudioClip[] MusicLayers => musicLayers;
}
