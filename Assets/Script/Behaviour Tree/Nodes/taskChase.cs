using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class taskChase : BTNode
{
    private Transform thisTarget;
    NavMeshAgent thisAgent;
    EarthPlayer thisPlayer;

    public taskChase(Transform target, NavMeshAgent enemyMeshAgent, EarthPlayer player)
    {
        thisAgent = enemyMeshAgent;
        thisTarget = target;
        thisPlayer = player;
    }
    

    protected override NodeState OnRun()
    {
        // throw new System.NotImplementedException();

        float distance = Vector3.Distance(thisTarget.position, thisAgent.transform.position);

        if (distance > 7f && thisAgent.GetComponent<Enemy>().seesPlayer)
        {
            thisAgent.SetDestination(thisTarget.position);
            //Debug.Log(distance);
            Debug.Log("im chasing");
            state = NodeState.RUNNING;
        }
        else if (distance <= 7f)
        {
           // Debug.Log(distance);
            Debug.Log("im close enough");
            state = NodeState.SUCCESS;
        }
        else if (! thisAgent.GetComponent<Enemy>().seesPlayer)
        {
            state = NodeState.FAILURE;
        }




            return state;


    }


    protected override void OnReset()
    {

    }
}
