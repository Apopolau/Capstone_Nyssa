using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

//This check condition checks if the object in question is near to any objects in a list
public class CheckForClosestFood : BTCondition
{
    Animal thisAnimal;
    float distanceRange;
    float newRange;
    bool foundFoodInRange;

    public CheckForClosestFood(Animal animal, float range)
    {
        thisAnimal = animal;
        distanceRange = range;
        newRange = distanceRange;
    }

    //Should r
    protected override NodeState OnRun()
    {
        foundFoodInRange = false;
        newRange = distanceRange;

        if (thisAnimal.foodSet.Items != null)
        {
            foreach (GameObject g in thisAnimal.foodSet.Items)
            {
                float objectRange = (thisAnimal.transform.position - g.transform.position).magnitude;
                if (Mathf.Abs(objectRange) <= distanceRange || Mathf.Abs(objectRange) <= newRange)
                {
                    newRange = objectRange;
                    thisAnimal.SetClosestFood(g);
                    foundFoodInRange = true;
                }
            }
            if (foundFoodInRange)
            {
                return NodeState.SUCCESS;
            }
            else
            {
                return NodeState.FAILURE;
            }
        }
        else
        {
            return NodeState.FAILURE;
        }


    }

    protected override void OnReset() { }
}
