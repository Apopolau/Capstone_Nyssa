using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class CheckIfBored : BTCondition
{
    Animal animal;

    public CheckIfBored(Animal thisAnimal)
    {
        animal = thisAnimal;
    }

    //Returns true if it's night time
    protected override NodeState OnRun()
    {
        //Debug.Log("Checking if " + animal + " is bored");
        if (animal.GetBoredState())
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }

    protected override void OnReset() { }
}
