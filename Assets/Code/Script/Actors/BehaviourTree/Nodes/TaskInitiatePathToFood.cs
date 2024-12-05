using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class taskInitiatePathToFood : BTNode
{
    NavMeshAgent thisAgent;
    Animal thisAnimal;

    public taskInitiatePathToFood(Animal animal)
    {
        thisAnimal = animal;
        thisAgent = thisAnimal.GetComponent<NavMeshAgent>();
    }


    protected override NodeState OnRun()
    {
        //bool inRange = Mathf.Abs((thisAgent.transform.position - thisAnimal.GetClosestFood().transform.position).magnitude) <= thisAgent.stoppingDistance;
        //Debug.Log(thisAnimal + " is seeking " + thisAnimal.GetClosestFood());

        //If we're not at the destination but we can reach it
        //if (!inRange)
        //{
            thisAnimal.SetActiveWaypoint(thisAnimal.GetClosestFood());
            thisAnimal.SetAgentPath(thisAnimal.GetActiveWaypoint().transform.position);
            state = NodeState.SUCCESS;
        //}
        //If we've reached the destination
        /*
        else if (inRange)
        {
            thisAnimal.ResetAgentPath();
            state = NodeState.FAILURE;
        }
        */
        return state;
    }


    protected override void OnReset()
    {

    }
}
