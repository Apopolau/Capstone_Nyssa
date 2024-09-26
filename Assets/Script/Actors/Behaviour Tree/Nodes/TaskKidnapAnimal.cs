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

        float distance = Vector3.Distance(thisEnemy.GetClosestAnimal().transform.position, thisAgent.transform.position);

        if (thisEnemy.isStaggered || thisEnemy.isDying)
        {
            if(thisEnemy.GetKidnappedAnimal() != null)
                thisEnemy.GetKidnappedAnimal().SetKidnapStatus(false);
            //thisEnemy.GetClosestAnimal().GetComponentInParent<Animal>().UpdateKidnapIcon();
            // thisEnemy.enemyAnimator.animator.SetBool(thisEnemy.enemyAnimator.IfAttackingHash, false);
            state = NodeState.FAILURE;
            return state;
        }
        if (!thisEnemy.inKidnapRange)
        {
            if (thisEnemy.GetKidnappedAnimal() != null)
                thisEnemy.GetKidnappedAnimal().SetKidnapStatus(false);
            //thisEnemy.GetClosestAnimal().GetComponentInParent<Animal>().UpdateKidnapIcon();
            // thisEnemy.enemyAnimator.animator.SetBool(thisEnemy.enemyAnimator.IfAttackingHash, false);
            state = NodeState.FAILURE;
            return state;
        }
        if (thisEnemy.GetClosestAnimal().GetComponent<Animal>().isKidnapped)
        {
            state = NodeState.FAILURE;
            return state;

        }

        //if (thisEnemy.inSmotherRange && thisEnemy.smotherInitiated)
        if (thisEnemy.inKidnapRange)
        {
            thisEnemy.GetClosestAnimal().GetComponent<Animal>().kidnapper = thisEnemy;
            thisEnemy.GetClosestAnimal().GetComponent<Animal>().SetKidnapStatus(true);
            thisEnemy.kidnappedAnimal = thisEnemy.GetClosestAnimal().GetComponent<Animal>();
            //thisEnemy.GetClosestAnimal().GetComponentInParent<Animal>().UpdateKidnapIcon();


            state = NodeState.SUCCESS;

        }
        return state;
    }

    protected override void OnReset()
    {
    }
}
