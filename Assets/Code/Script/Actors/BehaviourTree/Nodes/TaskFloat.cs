using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
// Reference 
public class TaskFloat : BTNode
{
    NavMeshAgent thisAgent;
    PlasticBagMonster thisEnemy;
    //Transformation
    private Transform transformPos;
    //Wait a Sec
    //private bool iswaiting = false;
    //private float waitTime = 1f;
    //private float waitCounter = 0;
    Rigidbody rb;
    bool isSet = false;
    Vector3 newPos = Vector3.zero;
    float distanceToEdge = 1;

    public float range = 20;

    public TaskFloat(PlasticBagMonster enemy)
    {
        thisEnemy = enemy;
        thisAgent = thisEnemy.GetComponent<NavMeshAgent>();
        transformPos = thisEnemy.GetComponent<Transform>();
    }
    protected override NodeState OnRun()
    {

        if (thisEnemy.GetComponent<PlasticBagMonster>().GetSeesPlant())
        {
            return NodeState.FAILURE;
        }

        //Get a random point inside of a sphere based on the range given and the current position
        Vector3 randomPoint = transformPos.position + Random.insideUnitSphere * range;

        bool validPos = false;
        NavMeshHit hit;

        if (thisEnemy.GetSeesPlant())
        {
            state = NodeState.FAILURE;
        }


        if (!isSet)
        {
            if (NavMesh.SamplePosition(randomPoint, out hit, 10.0f, NavMesh.AllAreas))
            {
                newPos = hit.position;
                validPos = true;
                isSet = true;

            }
            else
            {
                newPos = Vector3.zero;
                validPos = false;

            }
            state = NodeState.RUNNING;
        }


        if (isSet)
        {
            float distance = Vector3.Distance(thisAgent.transform.position, newPos);

            //check if the enemy has reached the new position if so pause
            if (distance < 3f)
            {

                //waitCounter = 0f;

                isSet = false;
                //state = NodeState.RUNNING;
                state = NodeState.FAILURE;
            }
            else if (thisEnemy.GetIsColliding())
            {
                isSet = false;

            }
            //if not move towards it
            else
            {
                if (validPos)
                {

                    thisEnemy.SetAgentPath(newPos);


                    if (UnityEngine.AI.NavMesh.FindClosestEdge(thisAgent.transform.position, out hit, UnityEngine.AI.NavMesh.AllAreas))
                    {
                        distanceToEdge = hit.distance;
                    }

                    if (distanceToEdge < 1f)
                    {
                        isSet = false;
                        state = NodeState.FAILURE;
                    }

                    state = NodeState.SUCCESS;
                }
            }
        }
        return state;

    }


    protected override void OnReset() { }
}