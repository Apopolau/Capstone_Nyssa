using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class CheckIfPlayerDead : BTCondition
{
    Enemy thisEnemy;

    public CheckIfPlayerDead(Enemy enemy)
    {
        thisEnemy = enemy;
    }

    protected override NodeState OnRun()
    {
        if (thisEnemy.GetClosestPlayer().GetComponent<Player>().IsDead())
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
