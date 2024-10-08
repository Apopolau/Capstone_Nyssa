using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Sound Libraries/Sprout Library", fileName = "Sprout Sounds")]
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
            if(plantClips.Count > 1)
            {
                int index = Random.Range(0, plantClips.Count - 1);
                plantClips[index].Play();
            }
            else if (plantClips.Count == 1)
            {
                plantClips[0].Play();
            }
        }
    }

    

    public void PlayBuildClips()
    {
        if (buildClips != null)
        {
            if(buildClips.Count > 1)
            {
                int index = Random.Range(0, buildClips.Count - 1);
                buildClips[index].Play();
            }
            else if (buildClips.Count == 1)
            {
                buildClips[0].Play();
            }
        }
    }

    
}
