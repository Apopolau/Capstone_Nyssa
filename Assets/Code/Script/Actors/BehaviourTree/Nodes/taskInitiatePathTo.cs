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
        //bool inRange = Mathf.Abs((thisActor.transform.position - thisTarget.transform.position).magnitude) <= thisAgent.stoppingDistance;

        //If we're not at the destination but we can reach it
        //if (!inRange)
        //{
            //Debug.Log("Heading to " + thisTarget);
            thisActor.SetActiveWaypoint(thisTarget);
            state = NodeState.SUCCESS;

        
        //}
        //If we've reached the destination
        /*
        else if (inRange)
        {
            Debug.Log("No need to go to " + thisTarget + ", we're already here.");
            thisActor.ResetAgentPath();
            state = NodeState.FAILURE;
        }
        */

        return state;
    }


    protected override void OnReset()
    {

    }
}
