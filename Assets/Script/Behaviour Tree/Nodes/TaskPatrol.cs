using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
// Reference https://www.youtube.com/watch?v=aR6wt5BlE-E&t=904s
public class TaskPatrol : BTNode
{
    NavMeshAgent thisAgent;
    Enemy thisEnemy;
    //Transformation
    private Transform transformPos;
    private Transform[] waypoints;
    private int currWaypointIndex=0;
    //Wait a Sec
    private bool iswaiting = false;
    private float waitTime = 1f;
    private float waitCounter =0;


    public TaskPatrol(Enemy enemy, NavMeshAgent enemyMeshAgent, Transform transform, Transform[] waypointList)
    {
        thisEnemy = enemy;
        thisAgent = enemyMeshAgent;
        transformPos = transform;
        waypoints = waypointList;
    }
    protected override NodeState OnRun() {
        Debug.Log("start patrolling");

        //if while player is chilling they are in range of somehing, patroling fails
        if (iswaiting)
        {
            waitCounter += Time.deltaTime;
            if (waitCounter >= waitTime)
            {
                iswaiting = false;
            }
        }
        else
        {
            Transform wPoint = waypoints[currWaypointIndex];
            //check if the enemy has reached the waypoint, if it has pause for a few seconds
            if (Vector3.Distance(transformPos.position, wPoint.position) < 0.5f)
            {
                transformPos.position = wPoint.position;
                waitCounter = 0f;
                iswaiting = true;
                //makes enemy loop back through way points
                currWaypointIndex = (currWaypointIndex + 1) % waypoints.Length;
                Debug.Log("waiting");
                state = NodeState.RUNNING;
            }
            //if not move towards it
            else
            {
                transformPos.position= Vector3.MoveTowards(transformPos.position, wPoint.position, 0.5f);
                //thisAgent.SetDestination(wPoint.position);
                transformPos.LookAt(wPoint.position);
                Debug.Log("making rounds");
                state = NodeState.RUNNING;
            }
        }
   
        if (thisAgent.GetComponent<Enemy>().seesPlayer)
         {
             state = NodeState.FAILURE;
         }
        return state;
    }
    protected override void OnReset() { }
}
