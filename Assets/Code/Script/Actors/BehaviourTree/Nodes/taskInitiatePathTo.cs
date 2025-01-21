using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class taskInitiatePathTo : BTNode
{
    private GameObject thisTarget;
    Actor thisActor;
    NavMeshAgent thisAgent;

    public taskInitiatePathTo(Actor actor, GameObject targetLocation)
    {
        thisActor = actor;
        thisTarget = targetLocation;
        thisAgent = thisActor.GetComponent<NavMeshAgent>();   
    }


    protected override NodeState OnRun()
    {
        if (thisActor.GetComponent<Animal>())
        {
            if (thisActor.GetComponent<Animal>().GetIsKidnapped())
            {
                state = NodeState.FAILURE;
                return state;
            }
        }

        thisActor.SetActiveWaypoint(thisTarget);
        state = NodeState.SUCCESS;

        return state;
    }


    protected override void OnReset()
    {

    }
}
