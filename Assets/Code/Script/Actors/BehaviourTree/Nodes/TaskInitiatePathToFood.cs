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
        bool inRange = Mathf.Abs((thisAgent.transform.position - animal.GetClosestFood().transform.position).magnitude) <= thisAgent.stoppingDistance;

        //If we're not at the destination but we can reach it
        if (!inRange && thisAgent.path.status != NavMeshPathStatus.PathInvalid)
        {
            animal.SetAgentPath(animal.GetClosestFood().transform.position);
            //thisAnimator.ToggleSetWalk();
            animal.GetAnimator().SetAnimationFlag("move", true);
            state = NodeState.RUNNING;
        }
        //If we've reached the destination
        else if (inRange)
        {
            //thisAnimator.ToggleSetWalk();
            //animal.SetAnimationFlag("move", false);
            animal.ResetAgentPath();
            state = NodeState.SUCCESS;
        }
        //If we can't reach the destination
        else if (thisAgent.path.status == NavMeshPathStatus.PathInvalid)
        {
            //thisAnimator.ToggleSetWalk();
            //animal.SetAnimationFlag("move", false);
            animal.ResetAgentPath();
            state = NodeState.FAILURE;
        }
        return state;
    }


    protected override void OnReset()
    {

    }
}
