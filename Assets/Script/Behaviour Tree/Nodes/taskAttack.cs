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



    public TaskAttack(Transform target, NavMeshAgent enemyMeshAgent, CelestialPlayer player)
    {
        thisAgent = enemyMeshAgent;
        thisTarget = target;
        thisEnemy = enemyMeshAgent.GetComponent<Enemy>();
    }

    protected override NodeState OnRun()
    {

        float distance = Vector3.Distance(thisTarget.position, thisAgent.transform.position);

          
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
            bool playerIsDead;
            playerIsDead = thisTarget.GetComponentInParent<CelestialPlayer>().TakeHit();
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
