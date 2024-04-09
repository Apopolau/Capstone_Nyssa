using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SoundLibraries/SproutLibrary", fileName = "Sprout Sounds")]
public class SproutSoundLibrary : PlayerSoundLibrary
{
    

    [Header("Plant")]
    public List<InstanceSoundEvent> plantClips;

    

    [Header("Build")]
    public List<InstanceSoundEvent> buildClips;

    public void PlayPlantClips()
    {
        if (plantClips != null)
        {
            int index = Random.Range(0, plantClips.Count);
            plantClips[index].Play();
        }

    }

    

    public void PlayBuildClips()
    {
        if (buildClips != null)
        {
            int index = Random.Range(0, buildClips.Count);
            buildClips[index].Play();
        }

    }

    
}
