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
        if (thisActor.GetComponent<PlasticBagMonster>())
        {
            if (thisActor.GetComponent<PlasticBagMonster>().GetSeesPlant())
            {
                return NodeState.FAILURE;
            }
        }

        if (thisActor.GetComponent<Animal>())
        {
            if (thisActor.GetComponent<Animal>().GetIsKidnapped())
            {
                return NodeState.FAILURE;
            }
        }

        if(thisActor.GetActiveWaypoint() == null)
        {
            return NodeState.FAILURE;
        }

        float distance = (thisActor.transform.position - thisActor.GetActiveWaypoint().transform.position).magnitude;

        if (distance <= thisAgent.stoppingDistance + 1)
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

        return state;
    }
    protected override void OnReset()
    {

    }

}

