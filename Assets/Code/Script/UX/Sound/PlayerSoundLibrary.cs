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
            if (movementClips.Count > 1)
            {
                int index = Random.Range(0, movementClips.Count - 1);
                movementClips[index].Play();
            }
            else if (movementClips.Count == 1)
            {
                movementClips[0].Play();
            }
        }

    }

    public void PlayIdleClips()
    {
        if (idleClips != null)
        {
            if(idleClips.Count > 1)
            {
                int index = Random.Range(0, idleClips.Count - 1);
                idleClips[index].Play();
            }
            else if (idleClips.Count == 1)
            {
                idleClips[0].Play();
            }
        }
    }

    public void PlaySpellClips()
    {
        if (spellClips != null)
        {
            if(spellClips.Count > 1)
            {
                int index = Random.Range(0, spellClips.Count - 1);
                spellClips[index].Play();
            }
            else if (spellClips.Count == 1)
            {
                spellClips[0].Play();
            }
        }
    }

    public void PlayTakeHitClips()
    {
        if (injuryClips != null)
        {
            if(injuryClips.Count > 1)
            {
                int index = Random.Range(0, injuryClips.Count - 1);
                injuryClips[index].Play();
            }
            else if (injuryClips.Count == 1)
            {
                injuryClips[0].Play();
            }
        }
    }

    public void PlayDeathClips()
    {
        if (deathClips != null)
        {
            if(deathClips.Count > 1)
            {
                int index = Random.Range(0, deathClips.Count - 1);
                deathClips[index].Play();
            }
            else if (deathClips.Count == 1)
            {
                deathClips[0].Play();
            }
        }
        
    }
}
