using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
public class CheckInAttackRange : BTCondition
{
    private Transform thisTarget;
    NavMeshAgent thisAgent;
    CelestialPlayer thisPlayer;

    public CheckInAttackRange(Transform target, NavMeshAgent enemyMeshAgent, CelestialPlayer player)
    {
        thisAgent = enemyMeshAgent;
        thisTarget = target;
        thisPlayer = player;
    }

    protected override NodeState OnRun()
    {
        float distance = Vector3.Distance(thisTarget.position, thisAgent.transform.position);
        if (thisAgent.GetComponent<Enemy>().inAttackRange)
        {
           // Debug.Log("in attack range");
            //place enemy animation here
          //  thisAgent.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
            return NodeState.SUCCESS;
        }
        else
        {
           // Debug.Log("not in attack range");
            return NodeState.FAILURE;
        }
    }

    protected override void OnReset() { }
}
