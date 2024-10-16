using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class CheckIfPlantSpotted : BTCondition
{

    PlasticBagMonster thisEnemy;

    public CheckIfPlantSpotted(PlasticBagMonster enemy)
    {
        thisEnemy = enemy;
    }
    protected override NodeState OnRun()
    { if (thisEnemy.GetSeesPlant())
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


