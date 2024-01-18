using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class TaskDie : BTNode
{
    NavMeshAgent thisAgent;
    Enemy thisEnemy;
    bool thisLifeState;



    public TaskDie(Enemy enemy, NavMeshAgent enemyMeshAgent, bool isAlive)
    {
        thisEnemy = enemy;
        thisAgent = enemyMeshAgent;
        thisLifeState = isAlive;

    }
    protected override NodeState OnRun()
    {
        if (thisLifeState)
        {
            Debug.Log("Is alive.");
            state = NodeState.FAILURE;
        }
        if (thisLifeState)
        {
            Debug.Log("Is alive.");
            state = NodeState.SUCCESS;
        }
        return state;

    }
    protected override void OnReset() { }
}

