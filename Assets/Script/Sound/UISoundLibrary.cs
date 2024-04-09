using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SoundLibraries/UILibrary", fileName = "UI Sounds")]
public class UISoundLibrary : EventSoundLibrary
{
    [Header("Click")]
    public List<InstanceSoundEvent> clickClips;

    [Header("Back")]
    public List<InstanceSoundEvent> backClips;

    [Header("Submit")]
    public List<InstanceSoundEvent> submitClips;

    [Header("Error")]
    public List<InstanceSoundEvent> errorClips;

    [Header("Progress")]
    public List<InstanceSoundEvent> progressClips;


    public void PlayClickClips()
    {
        if (clickClips != null)
        {
            if(clickClips.Count > 1)
            {
                int index = Random.Range(0, clickClips.Count - 1);
                clickClips[index].Play();
            }
            else if (clickClips.Count == 1)
            {
                clickClips[0].Play();
            }
        }
    }

    public void PlayBackClips()
    {
        if (backClips != null)
        {
            if(backClips.Count > 1)
            {
                int index = Random.Range(0, backClips.Count - 1);
                backClips[index].Play();
            }
            else if (backClips.Count == 1)
            {
                backClips[0].Play();
            }
        }
    }

    public void PlaySubmitClips()
    {
        if (submitClips != null)
        {
            if(submitClips.Count > 1)
            {
                int index = Random.Range(0, submitClips.Count - 1);
                submitClips[index].Play();
            }
            else if (submitClips.Count == 1)
            {
                submitClips[0].Play();
            }
        }
    }

    public void PlayErrorClips()
    {
        if (errorClips != null)
        {
            if(errorClips.Count > 1)
            {
                int index = Random.Range(0, errorClips.Count - 1);
                errorClips[index].Play();
            }
            else if (errorClips.Count == 1)
            {
                errorClips[0].Play();
            }
        }
    }

    public void PlayProgressClips()
    {
        if (progressClips != null)
        {
            if(progressClips.Count > 1)
            {
                int index = Random.Range(0, progressClips.Count - 1);
                progressClips[index].Play();
            }
            else if (progressClips.Count == 1)
            {
                progressClips[0].Play();
            }
        }
    }
}
