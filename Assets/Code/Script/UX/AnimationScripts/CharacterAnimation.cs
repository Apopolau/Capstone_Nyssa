using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterAnimation", menuName = "Data Type/CharacterAnimation")]
public class CharacterAnimation : ScriptableObject
{
    [SerializeField] string animationName;
    [SerializeField] string flagName;
    [SerializeField] AnimationClip animationClip;
    [SerializeField] float animationLength;
    [SerializeField] WaitForSeconds animationTime;

    private void OnEnable()
    {
        animationLength = animationClip.length;
        animationTime = new WaitForSeconds(animationLength);
    }

    public string GetAnimationName()
    {
        return animationName;
    }

    public AnimationClip GetAnimationClip()
    {
        return animationClip;
    }

    public float GetAnimationLength()
    {
        if(animationLength == 0)
            animationLength = animationClip.length;
        return animationLength;
    }

    public WaitForSeconds GetAnimationWaitTime()
    {
        return animationTime;
    }
}
