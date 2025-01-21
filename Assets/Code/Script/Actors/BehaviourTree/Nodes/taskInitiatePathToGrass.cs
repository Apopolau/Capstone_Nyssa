using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class taskInitiatePathToGrass : BTNode
{
    NavMeshAgent thisAgent;
    Animal thisAnimal;

    public taskInitiatePathToGrass(NavMeshAgent navAgent)
    {
        thisAgent = navAgent;
        thisAnimal = navAgent.GetComponent<Animal>();
    }


    protected override NodeState OnRun()
    {
        if (thisAnimal.GetIsKidnapped())
        {
            state = NodeState.FAILURE;
            return state;
        }

        bool inRange = Mathf.Abs((thisAgent.transform.position - thisAnimal.GetClosestGrass().transform.position).magnitude) <= thisAgent.stoppingDistance;

        //If we're not at the destination but we can reach it
        if (!inRange)
        {
            thisAnimal.SetActiveWaypoint(thisAnimal.GetClosestGrass());
            thisAnimal.SetAgentPath(thisAnimal.GetActiveWaypoint().transform.position);
            state = NodeState.SUCCESS;
        }
        //If we've reached the destination
        else if (inRange)
        {
            thisAnimal.ResetAgentPath();
            state = NodeState.FAILURE;
        }
        return state;
    }


    protected override void OnReset()
    {

    }
}
