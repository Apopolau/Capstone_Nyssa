using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Sound Libraries/Ambient Sounds", fileName = "Ambient Sounds")]
public class AmbientSoundLibrary : SoundLibrary
{
    [Header("Music")]
    public List<AudioClip> musicClips;

    [Header("Weather Effects")]
    public List<AudioClip> weatherClips;

    [Header("Ambient Sound Effects")]
    public List<AudioClip> ambientSFX;
}
