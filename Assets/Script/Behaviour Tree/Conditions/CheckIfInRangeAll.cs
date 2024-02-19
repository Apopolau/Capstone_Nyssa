using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

//This check condition checks if the object in question is near to any objects in a list
public class CheckIfInRangeAll : BTCondition
{
    GameObject checkingObject;
    GameObjectRuntimeSet checkObjectSet;
    float distanceRange;
    bool foundObjectInRange;

    // 
    public CheckIfInRangeAll(GameObject checkingEntity, GameObjectRuntimeSet entitiesCheckedAgainst, float range)
    {
        checkingObject = checkingEntity;
        checkObjectSet = entitiesCheckedAgainst;
        distanceRange = range;
    }

    //Returns true if it's night time
    protected override NodeState OnRun()
    {
        foreach(GameObject g in checkObjectSet.Items)
        {
            float objectRange = (checkingObject.transform.position - g.transform.position).magnitude;
            if(Mathf.Abs(objectRange) <= distanceRange)
            {
                foundObjectInRange = true;
                break;
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

    protected override void OnReset() { }
}
