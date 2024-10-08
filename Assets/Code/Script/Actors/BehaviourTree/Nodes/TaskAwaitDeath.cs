using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class TaskAwaitDeath : BTNode
{
    NavMeshAgent thisAgent;
    public TaskAwaitDeath(NavMeshAgent agent)
    {
        thisAgent = agent;
    }

    protected override NodeState OnRun()
    {
        thisAgent.ResetPath();
        state = NodeState.SUCCESS;
        return state;

    }

    protected override void OnReset() { }
}

