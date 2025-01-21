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
        if (thisAnimal.GetIsKidnapped())
        {
            state = NodeState.FAILURE;
            return state;
        }

        thisAnimal.SetActiveWaypoint(thisAnimal.GetClosestFood());
        thisAnimal.SetAgentPath(thisAnimal.GetActiveWaypoint().transform.position);

        if(thisAgent.pathStatus != NavMeshPathStatus.PathInvalid)
        {
            state = NodeState.SUCCESS;

            return state;
        }
        else
        {
            state = NodeState.FAILURE;
            return state;
        }
        
    }


    protected override void OnReset()
    {

    }
}
