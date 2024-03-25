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
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }

    protected override void OnReset() { }
}
