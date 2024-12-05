using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class TaskAwaitDeath : BTNode
{
    Actor thisActor;

    public TaskAwaitDeath(Actor actor)
    {
        thisActor = actor;
    }

    protected override NodeState OnRun()
    {
        thisActor.ResetAgentPath();
        state = NodeState.SUCCESS;
        return state;

    }

    protected override void OnReset() { }
}

