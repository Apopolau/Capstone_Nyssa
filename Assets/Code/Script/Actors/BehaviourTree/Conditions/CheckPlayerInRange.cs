using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
public class CheckPlayerInRange : BTCondition
{
    KidnappingEnemy thisEnemy;

    public CheckPlayerInRange(KidnappingEnemy enemy)
    {
        thisEnemy = enemy;
    }

    protected override NodeState OnRun()
    {
        if (thisEnemy.GetSeesPlayer())
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
