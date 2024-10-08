using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
public class TaskPathToPlant : BTNode
{
    NavMeshAgent thisAgent;
    private PlasticBagMonster thisEnemy;
    public TaskPathToPlant(PlasticBagMonster enemy)
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

        if (thisEnemy.GetSeesPlant() && !thisEnemy.GetInSmotherRange())

        {
            thisEnemy.SetAgentPath(thisEnemy.GetClosestPlant().transform.position);
            //thisAgent.SetDestination(thisEnemy.GetClosestPlant().transform.position);
            thisEnemy.transform.LookAt(thisEnemy.GetClosestPlant().transform.position);

            if (thisEnemy.GetInSmotherRange())
            {
                thisEnemy.ResetAgentPath();
                state = NodeState.FAILURE;
                return state;
            }
            state = NodeState.RUNNING;
        }
        else if (thisEnemy.GetInSmotherRange() || !thisEnemy.GetSeesPlant())
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
