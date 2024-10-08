using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
public class TaskAttack : BTNode
{
    NavMeshAgent thisAgent;
    KidnappingEnemy thisEnemy;


    public TaskAttack(KidnappingEnemy enemy)
    {
        thisEnemy = enemy;
        thisAgent = enemy.GetComponent<NavMeshAgent>();
    }

    protected override NodeState OnRun()
    {

        float distance = Vector3.Distance(thisEnemy.GetClosestPlayer().transform.position, thisAgent.transform.position);

        if(thisEnemy.GetIsStaggered() || thisEnemy.GetIsDying())
        {
            //thisEnemy.GetEnemyAnimator().animator.SetBool(thisEnemy.GetEnemyAnimator().IfAttackingHash, false);
            thisEnemy.GetAnimator().SetAnimationFlag("attack", false);
            state = NodeState.FAILURE;
            return state;
        }

        if (!thisEnemy.GetInAttackRange())
        {
            //thisEnemy.GetEnemyAnimator().animator.SetBool(thisEnemy.GetEnemyAnimator().IfAttackingHash, false);
            thisEnemy.GetAnimator().SetAnimationFlag("attack", true);
        }

        if (thisEnemy.GetInAttackRange() && thisEnemy.GetAttackInitiated())
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
            thisEnemy.GetAnimator().SetAnimationFlag("attack", false);
            //thisEnemy.GetEnemyAnimator().animator.SetBool(thisEnemy.GetEnemyAnimator().IfAttackingHash, false);
        }
        else
        {
            state = NodeState.FAILURE;
        }
        thisEnemy.SetAttackInitiated(false);
        return state;
    }

    

    protected override void OnReset()
    {
    }
}
