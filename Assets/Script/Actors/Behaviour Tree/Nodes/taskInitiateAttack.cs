using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class taskInitiateAttack : BTNode
{
    NavMeshAgent thisAgent;
    Enemy thisEnemy;
    private Transform thisTarget;

    public taskInitiateAttack(Transform target, NavMeshAgent enemyMeshAgent, CelestialPlayer player)
    {
        thisAgent = enemyMeshAgent;
        thisTarget = target;
        thisEnemy = enemyMeshAgent.GetComponent<Enemy>();
    }

    protected override NodeState OnRun()
    {

        float distance = Vector3.Distance(thisTarget.position, thisAgent.transform.position);

        if (!thisEnemy.attackInitiated)
        {
            if (distance <= 10f)
            {
                // Debug.Log(distance);
                thisEnemy.inAttackRange = true;
            }
            else
            {
                thisEnemy.inAttackRange = false;
                thisEnemy.enemyAnimator.animator.SetBool(thisEnemy.enemyAnimator.IfAttackingHash, false);
            }

            if (thisEnemy.inAttackRange && !thisEnemy.isDying && !thisEnemy.isStaggered)
            {
                thisEnemy.attackInitiated = true;
                thisEnemy.enemyAnimator.animator.SetBool(thisEnemy.enemyAnimator.IfAttackingHash, true);
                state = NodeState.SUCCESS;
                Debug.Log("am attacking");
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
