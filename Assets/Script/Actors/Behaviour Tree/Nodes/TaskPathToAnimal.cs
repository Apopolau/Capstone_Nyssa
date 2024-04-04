
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
public class TaskPathToAnimal : BTNode
{
    NavMeshAgent thisAgent;
    private Enemy thisEnemy;
    public TaskPathToAnimal(Enemy enemy)
    {
        thisEnemy = enemy;
        thisAgent = thisEnemy.GetComponent<NavMeshAgent>();


    }

    protected override NodeState OnRun()
    {

        if (thisEnemy.isStaggered || thisEnemy.isDying)
        {
            thisAgent.ResetPath();
            state = NodeState.FAILURE;
            return state;
        }

        if (thisAgent.GetComponent<Enemy>().seesAnimal && !thisAgent.GetComponent<Enemy>().inKidnapRange)

        {

            thisAgent.SetDestination(thisEnemy.GetClosestAnimal().transform.position);
            thisEnemy.transform.LookAt(thisEnemy.GetClosestAnimal().transform.position);

            if (thisEnemy.inKidnapRange)
            {
                Debug.Log("i am in kidnap range and should be failing");
                state = NodeState.FAILURE;
                return state;
            }
            state = NodeState.RUNNING;
        }
        else if (thisAgent.GetComponent<Enemy>().inKidnapRange || !thisAgent.GetComponent<Enemy>().seesAnimal)
        {
            state = NodeState.FAILURE;
        }

        return state;


    }
    protected override void OnReset()
    {

    }

}
