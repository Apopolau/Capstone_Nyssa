using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundLibrary : EventSoundLibrary
{
    [Header("Movement")]
    public List<InstanceSoundEvent> movementClips;

    [Header("Idle")]
    public List<InstanceSoundEvent> idleClips;

    [Header("Spells")]
    public List<InstanceSoundEvent> spellClips;

    [Header("Take hit")]
    public List<InstanceSoundEvent> injuryClips;

    [Header("Death")]
    public List<InstanceSoundEvent> deathClips;


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

    public void PlaySpellClips()
    {
        if (spellClips != null)
        {
            int index = Random.Range(0, spellClips.Count);
            spellClips[index].Play();
        }

    }

    public void PlayTakeHitClips()
    {
        if (injuryClips != null)
        {
            int index = Random.Range(0, injuryClips.Count);
            injuryClips[index].Play();
        }
    }

    public void PlayDeathClips()
    {
        if (deathClips != null)
        {
            int index = Random.Range(0, deathClips.Count);
            deathClips[index].Play();
        }
    }
}
