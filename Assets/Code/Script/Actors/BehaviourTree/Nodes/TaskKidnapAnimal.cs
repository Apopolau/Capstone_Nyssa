using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class TaskKidnapAnimal : BTNode
{
    NavMeshAgent thisAgent;
    KidnappingEnemy thisEnemy;

    public TaskKidnapAnimal(KidnappingEnemy enemy)
    {
        thisEnemy = enemy;
        thisAgent = enemy.GetComponent<NavMeshAgent>();
    }

    protected override NodeState OnRun()
    {

        float distance = Vector3.Distance(thisEnemy.GetClosestAnimal().transform.position, thisAgent.transform.position);

        if (thisEnemy.GetIsStaggered() || thisEnemy.GetIsDying())
        {
            if(thisEnemy.GetKidnappedAnimal() != null)
                thisEnemy.GetKidnappedAnimal().SetKidnapStatus(false);
            //thisEnemy.GetClosestAnimal().GetComponentInParent<Animal>().UpdateKidnapIcon();
            // thisEnemy.enemyAnimator.animator.SetBool(thisEnemy.enemyAnimator.IfAttackingHash, false);
            state = NodeState.FAILURE;
            return state;
        }
        if (!thisEnemy.GetInKidnapRange())
        {
            if (thisEnemy.GetKidnappedAnimal() != null)
                thisEnemy.GetKidnappedAnimal().SetKidnapStatus(false);
            //thisEnemy.GetClosestAnimal().GetComponentInParent<Animal>().UpdateKidnapIcon();
            // thisEnemy.enemyAnimator.animator.SetBool(thisEnemy.enemyAnimator.IfAttackingHash, false);
            state = NodeState.FAILURE;
            return state;
        }
        if (thisEnemy.GetClosestAnimal().GetComponent<Animal>().GetIsKidnapped())
        {
            state = NodeState.FAILURE;
            return state;

        }

        //if (thisEnemy.inSmotherRange && thisEnemy.smotherInitiated)
        if (thisEnemy.GetInKidnapRange())
        {
            thisEnemy.GetClosestAnimal().GetComponent<Animal>().SetKidnapper(thisEnemy);
            thisEnemy.GetClosestAnimal().GetComponent<Animal>().SetKidnapStatus(true);
            thisEnemy.SetKidnappedAnimal(thisEnemy.GetClosestAnimal().GetComponent<Animal>());
            //thisEnemy.GetClosestAnimal().GetComponentInParent<Animal>().UpdateKidnapIcon();


            state = NodeState.SUCCESS;

        }
        return state;
    }

    protected override void OnReset()
    {
    }
}
