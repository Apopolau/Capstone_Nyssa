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
        if (movementClips != null && movementClips.Count > 1)
        {
            int index = Random.Range(0, movementClips.Count - 1);
            movementClips[index].Play();
        }
        else if (movementClips.Count == 1)
        {
            movementClips[0].Play();
        }

    }

    public void PlayIdleClips()
    {
        if (idleClips != null && idleClips.Count > 1)
        {
            int index = Random.Range(0, idleClips.Count - 1);
            idleClips[index].Play();
        }
        else if (idleClips.Count == 1)
        {
            idleClips[0].Play();
        }

    }

    public void PlayVocalizeClips()
    {
        if (vocalizeClips != null && vocalizeClips.Count > 1)
        {
            int index = Random.Range(0, vocalizeClips.Count - 1);
            vocalizeClips[index].Play();
        }
        else if (vocalizeClips.Count == 1)
        {
            vocalizeClips[0].Play();
        }

    }

    public void PlayPanicClips()
    {
        if (panicClips != null && panicClips.Count > 1)
        {
            int index = Random.Range(0, panicClips.Count - 1);
            panicClips[index].Play();
        }
        else if (panicClips.Count == 1)
        {
            panicClips[0].Play();
        }
    }
}
