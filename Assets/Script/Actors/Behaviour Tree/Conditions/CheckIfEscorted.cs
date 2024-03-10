using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class CheckIfEscorted : BTCondition
{
    Animal animal;

    public CheckIfEscorted(Animal animal)
    {
        this.animal = animal;
    }

    //Returns true if it's night time
    protected override NodeState OnRun()
    {
        if (animal.isEscorted)
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
