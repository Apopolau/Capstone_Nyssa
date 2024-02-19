using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class CheckIfHungry : BTCondition
{
    Animal animal;
    // Start is called before the first frame update
    public CheckIfHungry(Animal thisAnimal)
    {
        animal = thisAnimal;
    }

    //Returns true if it's night time
    protected override NodeState OnRun()
    {
        if (animal.GetHungryState())
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
