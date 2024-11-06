using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OurAnimator : MonoBehaviour
{
    public Animator animator;

    [Tooltip("Setting this to visible for playtesting")]
    [SerializeField] private CharacterAnimation currentAnimation;

    //Dictionaries
    [Tooltip("Setting this to visible for playtesting")]
    [SerializeField] protected Dictionary<string, bool> animationFlags;
    protected Dictionary<string, CharacterAnimation> animations;

    protected bool inMovingAnimation = false;
    protected bool inSoftLock = false;

    [SerializeField] protected CharacterAnimation idleAnimation;
    [SerializeField] protected CharacterAnimation moveAnimation;

    public void PlayAnimation(string stateName)
    {
        currentAnimation = animations[stateName];
        animator.CrossFadeInFixedTime(GetAnimationName(stateName), animations[stateName].GetAnimationBlendTime());
    }

    /*
    public void RewindAn()
    {
        animator.CrossFadeInFixedTime(GetAnimationName("noAnimation"), 0.1f);
    }
    */

    //Turns an animation flag on or off
    public void SetAnimationFlag(string animationType, bool toSetTo)
    {
        if (animationFlags.ContainsKey(animationType))
            animationFlags[animationType] = toSetTo;
        else
            Debug.Log(animationType + " doesn't exist.");
    }

    //Get the current state of an animation flag
    public bool GetAnimationFlag(string animationType)
    {
        return animationFlags[animationType];
    }

    //Get the name of a particular animation as it's listed in the Animation Controller
    public string GetAnimationName(string name)
    {
        if (animations.ContainsKey(name))
            return animations[name].GetAnimationName();
        else
        {
            Debug.Log("Animation " + name + " doesn't exist");
            return "";
        }
    }

    public float GetAnimationLength(string name)
    {
        return animations[name].GetAnimationLength();
    }

    public WaitForSeconds GetAnimationWaitTime(string name)
    {
        return animations[name].GetAnimationWaitTime();
    }

    public CharacterAnimation GetCurrentAnimation()
    {
        return currentAnimation;
    }

    public void SetInSoftLock(bool softLock)
    {
        if (softLock)
            animationFlags[currentAnimation.GetAnimationFlag()] = false;
        inSoftLock = softLock;
    }

    //Gets whether or not the actor is optionally letting an animation play out
    public bool GetInSoftLock()
    {
        return inSoftLock;
    }
}
