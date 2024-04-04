using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class CheckIfAnimalSpotted : BTCondition
{

    Enemy thisEnemy;

    public CheckIfAnimalSpotted(Enemy enemy)
    {
        thisEnemy = enemy;
    }
    protected override NodeState OnRun()
    {
        if (thisEnemy.seesAnimal)
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


