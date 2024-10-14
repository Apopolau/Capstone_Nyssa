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
    [SerializeField] float blendSpeed;

    private void OnEnable()
    {
        animationLength = animationClip.length;
        animationTime = new WaitForSeconds(animationLength);
    }

    //Gets the name of the animation as it's listed in the Mecanim state machine/Animation Controller
    public string GetAnimationName()
    {
        return animationName;
    }

    //Gets the name of the animation flag
    public string GetAnimationFlag()
    {
        return flagName;
    }

    //Returns the actual animation file
    public AnimationClip GetAnimationClip()
    {
        return animationClip;
    }

    //Returns the length of the animation in seconds (float)
    public float GetAnimationLength()
    {
        if(animationLength == 0)
            animationLength = animationClip.length;
        return animationLength;
    }

    //Returns the WaitForSeconds animation length
    public WaitForSeconds GetAnimationWaitTime()
    {
        return animationTime;
    }

    //Get the time in seconds it takes for an animation to blend into another
    public float GetAnimationBlendTime()
    {
        return blendSpeed;
    }
}
