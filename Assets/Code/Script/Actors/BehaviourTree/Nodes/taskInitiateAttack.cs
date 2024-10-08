using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class taskInitiateAttack : BTNode
{
    NavMeshAgent thisAgent;
    KidnappingEnemy thisEnemy;

    public taskInitiateAttack(KidnappingEnemy enemy)
    {
        thisEnemy = enemy;
        thisAgent = enemy.GetComponent<NavMeshAgent>();
    }

    protected override NodeState OnRun()
    {

        float distance = Vector3.Distance(thisEnemy.GetClosestPlayer().transform.position, thisAgent.transform.position);

        if (!thisEnemy.GetAttackInitiated())
        {
            if (thisEnemy.GetInAttackRange() && !thisEnemy.GetIsDying() && !thisEnemy.GetIsStaggered())
            {
                thisEnemy.SetAttackInitiated(true);
                //thisEnemy.GetEnemyAnimator().animator.SetBool(thisEnemy.GetEnemyAnimator().IfAttackingHash, true);
                thisEnemy.GetAnimator().SetAnimationFlag("attack", true);
                state = NodeState.SUCCESS;
                thisEnemy.GetMonsterSoundLibrary().PlayAttackClips();
            }
            else
            {
                //thisEnemy.GetEnemyAnimator().animator.SetBool(thisEnemy.GetEnemyAnimator().IfAttackingHash, false);
                thisEnemy.GetAnimator().SetAnimationFlag("attack", false);
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
