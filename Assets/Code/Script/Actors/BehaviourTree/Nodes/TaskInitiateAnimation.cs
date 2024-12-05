using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class TaskInitiateAnimation : BTNode
{
    OurAnimator animator;
    string animationName;

    public TaskInitiateAnimation(OurAnimator animator, string animation)
    {
        this.animator = animator;
        animationName = animation;
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

        state = NodeState.SUCCESS;
        return state;
    }



    protected override void OnReset()
    {

    }
}
