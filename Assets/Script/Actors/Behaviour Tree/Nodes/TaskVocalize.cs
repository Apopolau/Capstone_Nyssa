using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class TaskVocalize : BTNode
{
    NavMeshAgent thisAgent;
    float playerRange;
    Animal thisAnimal;

    public TaskVocalize(NavMeshAgent thisAgent, Animal animal, float range)
    {
        this.thisAgent = thisAgent;
        playerRange = range;
        thisAnimal = animal;
    }


    protected override NodeState OnRun()
    {
        float distance = (thisAgent.transform.position - thisAnimal.GetClosestPlayer().transform.position).magnitude;
        if(distance <= playerRange)
        {
            thisAgent.GetComponent<Transform>().LookAt(thisAnimal.GetClosestPlayer().transform);
            //Do a popup here
            //Also do a sound, like a quack
            state = NodeState.RUNNING;
        }
        else
        {
            state = NodeState.FAILURE;
        }
        
        return state;
    }


    protected override void OnReset()
    {

    }
}
