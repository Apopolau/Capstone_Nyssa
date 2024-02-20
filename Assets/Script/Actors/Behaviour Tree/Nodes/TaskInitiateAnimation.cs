using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class TaskInitiateAnimation : BTNode
{
    Animator thisAnimator;
    int animationHashValue;
    List<int> animationsToStop;

    public TaskInitiateAnimation(Animator animator, int animationHash, List<int> incAnimationsToStop)
    {
        thisAnimator = animator;
        animationHashValue = animationHash;
        animationsToStop = incAnimationsToStop;
    }

    //Restores a stat like hunger or life to full
    protected override NodeState OnRun()
    {
        thisAnimator.SetBool(animationHashValue, true);
        if(animationsToStop.Count > 0)
        {
            foreach (int i in animationsToStop)
            {
                thisAnimator.SetBool(i, false);
            }
        }
        
        state = NodeState.SUCCESS;
        return state;
    }



    protected override void OnReset()
    {

    }
}
