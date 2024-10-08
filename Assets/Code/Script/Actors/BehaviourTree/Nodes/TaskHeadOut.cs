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
    private List<Transform> currWayPointList;
    private int currWaypointIndex = 0;
    //Wait a Sec
    private bool iswaiting = false;
    private float waitTime = 1f;
    private float waitCounter = 0;
    Rigidbody rb;
  
    private float elapsed = 0.0f;

    public TaskHeadOut(KidnappingEnemy enemy, NavMeshAgent enemyMeshAgent, Transform transform)
    {
        thisEnemy = enemy;
        thisAgent = enemyMeshAgent;
        transformPos = transform;
    }


    protected override NodeState OnRun()
    {

        //if while player is chilling they are in range of somehing, patroling fails
        currWayPointList = thisEnemy.GetChosenPath();
        if (thisEnemy.GetIsStaggered() || thisEnemy.GetIsDying())
        {
            thisEnemy.GetClosestAnimal().GetComponentInParent<Animal>().SetKidnapStatus(false);
            //thisEnemy.GetClosestAnimal().GetComponentInParent<Animal>().UpdateKidnapIcon();
            //thisEnemy.GetClosestAnimal().GetComponentInParent<Animal>().ResetOrigSpeed();
            state = NodeState.FAILURE;
        }
        else
        { 
            Transform wPoint = thisEnemy.GetEscapeRoute();
            float distance = Vector3.Distance(transformPos.position, wPoint.position);
           
            float stoppingDistance = thisAgent.stoppingDistance;

            if (distance < stoppingDistance)
            {

                state = NodeState.RUNNING;

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