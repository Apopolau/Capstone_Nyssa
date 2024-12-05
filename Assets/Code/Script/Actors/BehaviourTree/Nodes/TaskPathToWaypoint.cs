using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
public class TaskPathToWaypoint : BTNode
{
    NavMeshAgent thisAgent;
    private Actor thisActor;

    public TaskPathToWaypoint(Actor actor)
    {
        thisActor = actor;
        thisAgent = thisActor.GetComponent<NavMeshAgent>();
    }

    protected override NodeState OnRun()
    {
        float distance = (thisActor.transform.position - thisActor.GetActiveWaypoint().transform.position).magnitude;

        if(distance <= thisAgent.stoppingDistance + 1)
        {
            thisActor.ResetAgentPath();
            state = NodeState.SUCCESS;
        }
        else if(thisAgent.path == null)
        {
            thisActor.ResetAgentPath();
            state = NodeState.FAILURE;
        }
        else
        {
            state = NodeState.RUNNING;
        }

        //Debug.Log("Pathing to waypoint, status " + state);
        return state;
    }
    protected override void OnReset()
    {

    }

}

