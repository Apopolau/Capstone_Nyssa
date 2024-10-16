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
    //Transformation
    private Transform transformPos;
    private Transform[] waypoints;
    private int currWaypointIndex = 0;
    //Wait a Sec
    private bool iswaiting = false;
    private float waitTime = 1f;
    private float waitCounter = 0;
    Rigidbody rb;

    public TaskPatrol(KidnappingEnemy enemy, Rigidbody rbi, NavMeshAgent enemyMeshAgent, Transform transform, Transform[] waypointList)
    {
        thisEnemy = enemy;
        thisAgent = enemyMeshAgent;
        transformPos = transform;
        waypoints = waypointList;
        rb = rbi;
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
                float distance = Vector3.Distance(transformPos.position, wPoint.position);

                //check if the enemy has reached the waypoint, if it has pause for a few seconds
                if (distance < 2f)
                {

                    //rb.MovePosition( wPoint.position);
                    thisAgent.transform.position = wPoint.position;
                    waitCounter = 0f;
                    iswaiting = true;
                    //makes enemy loop back through way points
                    currWaypointIndex = (currWaypointIndex + 1) % waypoints.Length;
                    state = NodeState.RUNNING;
                }
                //if not move towards it
                else
                {

                    /// thisAgent.transform.position
                    //rb.MovePosition(Vector3.MoveTowards(rb.position, wPoint.position, 10f * Time.deltaTime));

                    //thisAgent.SetDestination(wPoint.position);
                    thisEnemy.SetAgentPath(wPoint.position);

                    transformPos.LookAt(wPoint.position);
                    state = NodeState.RUNNING;
                }
            }

        }
        return state;
    }
    protected override void OnReset() { }
}
