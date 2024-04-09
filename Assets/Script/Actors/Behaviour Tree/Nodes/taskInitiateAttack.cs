using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class taskInitiateAttack : BTNode
{
    NavMeshAgent thisAgent;
    Enemy thisEnemy;

    public taskInitiateAttack(Enemy enemy)
    {
        thisEnemy = enemy;
        thisAgent = enemy.GetComponent<NavMeshAgent>();
    }

    protected override NodeState OnRun()
    {

        float distance = Vector3.Distance(thisEnemy.GetClosestPlayer().transform.position, thisAgent.transform.position);

        if (!thisEnemy.attackInitiated)
        {
            if (thisEnemy.inAttackRange && !thisEnemy.isDying && !thisEnemy.isStaggered)
            {
                thisEnemy.attackInitiated = true;
                thisEnemy.enemyAnimator.animator.SetBool(thisEnemy.enemyAnimator.IfAttackingHash, true);
                state = NodeState.SUCCESS;
                Debug.Log("enemy attacking");
                thisEnemy.soundLibrary.PlayAttackClips();
            }
            else
            {
                thisEnemy.enemyAnimator.animator.SetBool(thisEnemy.enemyAnimator.IfAttackingHash, false);
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
