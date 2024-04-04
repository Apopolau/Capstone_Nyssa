using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;


public class TaskFindEscapeRoute : BTNode
{
    NavMeshAgent thisAgent;
    Enemy thisEnemy;
    Transform escapeWaypoint;

    public TaskFindEscapeRoute(Enemy enemy)
    {
        thisEnemy = enemy;
        thisAgent = enemy.GetComponent<NavMeshAgent>();
    }
    protected override NodeState OnRun()
    {
        if (thisEnemy.isStaggered || thisEnemy.isDying)
        {
            thisEnemy.GetClosestAnimal().GetComponentInParent<Animal>().isKidnapped = false;
            // thisEnemy.enemyAnimator.animator.SetBool(thisEnemy.enemyAnimator.IfAttackingHash, false);
            state = NodeState.FAILURE;
            return state;
        }
        if (!thisEnemy.inKidnapRange)
        {

            thisEnemy.GetClosestAnimal().GetComponentInParent<Animal>().isKidnapped = false;
            state = NodeState.FAILURE;
            return state;
        }

        if (thisEnemy.inKidnapRange)
        {
            escapeWaypoint = thisEnemy.invaderEnemyRoutes.getClosestEscapeRoute(thisEnemy);
            thisEnemy.invaderEnemyRoutes.setClosestEscapeRoute(thisEnemy, escapeWaypoint);
        
            //thisEnemy.GetClosestAnimal().GetComponentInParent<Animal>().isKidnapped = true;




            state = NodeState.SUCCESS;

        }
        return state;
    }

    protected override void OnReset()
    {
    }
}
