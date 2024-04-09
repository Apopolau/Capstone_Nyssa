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
            int index = Random.Range(0, clickClips.Count);
            clickClips[index].Play();
        }

    }

    public void PlayBackClips()
    {
        if (backClips != null)
        {
            int index = Random.Range(0, backClips.Count);
            backClips[index].Play();
        }

    }

    public void PlaySubmitClips()
    {
        if (submitClips != null)
        {
            int index = Random.Range(0, submitClips.Count);
            submitClips[index].Play();
        }

    }

    public void PlayErrorClips()
    {
        if (errorClips != null)
        {
            int index = Random.Range(0, errorClips.Count);
            errorClips[index].Play();
        }

    }

    public void PlayProgressClips()
    {
        if (progressClips != null)
        {
            int index = Random.Range(0, progressClips.Count);
            progressClips[index].Play();
        }

    }
}
