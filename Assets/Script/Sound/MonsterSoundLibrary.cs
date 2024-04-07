using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SoundLibraries/AmbientSounds", fileName = "Monster Sounds")]
public class MonsterSoundLibrary : SoundLibrary
{
    [Header("Movement")]
    public List<AudioClip> movementClips;

    [Header("Idle")]
    public List<AudioClip> idleClips;

    [Header("Attack")]
    public List<AudioClip> attackClips;

    [Header("Take hit")]
    public List<AudioClip> injuryClips;
}
