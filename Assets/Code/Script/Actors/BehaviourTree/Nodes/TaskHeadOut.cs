using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
public class TaskHeadOut : BTNode
{
    NavMeshAgent thisAgent;
    KidnappingEnemy thisEnemy;
    //Transformation
    private Transform transformPos;
    //private List<Transform> currWayPointList;
    //private int currWaypointIndex = 0;
    //Wait a Sec
    //private bool iswaiting = false;
    //private float waitTime = 1f;
    //private float waitCounter = 0;
    //Rigidbody rb;
  
    //private float elapsed = 0.0f;

    public TaskHeadOut(KidnappingEnemy enemy, NavMeshAgent enemyMeshAgent, Transform transform)
    {
        thisEnemy = enemy;
        thisAgent = enemyMeshAgent;
        transformPos = transform;
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
            //Transform wPoint = currWayPointList[currWaypointIndex].transform;
            Transform wPoint = thisEnemy.GetEscapeWaypoint();

            float distance = Vector3.Distance(transformPos.position, wPoint.position);
            float stoppingDistance = thisAgent.stoppingDistance;

            if (distance < stoppingDistance)
            {
                //currWaypointIndex++;
                //state = NodeState.RUNNING;
                state = NodeState.SUCCESS;
            }
            else
            {
                thisEnemy.SetAgentPath(wPoint.position);

                transformPos.LookAt(wPoint.position);
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