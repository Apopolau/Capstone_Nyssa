using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

//This check condition checks if the object in question is near to any objects in a list
public class CheckIfInRangeOne : BTCondition
{
    GameObject checkingObject;
    GameObject checkIfClose;
    float distanceRange;
    

    // 
    public CheckIfInRangeOne(GameObject checkingEntity, GameObject entityToCheckAgainst, float range)
    {
        checkingObject = checkingEntity;
        checkIfClose = entityToCheckAgainst;
        distanceRange = range;
    }

    //Returns true if it's night time
    protected override NodeState OnRun()
    {
        bool objectInRange = false;
        float objectRange = (checkingObject.transform.position - checkIfClose.transform.position).magnitude;
        if (Mathf.Abs(objectRange) <= distanceRange)
        {
            objectInRange = true;
        }

        if (objectInRange)
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
