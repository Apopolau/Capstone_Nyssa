using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;


public class TaskInitiateSmother : BTNode
{
    PlasticBagMonster thisEnemy;

    public TaskInitiateSmother(PlasticBagMonster enemy)
    {
        thisEnemy = enemy;
    }


    protected override NodeState OnRun()
    {

        //float distance = Vector3.Distance(thisEnemy.GetClosestPlant().transform.position, thisAgent.transform.position);
        if(thisEnemy.GetClosestPlant() == null)
        {
            return NodeState.FAILURE;
        }

        if (!thisEnemy.GetSmotherInitiated() && !thisEnemy.GetClosestPlant().GetComponentInParent<Plant>().isDying)
        {
            if (thisEnemy.GetInSmotherRange() && !thisEnemy.GetIsDying() && !thisEnemy.GetIsStaggered())
            {
                thisEnemy.GetClosestPlant().GetComponentInParent<Plant>().isSmothered = true;
                thisEnemy.SetSmotherInitiated(true);
                state = NodeState.SUCCESS;
            }
            else
            {
                thisEnemy.GetClosestPlant().GetComponentInParent<Plant>().isSmothered = false;
                thisEnemy.SetSmotherInitiated(false);
                state = NodeState.FAILURE;
            }
        }
        else
        {
            state = NodeState.RUNNING;
        }

        return state;
    }



    protected override void OnReset()
    {

    }
}
