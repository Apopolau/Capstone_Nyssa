using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class TaskSmotherPlant : BTNode
{
    NavMeshAgent thisAgent;
    PlasticBagMonster thisEnemy;

    public TaskSmotherPlant(PlasticBagMonster enemy)
    {
        thisEnemy = enemy;
        thisAgent = enemy.GetComponent<NavMeshAgent>();
    }

    protected override NodeState OnRun()
    {
        //If the bag is interrupted, it can't smother anymore
        if (thisEnemy.GetIsStaggered() || thisEnemy.GetIsDying())
        {
            thisEnemy.GetClosestPlant().GetComponentInParent<Plant>().isSmothered = false;
            thisEnemy.SetSmotherInitiated(false);
            state = NodeState.FAILURE;
            return state;
        }

        if (thisEnemy.GetInSmotherRange() && thisEnemy.GetSmotherInitiated())
        {
            //Reset bag's smother state so it can try again
            thisEnemy.SetSmotherInitiated(false);
            state = NodeState.RUNNING;

            //If the plant's not dead, we get it to take damage
            if (!thisEnemy.GetClosestPlant().GetComponentInParent<Plant>().isDying)
            {
                state = NodeState.SUCCESS;
                
                thisEnemy.GetClosestPlant().GetComponentInParent<Plant>().TakeDamage((int)thisEnemy.GetEnemyStats().maxDamage);
            }
            //Otherwise we want to leave this procedure
            else if (thisEnemy.GetClosestPlant().GetComponentInParent<Plant>().isDying)
            {
                state = NodeState.FAILURE;
                return state;
            }
        }
        return state;
    }
    protected override void OnReset()
    {
    }
}
