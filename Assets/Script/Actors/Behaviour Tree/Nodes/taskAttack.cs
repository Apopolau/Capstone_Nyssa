using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
public class TaskAttack : BTNode
{
    NavMeshAgent thisAgent;
    Enemy thisEnemy;


    public TaskAttack(Enemy enemy)
    {
        thisEnemy = enemy;
        thisAgent = enemy.GetComponent<NavMeshAgent>();
    }

    protected override NodeState OnRun()
    {

        float distance = Vector3.Distance(thisEnemy.GetClosestPlayer().transform.position, thisAgent.transform.position);

        if(thisEnemy.isStaggered || thisEnemy.isDying)
        {
            thisEnemy.enemyAnimator.animator.SetBool(thisEnemy.enemyAnimator.IfAttackingHash, false);
            state = NodeState.FAILURE;
            return state;
        }

        if (!thisEnemy.inAttackRange)
        {
            thisEnemy.enemyAnimator.animator.SetBool(thisEnemy.enemyAnimator.IfAttackingHash, false);
        }

        if (thisEnemy.inAttackRange && thisEnemy.attackInitiated)
        {
            //yield return attackTime;
            bool playerIsDead = false;
            //I refactored this a little, you may want to set the damage they can deal in their stats and pass it in here
            if (!thisEnemy.GetClosestPlayer().GetComponentInParent<Player>().GetShielded())
            {
                playerIsDead = thisEnemy.GetClosestPlayer().GetComponentInParent<Player>().TakeHit(10);
            }
            else
            {
                thisEnemy.TakeHit(25);
            }
            
            thisEnemy.transform.LookAt(thisEnemy.GetClosestPlayer().transform.position);
            if (playerIsDead)
            {
                state = NodeState.FAILURE;
            }
            else
            {
                state = NodeState.SUCCESS;
            }
            thisEnemy.enemyAnimator.animator.SetBool(thisEnemy.enemyAnimator.IfAttackingHash, false);
        }
        else
        {
            state = NodeState.FAILURE;
        }
        thisEnemy.attackInitiated = false;
        return state;
    }

    

    protected override void OnReset()
    {
    }
}
