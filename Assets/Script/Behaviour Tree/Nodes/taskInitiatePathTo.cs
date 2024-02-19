using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class taskInitiatePathTo : BTNode
{
    private Transform thisTarget;
    NavMeshAgent thisAgent;

    public taskInitiatePathTo(NavMeshAgent navAgent, Transform target)
    {
        thisAgent = navAgent;
        thisTarget = target;
    }


    protected override NodeState OnRun()
    {
        bool inRange = Mathf.Abs((thisAgent.transform.position - thisTarget.position).magnitude) <= thisAgent.stoppingDistance;

        //If we're not at the destination but we can reach it
        if (!inRange && thisAgent.path.status != NavMeshPathStatus.PathInvalid)
        {
            thisAgent.SetDestination(thisTarget.position);
            state = NodeState.RUNNING;
        }
        //If we've reached the destination
        else if (inRange)
        {
            state = NodeState.SUCCESS;
        }
        //If we can't reach the destination
        else if(thisAgent.path.status == NavMeshPathStatus.PathInvalid)
        {
            state = NodeState.FAILURE;
        }
        return state;
    }


    protected override void OnReset()
    {

    }
}
