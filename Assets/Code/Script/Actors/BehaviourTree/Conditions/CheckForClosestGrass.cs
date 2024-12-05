using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

//This check condition checks if the object in question is near to any objects in a list
public class CheckForClosestGrass : BTCondition
{
    Animal thisAnimal;
    //GameObject storageObject;
    GameObjectRuntimeSet checkObjectSet;
    float distanceRange;
    bool foundObjectInRange;

    public CheckForClosestGrass(Animal animal, GameObjectRuntimeSet entitiesCheckedAgainst, float range)
    {
        thisAnimal = animal;
        checkObjectSet = entitiesCheckedAgainst;
        distanceRange = range;
    }

    //Should r
    protected override NodeState OnRun()
    {
        foundObjectInRange = false;
        if (checkObjectSet.Items != null)
        {
            foreach (GameObject g in checkObjectSet.Items)
            {
                float objectRange = (thisAnimal.transform.position - g.transform.position).magnitude;
                if (Mathf.Abs(objectRange) <= distanceRange)
                {
                    distanceRange = objectRange;
                    thisAnimal.SetClosestGrass(g);
                    foundObjectInRange = true;
                }
            }
            if (foundObjectInRange)
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
