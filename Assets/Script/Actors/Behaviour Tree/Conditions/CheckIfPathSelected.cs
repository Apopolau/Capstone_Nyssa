using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
public class CheckIfPathSelected : BTCondition
{
    Enemy thisEnemy;

    public CheckIfPathSelected(Enemy enemy)
    {
        thisEnemy = enemy;
    }

    //Returns true if it's night time
    protected override NodeState OnRun()
    {
        if (thisEnemy.isPathSelected)
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