using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class CheckIfPlantSpotted : BTCondition
{

    Enemy thisEnemy;

    public CheckIfPlantSpotted(Enemy enemy)
    {
        thisEnemy = enemy;
    }
    protected override NodeState OnRun()
    { if (thisEnemy.seesPlant)
        {
            return NodeState.SUCCESS;
        }
    else
        {
            return NodeState.FAILURE;
        }
    
    
    
    }
    protected override void OnReset()
    {
    }
}


