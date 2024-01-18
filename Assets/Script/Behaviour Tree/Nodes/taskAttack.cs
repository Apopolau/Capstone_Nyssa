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
    private bool isAttacking= false;
    private float attackTime = 1f;
    private float attackcounter = 0;


    public TaskAttack(Transform target, NavMeshAgent enemyMeshAgent, CelestialPlayer player)
    {
        thisAgent = enemyMeshAgent;
        thisTarget = target;
    }

    protected override NodeState OnRun()
    {

        float distance = Vector3.Distance(thisTarget.position, thisAgent.transform.position);

          
            if (distance <= 10f)
            {
               // Debug.Log(distance);
                thisAgent.GetComponent<Enemy>().inAttackRange = true;
            }
        else
        {
            thisAgent.GetComponent<Enemy>().inAttackRange = false;
        }










        if (thisAgent.GetComponent<Enemy>().inAttackRange)
        {
            Debug.Log("am attacking");
            attackcounter += Time.deltaTime;
            if (attackcounter >= attackTime)
            {
                bool playerIsDead;
                playerIsDead = thisTarget.GetComponent<CelestialPlayer>().TakeHit();
                if (playerIsDead)
                {
                    //go back to patrolling
                    state = NodeState.FAILURE;
                }
                else { attackcounter = 0f; }


            }
            state = NodeState.RUNNING;

        }
        else
        {
            state = NodeState.FAILURE;
        }
        return state;
    }
    protected override void OnReset()
    {
    }
}
