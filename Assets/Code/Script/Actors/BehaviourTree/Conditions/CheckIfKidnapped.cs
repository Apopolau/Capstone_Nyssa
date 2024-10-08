
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
public class CheckIfKidnapped : BTCondition
{
    Animal thisAnimal;

    public CheckIfKidnapped(Animal animal)
    {
        thisAnimal = animal;
        
    }

    protected override NodeState OnRun()
    {
        //float distance = Vector3.Distance(thisTarget.position, thisAgent.transform.position);
        if (thisAnimal.GetIsKidnapped())
        {
            //place enemy animation here
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }

    protected override void OnReset() { }
}