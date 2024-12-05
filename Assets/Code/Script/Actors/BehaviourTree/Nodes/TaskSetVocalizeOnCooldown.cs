using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class TaskSetVocalizeOnCooldown : BTNode
{
    Animal thisAnimal;
    string thisNeedKey;

    public TaskSetVocalizeOnCooldown(Animal animal, string needKey)
    {
        thisAnimal = animal;
        thisNeedKey = needKey;
    }

    //Restores a stat like hunger or life to full
    protected override NodeState OnRun()
    {
        //Debug.Log(thisAnimal + " setting " + thisNeedKey + " to cooldown");
        thisAnimal.StartCoroutine(thisAnimal.RunVocalizeCooldown(thisNeedKey));
        state = NodeState.SUCCESS;
        return state;
    }



    protected override void OnReset()
    {
    }
}
