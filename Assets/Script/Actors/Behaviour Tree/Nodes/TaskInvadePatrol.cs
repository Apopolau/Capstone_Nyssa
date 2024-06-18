using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
public class TaskInvadePatrol : BTNode
{
    NavMeshAgent thisAgent;
    Enemy thisEnemy;
    //Transformation
    private Transform transformPos;
    private List<Transform> currWayPointList;
    private int currWaypointIndex = 0;
    //Wait a Sec
    private bool iswaiting = false;
    //private float waitTime = 1f;
    //private float waitCounter = 0;
    Rigidbody rb;
    NavMeshPath path;
    //private float elapsed = 0.0f;

    public TaskInvadePatrol(Enemy enemy, NavMeshAgent enemyMeshAgent, Transform transform)
    {
        thisEnemy = enemy;
        thisAgent = enemyMeshAgent;
        transformPos = transform;
        path = new NavMeshPath();


    }


    protected override NodeState OnRun()
    {

        //if while player is chilling they are in range of somehing, patroling fails
        currWayPointList = thisEnemy.chosenPath;

        if (thisAgent.GetComponent<Enemy>().seesPlayer || thisAgent.GetComponent<Enemy>().seesAnimal)
        {
            state = NodeState.FAILURE;
        }
        else
        {
            Transform wPoint = currWayPointList[currWaypointIndex].transform;
            float distance = Vector3.Distance(transformPos.position, wPoint.position);

            float stoppingDistance = thisAgent.stoppingDistance;

            if(distance < stoppingDistance)
            {

                    currWaypointIndex = (currWaypointIndex + 1);
                    state = NodeState.RUNNING;

            }
            else
            {


               thisAgent.SetDestination(wPoint.position);

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