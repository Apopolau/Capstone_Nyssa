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

        //If the monster gets attacked, it should drop its animal and eject from this sequence
        if (thisEnemy.GetIsStaggered() || thisEnemy.GetIsDying())
        {
            if(thisEnemy.GetKidnappedAnimal() != null)
                thisEnemy.GetKidnappedAnimal().SetKidnapStatus(false);

            state = NodeState.FAILURE;
            return state;
        }
        //If the monster falls out of kidnap range we don't want it to be able to kidnap
        if (!thisEnemy.GetInKidnapRange())
        {
            if (thisEnemy.GetKidnappedAnimal() != null)
                thisEnemy.GetKidnappedAnimal().SetKidnapStatus(false);

            state = NodeState.FAILURE;
            return state;
        }
        //If the animal it's trying to kidnap is already kidnapped, it should fail
        if (thisEnemy.GetClosestAnimal().GetComponent<Animal>().GetIsKidnapped() || thisEnemy.GetClosestAnimal().GetComponent<Animal>().GetIsStuck())
        {
            state = NodeState.FAILURE;
            return state;
        }
        
        //if none of those other things are true, we should successfully kidnap
        if (thisEnemy.GetInKidnapRange())
        {
            thisEnemy.GetClosestAnimal().GetComponent<Animal>().SetKidnapper(thisEnemy);
            thisEnemy.GetClosestAnimal().GetComponent<Animal>().SetKidnapStatus(true);
            thisEnemy.SetKidnappedAnimal(thisEnemy.GetClosestAnimal().GetComponent<Animal>());

            state = NodeState.SUCCESS;
        }
        return state;
    }

    protected override void OnReset()
    {
    }
}
