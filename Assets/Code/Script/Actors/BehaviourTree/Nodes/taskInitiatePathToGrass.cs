using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class taskInitiatePathToGrass : BTNode
{
    NavMeshAgent thisAgent;
    Animal animal;

    public taskInitiatePathToGrass(NavMeshAgent navAgent)
    {
        thisAgent = navAgent;
        animal = navAgent.GetComponent<Animal>();
    }


    protected override NodeState OnRun()
    {
        bool inRange = Mathf.Abs((thisAgent.transform.position - animal.GetClosestGrass().transform.position).magnitude) <= thisAgent.stoppingDistance;

        //If we're not at the destination but we can reach it
        if (!inRange)
        {
            animal.SetActiveWaypoint(animal.GetClosestGrass());
            animal.SetAgentPath(animal.GetActiveWaypoint().transform.position);
            state = NodeState.SUCCESS;
        }
        //If we've reached the destination
        else if (inRange)
        {
            animal.ResetAgentPath();
            state = NodeState.FAILURE;
        }
        return state;
    }


    protected override void OnReset()
    {

    }
}
