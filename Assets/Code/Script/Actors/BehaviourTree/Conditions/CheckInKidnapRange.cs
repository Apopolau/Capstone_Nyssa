using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
public class CheckInKidnapRange : BTCondition
{
    KidnappingEnemy thisEnemy;

    public CheckInKidnapRange(KidnappingEnemy enemy)
    {
        thisEnemy = enemy;
    }

    protected override NodeState OnRun()
    {
        //float distance = Vector3.Distance(thisTarget.position, thisAgent.transform.position);
        if (thisEnemy.GetInKidnapRange())
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