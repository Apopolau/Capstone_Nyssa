using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
// Reference 
public class TaskWander : BTNode
{
    NavMeshAgent thisAgent;
    Enemy thisEnemy;
    //Transformation
    private Transform transformPos;
    //Wait a Sec
    private bool iswaiting = false;
    private float waitTime = 1f;
    private float waitCounter = 0;
    Rigidbody rb;
    bool isSet = false;
    Vector3 newPos = Vector3.zero;


    public float range = 20;




    public TaskWander(Enemy enemy, Rigidbody rbi, NavMeshAgent enemyMeshAgent, Transform transform)
    {
        thisEnemy = enemy;
        thisAgent = enemyMeshAgent;
        transformPos = transform;
        rb = rbi;
    }
    protected override NodeState OnRun()
    {
        //Get a random point inside of a sphere based on the range given and the current position
        Vector3 randomPoint = transformPos.position + Random.insideUnitSphere * range;

        bool validPos = false;
        NavMeshHit hit;

        if (thisEnemy.seesPlant)
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
            if (distance < 1f)
            {


                //rb.MovePosition( wPoint.position);
                // thisAgent.transform.position = newPos;
                waitCounter = 0f;

                isSet = false;
                state = NodeState.RUNNING;
            }
            else if (thisEnemy.isColliding)
            {
                isSet = false;

            }
            //if not move towards it
            else
            {
                if (validPos)
                {
                    thisAgent.SetDestination(newPos);
                    state = NodeState.SUCCESS;
                }
            }
        }
        return state;

    }


    protected override void OnReset() { }
}