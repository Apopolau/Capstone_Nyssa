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
        thisActor.SetActiveWaypoint(thisActor.GetRandomWaypoint());
        thisAgent.destination = thisActor.GetActiveWaypoint().transform.position;

        //If the function successfully found a new location to path to
        if (thisAgent.path.status != NavMeshPathStatus.PathInvalid)
        {
            state = NodeState.SUCCESS;
        }
        //If we can't reach the destination
        else if (thisAgent.path.status == NavMeshPathStatus.PathInvalid)
        {
            thisAgent.GetComponent<Actor>().ResetAgentPath();
            state = NodeState.RUNNING;
        }
        return state;
    }


    protected override void OnReset()
    {

    }
}
