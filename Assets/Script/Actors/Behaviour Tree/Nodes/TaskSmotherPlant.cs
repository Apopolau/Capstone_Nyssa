using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class TaskSmotherPlant : BTNode
{
    NavMeshAgent thisAgent;
    Enemy thisEnemy;

    public TaskSmotherPlant(Enemy enemy)
    {
        thisEnemy = enemy;
        thisAgent = enemy.GetComponent<NavMeshAgent>();
    }

    protected override NodeState OnRun()
    {

        if (thisEnemy.GetClosestPlant())
        {
            float distance = Vector3.Distance(thisEnemy.GetClosestPlant().transform.position, thisAgent.transform.position);
        }
        else
        {
            thisEnemy.smotherInitiated = false;
            state = NodeState.FAILURE;
            return state;
        }

        if (thisEnemy.isStaggered || thisEnemy.isDying)
        {
            thisEnemy.GetClosestPlant().GetComponentInParent<Plant>().isSmothered = false;
            // thisEnemy.enemyAnimator.animator.SetBool(thisEnemy.enemyAnimator.IfAttackingHash, false);
            state = NodeState.FAILURE;
            return state;
        }
        if (!thisEnemy.inSmotherRange)
        {

            thisEnemy.GetClosestPlant().GetComponentInParent<Plant>().isSmothered = false;
            // thisEnemy.enemyAnimator.animator.SetBool(thisEnemy.enemyAnimator.IfAttackingHash, false);
            state = NodeState.FAILURE;
            return state;
        }


        if (thisEnemy.inSmotherRange && thisEnemy.smotherInitiated)
        {
            //yield return attackTime;
            //smother initiated should be turned off
             thisEnemy.GetClosestPlant().GetComponentInParent<Plant>().isSmothered = true;

            if (!thisEnemy.GetClosestPlant().GetComponentInParent<Plant>().isDying)
            {
                thisEnemy.SmotherInitiated();
                thisEnemy.GetClosestPlant().GetComponentInParent<Plant>().TakeDamage((int)700);
             

            }
            else if (thisEnemy.GetClosestPlant().GetComponentInParent<Plant>().isDying)
            {    state = NodeState.FAILURE;
                return state;

            }

            state = NodeState.SUCCESS;

        }
        return state;
    }
    protected override void OnReset()
    {
    }
}
