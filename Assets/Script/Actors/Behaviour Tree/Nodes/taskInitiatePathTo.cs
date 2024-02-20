using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class taskInitiatePathTo : BTNode
{
    private Transform thisTarget;
    NavMeshAgent thisAgent;
    OurAnimator thisAnimator;

    public taskInitiatePathTo(NavMeshAgent navAgent, Transform target, OurAnimator animator)
    {
        thisAgent = navAgent;
        thisTarget = target;
        thisAnimator = animator;   
    }


    protected override NodeState OnRun()
    {
        bool inRange = Mathf.Abs((thisAgent.transform.position - thisTarget.position).magnitude) <= thisAgent.stoppingDistance;

        //If we're not at the destination but we can reach it
        if (!inRange && thisAgent.path.status != NavMeshPathStatus.PathInvalid)
        {
            thisAgent.SetDestination(thisTarget.position);
            thisAnimator.ToggleSetWalk();
            state = NodeState.RUNNING;
        }
        //If we've reached the destination
        else if (inRange)
        {
            thisAnimator.ToggleSetWalk();
            state = NodeState.SUCCESS;
        }
        //If we can't reach the destination
        else if(thisAgent.path.status == NavMeshPathStatus.PathInvalid)
        {
            thisAnimator.ToggleSetWalk();
            state = NodeState.FAILURE;
        }
        return state;
    }


    protected override void OnReset()
    {

    }
}
