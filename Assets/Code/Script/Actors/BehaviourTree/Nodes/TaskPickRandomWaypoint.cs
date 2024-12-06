using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class TaskPickRandomWaypoint : BTNode
{
    Actor thisActor;
    NavMeshAgent thisAgent;

    public TaskPickRandomWaypoint(Actor actor)
    {
        thisActor = actor;
        thisAgent = thisActor.GetComponent<NavMeshAgent>();
    }


    protected override NodeState OnRun()
    {
        if (thisActor.GetComponent<Animal>())
        {
            if (thisActor.GetComponent<Animal>().GetIsKidnapped())
            {
                return NodeState.FAILURE;
            }
        }

        if (thisActor.GetWanderList().Count == 0)
        {
            return NodeState.FAILURE;
        }

        thisActor.SetActiveWaypoint(thisActor.GetRandomWaypoint());
        thisActor.SetAgentPath(thisActor.GetActiveWaypoint().transform.position);

        //If the function successfully found a new location to path to
        if (thisAgent.path.status != NavMeshPathStatus.PathInvalid)
        {
            state = NodeState.SUCCESS;
        }
        //If we can't reach the destination
        else if (thisAgent.path.status == NavMeshPathStatus.PathInvalid)
        {
            thisActor.ResetAgentPath();
            state = NodeState.RUNNING;
        }
        return state;
    }


    protected override void OnReset()
    {

    }
}
