using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

//This check condition checks if the object in question is near to any objects in a list
public class CheckForClosest : BTCondition
{
    GameObject checkingObject;
    GameObject storageObject;
    GameObjectRuntimeSet checkObjectSet;
    float distanceRange;
    bool foundObjectInRange;

    public CheckForClosest(GameObject checkingEntity, ref GameObject storageObject, GameObjectRuntimeSet entitiesCheckedAgainst, float range)
    {
        checkingObject = checkingEntity;
        checkObjectSet = entitiesCheckedAgainst;
        distanceRange = range;
        this.storageObject = storageObject;
    }

    //Should r
    protected override NodeState OnRun()
    {
        foundObjectInRange = false;
        if (checkObjectSet.Items != null)
        {
            foreach (GameObject g in checkObjectSet.Items)
            {
                float objectRange = (checkingObject.transform.position - g.transform.position).magnitude;
                if (Mathf.Abs(objectRange) <= distanceRange)
                {
                    distanceRange = objectRange;
                    storageObject = g;
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
