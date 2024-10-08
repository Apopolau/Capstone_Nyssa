using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
public class CheckInAttackRange : BTCondition
{
    KidnappingEnemy thisEnemy;

    public CheckInAttackRange(KidnappingEnemy enemy)
    {
        thisEnemy = enemy;
    }

    protected override NodeState OnRun()
    {
        //float distance = Vector3.Distance(thisTarget.position, thisAgent.transform.position);
        if (thisEnemy.GetInAttackRange())
        {
            //place enemy animation here
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }

    protected override void OnReset() { }
}
