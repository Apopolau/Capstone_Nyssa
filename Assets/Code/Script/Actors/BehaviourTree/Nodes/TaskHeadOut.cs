using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
public class TaskHeadOut : BTNode
{
    NavMeshAgent thisAgent;
    KidnappingEnemy thisEnemy;

    public TaskHeadOut(KidnappingEnemy enemy)
    {
        thisEnemy = enemy;
        thisAgent = thisEnemy.GetComponent<NavMeshAgent>();
    }


    protected override NodeState OnRun()
    {

        //If the player stops us, we stop kidnapping
        //currWayPointList = thisEnemy.GetEscapeRoute();

        if (thisEnemy.GetIsStaggered() || thisEnemy.GetIsDying())
        {
            thisEnemy.GetClosestAnimal().GetComponentInParent<Animal>().SetKidnapStatus(false);
            state = NodeState.FAILURE;
        }
        else
        {
            Transform wPoint = thisEnemy.GetEscapeWaypoint();

            float distance = Vector3.Distance(thisEnemy.GetComponent<Transform>().position, wPoint.position);
            float stoppingDistance = thisAgent.stoppingDistance;

            if (distance < stoppingDistance)
            {
                state = NodeState.SUCCESS;
            }
            else
            {
                thisEnemy.SetAgentPath(wPoint.position);

                //transformPos.LookAt(wPoint.position);
                state = NodeState.RUNNING;
            }

            state = NodeState.RUNNING;
        }
        return state;
    }
    protected override void OnReset()
    {
    }

}