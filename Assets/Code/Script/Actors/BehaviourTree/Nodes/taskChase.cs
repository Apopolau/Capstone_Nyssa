using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class taskChase : BTNode
{
    NavMeshAgent thisAgent;
    private KidnappingEnemy thisEnemy;

    public taskChase(KidnappingEnemy enemy)
    {
        thisEnemy = enemy;
        thisAgent = thisEnemy.GetComponent<NavMeshAgent>();
    }


    protected override NodeState OnRun()
    {
        //float distance = Vector3.Distance(thisEnemy.GetClosestPlayer().transform.position, thisAgent.transform.position);

        if (thisEnemy.GetIsStaggered() || thisEnemy.GetIsDying())
        {
            thisEnemy.ResetAgentPath();
            state = NodeState.FAILURE;
            return state;
        }

        if (!thisEnemy.GetInAttackRange() && thisEnemy.GetSeesPlayer())
        {
            thisEnemy.SetAgentPath(thisEnemy.GetClosestPlayer().transform.position);
            thisEnemy.transform.LookAt(thisEnemy.GetClosestPlayer().transform.position);

            state = NodeState.RUNNING;
        }

        else if (thisEnemy.GetInAttackRange() || !thisEnemy.GetSeesPlayer())
        {
            state = NodeState.FAILURE;
        }

        return state;
    }


    protected override void OnReset()
    {

    }
}
