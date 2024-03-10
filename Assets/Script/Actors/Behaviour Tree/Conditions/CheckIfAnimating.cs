using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class CheckIfAnimating : BTCondition
{
    OurAnimator thisAnimator;

    public CheckIfAnimating(OurAnimator animator)
    {
        thisAnimator = animator;
    }

    //Returns true if it's night time
    protected override NodeState OnRun()
    {
        if (!thisAnimator.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !thisAnimator.animator.GetCurrentAnimatorStateInfo(0).IsName("Walking"))
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }

    protected override void OnReset() { }
}
