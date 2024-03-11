using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class taskInitiatePathToFood : BTNode
{
    NavMeshAgent thisAgent;
    OurAnimator thisAnimator;
    Animal animal;

    public taskInitiatePathToFood(NavMeshAgent navAgent, OurAnimator animator)
    {
        thisAgent = navAgent;
        thisAnimator = animator;
        animal = navAgent.GetComponent<Animal>();
    }


    protected override NodeState OnRun()
    {
        bool inRange = Mathf.Abs((thisAgent.transform.position - animal.closestFood.transform.position).magnitude) <= thisAgent.stoppingDistance;

        //If we're not at the destination but we can reach it
        if (!inRange && thisAgent.path.status != NavMeshPathStatus.PathInvalid)
        {
            thisAgent.SetDestination(animal.closestGrass.transform.position);
            thisAnimator.ToggleSetWalk();
            state = NodeState.RUNNING;
        }
        //If we've reached the destination
        else if (inRange)
        {
            thisAnimator.ToggleSetWalk();
            state = NodeState.SUCCESS;
        }
        //If we can't reach the destination
        else if (thisAgent.path.status == NavMeshPathStatus.PathInvalid)
        {
            thisAnimator.ToggleSetWalk();
            state = NodeState.FAILURE;
        }
        return state;
    }


    protected override void OnReset()
    {

    }
}
