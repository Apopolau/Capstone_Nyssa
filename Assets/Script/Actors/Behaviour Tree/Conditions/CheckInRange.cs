using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
public class CheckInRange : BTCondition
{
    NavMeshAgent agent;

    public CheckInRange(NavMeshAgent checkingAgent)
    {
        agent = checkingAgent;
    }

    protected override NodeState OnRun()
    {
        if (agent.GetComponent<Enemy>().seesPlayer )
        {
            Debug.Log("in range");

            return NodeState.SUCCESS;
        }
        else
        {
            Debug.Log("not in range");
            return NodeState.FAILURE;
        }
    }

    protected override void OnReset() { }
}
