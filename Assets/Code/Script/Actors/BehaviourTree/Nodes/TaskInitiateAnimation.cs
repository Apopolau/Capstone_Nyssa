using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class TaskInitiateAnimation : BTNode
{
    OurAnimator animator;
    string animationName;
    //Animator thisAnimator;
    //int animationHashValue;
    //List<int> animationsToStop;

    public TaskInitiateAnimation(OurAnimator animator, string animation)
    {
        this.animator = animator;
        animationName = animation;
        //animationHashValue = animationHash;
        //animationsToStop = incAnimationsToStop;
    }

    //Restores a stat like hunger or life to full
    protected override NodeState OnRun()
    {
        if(animator != null)
        {
            animator.SetAnimationFlag(animationName, true);
        }
        else
        {
            state = NodeState.FAILURE;
            return state;
        }
        //thisAnimator.SetBool(animationHashValue, true);
        /*
        if(animationsToStop.Count > 0)
        {
            foreach (int i in animationsToStop)
            {
                thisAnimator.SetBool(i, false);
            }
        }
        */

        state = NodeState.SUCCESS;
        return state;
    }



    protected override void OnReset()
    {

    }
}
