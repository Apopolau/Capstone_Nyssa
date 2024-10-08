
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
public class TaskPathToAnimal : BTNode
{
    NavMeshAgent thisAgent;
    private KidnappingEnemy thisEnemy;

    public TaskPathToAnimal(KidnappingEnemy enemy)
    {
        thisEnemy = enemy;
        thisAgent = thisEnemy.GetComponent<NavMeshAgent>();


    }

    protected override NodeState OnRun()
    {

        if (thisEnemy.GetIsStaggered() || thisEnemy.GetIsDying())
        {
            //thisAgent.ResetPath();
            thisEnemy.ResetAgentPath();
            state = NodeState.FAILURE;
            return state;
        }

        if (thisEnemy.GetSeesAnimal() && !thisEnemy.GetInKidnapRange())

        {
            thisEnemy.SetAgentPath(thisEnemy.GetClosestAnimal().transform.position);
            //thisAgent.SetDestination(thisEnemy.GetClosestAnimal().transform.position);
            thisEnemy.transform.LookAt(thisEnemy.GetClosestAnimal().transform.position);

            if (thisEnemy.GetInKidnapRange())
            {
                thisEnemy.ResetAgentPath();
                state = NodeState.FAILURE;
                return state;
            }
            state = NodeState.RUNNING;
        }
        else if (thisEnemy.GetInKidnapRange() || !thisEnemy.GetSeesAnimal())
        {
            thisEnemy.ResetAgentPath();
            state = NodeState.FAILURE;
        }

        return state;


    }
    protected override void OnReset()
    {

    }

}
