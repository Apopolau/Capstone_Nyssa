using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class TaskKidnapAnimal : BTNode
{
    NavMeshAgent thisAgent;
    Enemy thisEnemy;

    public TaskKidnapAnimal(Enemy enemy)
    {
        thisEnemy = enemy;
        thisAgent = enemy.GetComponent<NavMeshAgent>();
    }

    protected override NodeState OnRun()
    {
        Debug.Log("Enter kidnapping");

        float distance = Vector3.Distance(thisEnemy.GetClosestAnimal().transform.position, thisAgent.transform.position);

        if (thisEnemy.isStaggered || thisEnemy.isDying)
        {
            thisEnemy.GetClosestAnimal().GetComponentInParent<Animal>().SetKidnapStatus(false);
            //thisEnemy.GetClosestAnimal().GetComponentInParent<Animal>().UpdateKidnapIcon();
            // thisEnemy.enemyAnimator.animator.SetBool(thisEnemy.enemyAnimator.IfAttackingHash, false);
            state = NodeState.FAILURE;
            return state;
        }
        if (!thisEnemy.inKidnapRange)
        {
            thisEnemy.GetClosestAnimal().GetComponentInParent<Animal>().SetKidnapStatus(false);
            //thisEnemy.GetClosestAnimal().GetComponentInParent<Animal>().UpdateKidnapIcon();
            // thisEnemy.enemyAnimator.animator.SetBool(thisEnemy.enemyAnimator.IfAttackingHash, false);
            state = NodeState.FAILURE;
            return state;
        }
        if (thisEnemy.GetClosestAnimal().GetComponentInParent<Animal>().isKidnapped)
        {
            state = NodeState.FAILURE;
            return state;

        }

            //if (thisEnemy.inSmotherRange && thisEnemy.smotherInitiated)
            if (thisEnemy.inKidnapRange)
        {
            thisEnemy.GetClosestAnimal().GetComponentInParent<Animal>().kidnapper = thisEnemy;
            thisEnemy.GetClosestAnimal().GetComponentInParent<Animal>().SetKidnapStatus(true);
            //thisEnemy.GetClosestAnimal().GetComponentInParent<Animal>().UpdateKidnapIcon();


            state = NodeState.SUCCESS;

        }
        return state;
    }
    protected override void OnReset()
    {
    }
}
