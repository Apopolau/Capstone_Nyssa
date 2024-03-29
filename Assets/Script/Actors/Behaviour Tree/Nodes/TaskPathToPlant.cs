using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
public class TaskPathToPlant : BTNode
{
    NavMeshAgent thisAgent;
    private Enemy thisEnemy;
    public TaskPathToPlant(Enemy enemy)
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

        if (thisAgent.GetComponent<Enemy>().seesPlant && !thisAgent.GetComponent<Enemy>().inSmotherRange)

        {

            thisAgent.SetDestination(thisEnemy.GetClosestPlant().transform.position);
            thisEnemy.transform.LookAt(thisEnemy.GetClosestPlant().transform.position);

            if (thisEnemy.inSmotherRange)
            {
                Debug.Log("i am in smother range and should be failing");
                state = NodeState.FAILURE;
                return state;
            }
            state = NodeState.RUNNING;
        }
        else if (thisAgent.GetComponent<Enemy>().inSmotherRange || !thisAgent.GetComponent<Enemy>().seesPlant)
        {
            state = NodeState.FAILURE;
        }

        return state;


    }
    protected override void OnReset()
    {
     
    }

}
