using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;


public class TaskInitiateSmother : BTNode
{
    NavMeshAgent thisAgent;
    Enemy thisEnemy;

    public TaskInitiateSmother(Enemy enemy)
    {
        thisEnemy = enemy;
        thisAgent = enemy.GetComponent<NavMeshAgent>();
    }


    protected override NodeState OnRun()
    {

        //float distance = Vector3.Distance(thisEnemy.GetClosestPlant().transform.position, thisAgent.transform.position);

        if (!thisEnemy.smotherInitiated)
        {
            if (thisEnemy.inSmotherRange && !thisEnemy.isDying && !thisEnemy.isStaggered)
            {
                thisEnemy.smotherInitiated = true;
               //thisEnemy.enemyAnimator.animator.SetBool(thisEnemy.enemyAnimator.IfAttackingHash, true);
                state = NodeState.SUCCESS;
                Debug.Log("enemy is smothering");
            }
            else
            {
                //thisEnemy.enemyAnimator.animator.SetBool(thisEnemy.enemyAnimator.IfAttackingHash, false);
                state = NodeState.FAILURE;
            }
        }
        else
        {
            state = NodeState.RUNNING;
        }

        return state;
    }



    protected override void OnReset()
    {

    }
}
