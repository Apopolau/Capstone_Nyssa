using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class CheckIfStaggered : BTCondition
{
    Enemy thisEnemy;

    public CheckIfStaggered(Enemy enemy)
    {
        thisEnemy = enemy;
    }

    //Returns true if it's night time
    protected override NodeState OnRun()
    {
        if (thisEnemy.GetIsStaggered())
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
