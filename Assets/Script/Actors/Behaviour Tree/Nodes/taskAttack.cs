using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
public class TaskAttack : BTNode
{
    NavMeshAgent thisAgent;
    Enemy thisEnemy;
    private Transform thisTarget;
    private Transform transformPos;


    public TaskAttack(Transform target, NavMeshAgent enemyMeshAgent, CelestialPlayer player, Transform transform)
    {
        thisAgent = enemyMeshAgent;
        thisTarget = target;
        thisEnemy = enemyMeshAgent.GetComponent<Enemy>();
        transformPos = transform;
    }

    protected override NodeState OnRun()
    {

        float distance = Vector3.Distance(thisTarget.position, thisAgent.transform.position);

        if(thisEnemy.isStaggered || thisEnemy.isDying)
        {
            thisEnemy.enemyAnimator.animator.SetBool(thisEnemy.enemyAnimator.IfAttackingHash, false);
            state = NodeState.FAILURE;
            return state;
        }
          
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

        if (thisEnemy.inAttackRange && thisEnemy.attackInitiated)
        {
            //yield return attackTime;
            bool playerIsDead = false;
            //I refactored this a little, you may want to set the damage they can deal in their stats and pass it in here
            if (!thisTarget.GetComponentInParent<CelestialPlayer>().isShielded)
            {
                playerIsDead = thisTarget.GetComponentInParent<CelestialPlayer>().TakeHit(10);
            }
            else
            {
                thisEnemy.TakeHit(10);
            }
            
            transformPos.LookAt(thisTarget.position);
            if (playerIsDead)
            {
                state = NodeState.FAILURE;
            }
            else
            {
                Debug.Log("finishing attack");
                //attackcounter += Time.deltaTime;
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
