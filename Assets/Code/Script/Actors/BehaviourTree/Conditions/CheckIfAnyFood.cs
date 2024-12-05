using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class CheckIfAnyFood : BTCondition
{
    Animal animal;
    // Start is called before the first frame update
    public CheckIfAnyFood(Animal thisAnimal)
    {
        animal = thisAnimal;
    }

    //Returns true if it's night time
    protected override NodeState OnRun()
    {
        if (animal.GetHasFood())
        {
            //Debug.Log("There was food");
            return NodeState.SUCCESS;
        }
        else
        {
            //Debug.Log("There was no food");
            return NodeState.FAILURE;
        }
    }

    protected override void OnReset() { }
}
