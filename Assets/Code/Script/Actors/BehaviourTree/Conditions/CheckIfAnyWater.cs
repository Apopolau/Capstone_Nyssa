using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
public class CheckIfAnyWater : BTCondition
{
    Animal animal;
    // Start is called before the first frame update
    public CheckIfAnyWater(Animal thisAnimal)
    {
        animal = thisAnimal;
    }

    //Returns true if it's night time
    protected override NodeState OnRun()
    {
        if (animal.GetHasWater())
        {
            //Debug.Log("There is water!");
            return NodeState.SUCCESS;
        }
        else
        {
            //Debug.Log("There is no water...");
            return NodeState.FAILURE;
        }
    }

    protected override void OnReset() { }
}
