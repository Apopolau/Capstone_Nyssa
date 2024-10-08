using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;


public class TaskFindEscapeRoute : BTNode
{
    NavMeshAgent thisAgent;
    KidnappingEnemy thisEnemy;
    Transform escapeWaypoint;

    public TaskFindEscapeRoute(KidnappingEnemy enemy)
    {
        thisEnemy = enemy;
        thisAgent = enemy.GetComponent<NavMeshAgent>();
    }
    protected override NodeState OnRun()
    {
        if (thisEnemy.GetIsStaggered() || thisEnemy.GetIsDying())
        {
            thisEnemy.GetClosestAnimal().GetComponentInParent<Animal>().SetKidnapStatus(false);
            //thisEnemy.GetClosestAnimal().GetComponentInParent<Animal>().UpdateKidnapIcon();
            thisAgent.speed = 7;
            state = NodeState.FAILURE;
            return state;
        }
        if (!thisEnemy.GetInKidnapRange())
        {
            thisEnemy.GetClosestAnimal().GetComponentInParent<Animal>().SetKidnapStatus(false);
            //thisEnemy.GetClosestAnimal().GetComponentInParent<Animal>().UpdateKidnapIcon();
            thisAgent.speed = 7;
            state = NodeState.FAILURE;
            return state;
        }

        if (thisEnemy.GetInKidnapRange())
        {
            escapeWaypoint = thisEnemy.GetInvaderEnemyRoutes().getClosestEscapeRoute(thisEnemy);
            thisEnemy.GetInvaderEnemyRoutes().setClosestEscapeRoute(thisEnemy, escapeWaypoint);
           // thisEnemy.GetClosestAnimal().GetComponentInParent<Animal>().SetCurrSpeed();
            thisEnemy.GetClosestAnimal().GetComponentInParent<Animal>().GetNavMeshAgent().speed = 8;
            thisAgent.speed = 8;

        
            //thisEnemy.GetClosestAnimal().GetComponentInParent<Animal>().isKidnapped = true;




            state = NodeState.SUCCESS;

        }
        return state;
    }

    protected override void OnReset()
    {
    }
}
