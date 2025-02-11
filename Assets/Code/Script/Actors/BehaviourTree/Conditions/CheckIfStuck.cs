using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class CheckIfStuck : BTCondition
{
    Actor actor;

    public CheckIfStuck(Actor actor)
    {
        this.actor = actor;
    }

    //Returns true if it's night time
    protected override NodeState OnRun()
    {
        if(actor.GetComponent<Animal>() != null)
        {
            if (actor.GetComponent<Animal>().GetIsStuck())
            {
                return NodeState.SUCCESS;
            }
            else
            {
                return NodeState.FAILURE;
            }
        }
        else if(actor.GetComponent<KidnappingEnemy>() != null)
        {
            if (actor.GetComponent<KidnappingEnemy>().GetClosestAnimal().GetComponent<Animal>().GetIsStuck())
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