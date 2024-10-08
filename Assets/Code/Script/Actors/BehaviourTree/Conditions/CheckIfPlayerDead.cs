using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class CheckIfPlayerDead : BTCondition
{
    KidnappingEnemy thisEnemy;

    public CheckIfPlayerDead(KidnappingEnemy enemy)
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
