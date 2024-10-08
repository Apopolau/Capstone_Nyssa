using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class CheckIfAnimalSpotted : BTCondition
{

    KidnappingEnemy thisEnemy;

    public CheckIfAnimalSpotted(KidnappingEnemy enemy)
    {
        thisEnemy = enemy;
    }
    protected override NodeState OnRun()
    {
        if (thisEnemy.GetSeesAnimal())
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


