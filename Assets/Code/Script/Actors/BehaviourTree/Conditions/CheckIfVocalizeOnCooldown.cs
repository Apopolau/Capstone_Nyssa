using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class CheckIfVocalizeOnCooldown : BTCondition
{
    Animal thisAnimal;
    string thisNeedKey;

    // Start is called before the first frame update
    public CheckIfVocalizeOnCooldown(Animal animal, string needKey)
    {
        thisAnimal = animal;
        thisNeedKey = needKey;
    }

    //Returns true if it's night time
    protected override NodeState OnRun()
    {
        if (thisAnimal.GetIsNeedOnCooldown(thisNeedKey))
        {
            //Debug.Log(thisAnimal + "'s " + thisNeedKey + " need was on cooldown");
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }

    protected override void OnReset() { }
}
