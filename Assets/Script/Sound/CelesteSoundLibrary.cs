using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SoundLibraries/CelesteLibrary", fileName = "Celeste Sounds")]
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
            int index = Random.Range(0, attackClips.Count);
            attackClips[index].Play();
        }

    }

    public void PlayLightningClips()
    {
        if (lightningClips != null)
        {
            int index = Random.Range(0, lightningClips.Count);
            lightningClips[index].Play();
        }

    }

    public void PlayFrostClips()
    {
        if (frostClips != null)
        {
            int index = Random.Range(0, frostClips.Count);
            frostClips[index].Play();
        }

    }

    public void PlayWaveClips()
    {
        if (waveClips != null)
        {
            int index = Random.Range(0, waveClips.Count);
            waveClips[index].Play();
        }

    }
}
