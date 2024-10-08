using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Sound Libraries/Celeste Library", fileName = "Celeste Sounds")]
public class CelesteSoundLibrary : PlayerSoundLibrary
{
    [Header("Attack")]
    public List<InstanceSoundEvent> attackClips;

    [Header("Lightning")]
    public List<InstanceSoundEvent> lightningClips;

    [Header("Frost")]
    public List<InstanceSoundEvent> frostClips;

    [Header("Wave")]
    public List<InstanceSoundEvent> waveClips;


    public void PlayAttackClips()
    {
        if (attackClips != null)
        {
            if(attackClips.Count > 1)
            {
                int index = Random.Range(0, attackClips.Count - 1);
                attackClips[index].Play();
            }
            else if (attackClips.Count == 1)
            {
                attackClips[0].Play();
            }
        }
    }

    public void PlayLightningClips()
    {
        if (lightningClips != null)
        {
            if(lightningClips.Count > 1)
            {
                int index = Random.Range(0, lightningClips.Count - 1);
                lightningClips[index].Play();
            }
            else if (lightningClips.Count == 1)
            {
                lightningClips[0].Play();
            }
        }
    }

    public void PlayFrostClips()
    {
        if (frostClips != null)
        {
            if(frostClips.Count > 1)
            {
                int index = Random.Range(0, frostClips.Count - 1);
                frostClips[index].Play();
            }
            else if (frostClips.Count == 1)
            {
                frostClips[0].Play();
            }
        }
    }

    public void PlayWaveClips()
    {
        if (waveClips != null)
        {
            if(waveClips.Count > 1)
            {
                int index = Random.Range(0, waveClips.Count - 1);
                waveClips[index].Play();
            }
            else if (waveClips.Count == 1)
            {
                waveClips[0].Play();
            }
        }
    }
}
