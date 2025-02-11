using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
public class TaskInvadePatrol : BTNode
{
    NavMeshAgent thisAgent;
    KidnappingEnemy thisEnemy;
    //Transformation
    private Transform transformPos;
    private List<Transform> currWayPointList;
    private int currWaypointIndex = 0;

    public TaskInvadePatrol(KidnappingEnemy enemy)
    {
        thisEnemy = enemy;
        thisAgent = thisEnemy.GetComponent<NavMeshAgent>();
        transformPos = thisEnemy.GetComponent<Transform>();
    }

    protected override NodeState OnRun()
    {

        //If we run across a player or animal while pathing, stop pathing
        currWayPointList = thisEnemy.GetInvasionPath();

        if(currWayPointList.Count == 0)
        {
            state = NodeState.FAILURE;
            return state;
        }

        if (thisEnemy.GetSeesPlayer() || (thisEnemy.GetSeesAnimal() && !thisEnemy.GetClosestAnimal().GetComponent<Animal>().GetIsStuck()))
        {
            state = NodeState.FAILURE;
        }
        else
        {
            Transform wPoint = currWayPointList[currWaypointIndex].transform;
            float distance = Vector3.Distance(transformPos.position, wPoint.position);

            float stoppingDistance = thisAgent.stoppingDistance;

            if (distance < stoppingDistance)
            {
                currWaypointIndex = (currWaypointIndex + 1);
                state = NodeState.RUNNING;
            }
            else
            {
                thisEnemy.SetAgentPath(wPoint.position);
                //thisAgent.SetDestination(wPoint.position);

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