using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

//This check condition checks if the object in question is near to any objects in a list
public class CheckForClosestPlayer : BTCondition
{
    Animal thisAnimal;
    KidnappingEnemy thisEnemy;
    float distanceRange;
    float newRange;
    bool foundPlayerInRange;

    public CheckForClosestPlayer(Animal animal, float range)
    {
        thisAnimal = animal;
        distanceRange = range;
        newRange = distanceRange;
    }

    public CheckForClosestPlayer(KidnappingEnemy enemy, float range)
    {
        thisEnemy = enemy;
        distanceRange = range;
        newRange = distanceRange;
    }

    //Should r
    protected override NodeState OnRun()
    {
        foundPlayerInRange = false;
        newRange = distanceRange;
        if(thisAnimal != null)
        {
            if (thisAnimal.GetPlayerSet().Items != null)
            {
                foreach (GameObject g in thisAnimal.GetPlayerSet().Items)
                {
                    float objectRange = (thisAnimal.transform.position - g.transform.position).magnitude;
                    if (Mathf.Abs(objectRange) <= distanceRange || Mathf.Abs(objectRange) <= newRange)
                    {
                        newRange = objectRange;
                        thisAnimal.SetClosestPlayer(g);
                        foundPlayerInRange = true;
                    }
                }
                if (foundPlayerInRange)
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
        else if(thisEnemy != null)
        {
            if (thisEnemy.GetPlayerSet().Items != null)
            {
                foreach (GameObject g in thisEnemy.GetPlayerSet().Items)
                {
                    float objectRange = (thisEnemy.transform.position - g.transform.position).magnitude;
                    if (Mathf.Abs(objectRange) <= distanceRange || Mathf.Abs(objectRange) <= newRange)
                    {
                        newRange = objectRange;
                        thisEnemy.SetClosestPlayer(g);
                        foundPlayerInRange = true;
                    }
                }
                if (foundPlayerInRange)
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
        else
        {
            return NodeState.FAILURE;
        }
        
    }

    protected override void OnReset() { }
}
