using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class taskChase : BTNode
{
    private Transform thisTarget;
    NavMeshAgent thisAgent;
    CelestialPlayer thisPlayer;

    public taskChase(Transform target, NavMeshAgent enemyMeshAgent, CelestialPlayer player)
    {
        thisAgent = enemyMeshAgent;
        thisTarget = target;
        thisPlayer = player;
    }
    

    protected override NodeState OnRun()
    {
        // throw new System.NotImplementedException();

        float distance = Vector3.Distance(thisTarget.position, thisAgent.transform.position);

        if (!thisAgent.GetComponent<Enemy>().inAttackRange && thisAgent.GetComponent<Enemy>().seesPlayer)
       
        {
            Debug.Log("I'm chasing abby");
            thisAgent.SetDestination(thisTarget.position);
            if (distance  <= 10f)
            {
                //Debug.Log(distance);
                thisAgent.GetComponent<Enemy>().inAttackRange = true;
            }
            state = NodeState.RUNNING;
        }
      
       /* else if (thisAgent.GetComponent<Enemy>().inAttackRange)
        {
           // Debug.Log(distance);
            Debug.Log("im close enough");
            thisAgent.GetComponent<Enemy>().inAttackRange = true;
            state = NodeState.SUCCESS;
        }*/
        else if (thisAgent.GetComponent<Enemy>().inAttackRange || !thisAgent.GetComponent<Enemy>().seesPlayer)
        {
            state = NodeState.FAILURE;
        }




            return state;


    }


    protected override void OnReset()
    {

    }
}
