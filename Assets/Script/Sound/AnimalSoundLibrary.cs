using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SoundLibraries/AnimalLibrary", fileName = "Animal Sounds")]
public class AnimalSoundLibrary : EventSoundLibrary
{
    [Header("Movement")]
    public List<InstanceSoundEvent> movementClips;

    [Header("Idle")]
    public List<InstanceSoundEvent> idleClips;

    [Header("Vocalize")]
    public List<InstanceSoundEvent> vocalizeClips;

    [Header("Panic")]
    public List<InstanceSoundEvent> panicClips;


    public void PlayMovementClips()
    {
        if (movementClips != null)
        {
            int index = Random.Range(0, movementClips.Count);
            movementClips[index].Play();
        }

    }

    public void PlayIdleClips()
    {
        if (idleClips != null)
        {
            int index = Random.Range(0, idleClips.Count);
            idleClips[index].Play();
        }

    }

    public void PlayVocalizeClips()
    {
        if (vocalizeClips != null)
        {
            int index = Random.Range(0, vocalizeClips.Count);
            vocalizeClips[index].Play();
        }

    }

    public void PlayPanicClips()
    {
        if (panicClips != null)
        {
            int index = Random.Range(0, panicClips.Count);
            panicClips[index].Play();
        }
    }
}
