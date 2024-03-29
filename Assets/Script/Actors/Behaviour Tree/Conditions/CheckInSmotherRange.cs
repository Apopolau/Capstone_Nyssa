using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class CheckInSmotherRange : BTCondition
{
    Enemy thisEnemy;

    public CheckInSmotherRange(Enemy enemy)
    {
        thisEnemy = enemy;
    }

    protected override NodeState OnRun()
    {
        //float distance = Vector3.Distance(thisTarget.position, thisAgent.transform.position);
        if (thisEnemy.inSmotherRange)
        {
            Debug.Log("Check in smother range");
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
