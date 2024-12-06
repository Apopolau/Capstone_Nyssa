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
        if (thisAnimal.GetComponent<Animal>())
        {
            if (thisAnimal.GetComponent<Animal>().GetIsKidnapped())
            {
                thisAnimal.SetIsHiding(false);
                return NodeState.FAILURE;
            }
        }

        thisAnimal.StartCoroutine(thisAnimal.RunVocalizeCooldown(thisNeedKey));
        state = NodeState.SUCCESS;
        return state;
    }



    protected override void OnReset()
    {
    }
}
