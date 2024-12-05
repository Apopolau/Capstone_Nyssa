using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
// Reference https://www.youtube.com/watch?v=aR6wt5BlE-E&t=904s
public class TaskPatrol : BTNode
{
    NavMeshAgent thisAgent;
    KidnappingEnemy thisEnemy;
    private Transform[] waypoints;
    private int currWaypointIndex = 0;
    //Wait a Sec
    private bool iswaiting = false;
    private float waitTime = 1f;
    private float waitCounter = 0;

    public TaskPatrol(KidnappingEnemy enemy, Transform[] waypointList)
    {
        thisEnemy = enemy;
        thisAgent = thisEnemy.GetComponent<NavMeshAgent>();
        waypoints = waypointList;
    }
    protected override NodeState OnRun()
    {
        //if while player is chilling they are in range of somehing, patroling fails


        if (thisEnemy.GetSeesPlayer())
        {
            thisEnemy.ResetAgentPath();
            state = NodeState.FAILURE;
        }

        else
        {
            if (iswaiting)
            {
                waitCounter += Time.deltaTime;
                if (waitCounter >= waitTime)
                {
                    iswaiting = false;
                }

                state = NodeState.RUNNING;

            }
            else
            {
                Transform wPoint = waypoints[currWaypointIndex];
                float distance = Vector3.Distance(thisEnemy.GetComponent<Transform>().position, wPoint.position);

                //check if the enemy has reached the waypoint, if it has pause for a few seconds
                if (distance <= thisAgent.stoppingDistance)
                {
                    //thisAgent.transform.position = wPoint.position;
                    waitCounter = 0f;
                    iswaiting = true;
                    //makes enemy loop back through way points
                    currWaypointIndex = (currWaypointIndex + 1) % waypoints.Length;
                    state = NodeState.SUCCESS;
                }
                //if not move towards it
                else
                {
                    thisEnemy.SetAgentPath(wPoint.position);
                    state = NodeState.RUNNING;
                }
            }

        }

        return state;
    }
    protected override void OnReset() { }
}
