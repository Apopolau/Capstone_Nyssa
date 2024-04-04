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
    private float waitTime = 1f;
    private float waitCounter = 0;
    Rigidbody rb;
    NavMeshPath path;
    private float elapsed = 0.0f;

    public TaskInvadePatrol(Enemy enemy, NavMeshAgent enemyMeshAgent, Transform transform)
    {
        thisEnemy = enemy;
        thisAgent = enemyMeshAgent;
        transformPos = transform;
        path = new NavMeshPath();


    }


    protected override NodeState OnRun()
    {
        //Debug.Log("start patrolling");

        //if while player is chilling they are in range of somehing, patroling fails
        currWayPointList = thisEnemy.chosenPath;

        if (thisAgent.GetComponent<Enemy>().seesPlayer || thisAgent.GetComponent<Enemy>().seesAnimal)
        {
            state = NodeState.FAILURE;
        }
        else
        { //Debug.Log("I'm patrolling, the current waypoint list index is" + currWayPointList[0].transform.position);
            Transform wPoint = currWayPointList[currWaypointIndex].transform;
            float distance = Vector3.Distance(transformPos.position, wPoint.position);
          /*  Debug.Log("current distance is" + distance);
            Debug.Log("current position" + transformPos.position);
            Debug.Log("I'm patrolling, Imgoing to" + wPoint.position);*/
            float stoppingDistance = thisAgent.stoppingDistance;

            if(distance < stoppingDistance)
            {
              //  if (currWaypointIndex < currWayPointList.Count)
                    //Debug.Log("Heading to waypoint #:" + currWaypointIndex);
               // {
                    currWaypointIndex = (currWaypointIndex + 1);
                    state = NodeState.RUNNING;


               // }
                //Debug.Log("waiting");

            }
            else
            {

                /// thisAgent.transform.position
                //rb.MovePosition(Vector3.MoveTowards(rb.position, wPoint.position, 10f * Time.deltaTime));
                //for more complex and or bigger scenes
             /*   elapsed += Time.deltaTime;
                if (elapsed > 1.0f)
                {
                    elapsed -= 1.0f;
                    NavMesh.CalculatePath(transformPos.position, wPoint.position, NavMesh.AllAreas, path);
                    
                }*/

                thisAgent.SetDestination(wPoint.position);

                transformPos.LookAt(wPoint.position);
                //Debug.Log("making rounds");
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