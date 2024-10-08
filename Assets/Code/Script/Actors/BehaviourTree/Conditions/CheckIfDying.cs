using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class CheckIfDying : BTCondition
{
    Enemy thisEnemy;

    public CheckIfDying(Enemy enemy)
    {
        thisEnemy = enemy;
    }

    //Returns true if it's night time
    protected override NodeState OnRun()
    {
        if (thisEnemy.GetIsDying())
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
