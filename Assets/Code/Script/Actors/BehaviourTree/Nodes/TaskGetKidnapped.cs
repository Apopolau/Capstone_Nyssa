using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;


public class TaskGetKidnapped: BTNode
{
    Animal thisAnimal;
    NavMeshAgent thisAgent;

    public  TaskGetKidnapped (Animal animal)
    {
        thisAnimal = animal;
        thisAgent = thisAnimal.GetNavMeshAgent();
    }
    protected override NodeState OnRun()
    { 
        if (!thisAnimal.GetIsKidnapped())
        {
            state = NodeState.FAILURE;
            return state;
        }

        if (thisAnimal.GetIsKidnapped())
        {
            thisAgent.ResetPath();
            state = NodeState.RUNNING;
        }

        return state;
    }

    protected override void OnReset()
    {
    }
}
