using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class TaskVocalize : BTNode
{
    NavMeshAgent thisAgent;
    float playerRange;
    Animal thisAnimal;
    Sprite thisSprite;
    bool isPlaying = false;

    public TaskVocalize(NavMeshAgent thisAgent, Animal animal, float range, Sprite sprite)
    {
        this.thisAgent = thisAgent;
        playerRange = range;
        thisAnimal = animal;
        thisSprite = sprite;
    }


    protected override NodeState OnRun()
    {
        float distance = (thisAgent.transform.position - thisAnimal.GetClosestPlayer().transform.position).magnitude;
        if(distance <= playerRange)
        {
            thisAgent.GetComponent<Transform>().LookAt(thisAnimal.GetClosestPlayer().transform);
            //Do a popup here
            if (!isPlaying)
            {
                thisAnimal.PlayVocal();
                thisAnimal.TurnOnSpeechPopup(thisSprite);
                isPlaying = true;
            }
            
            state = NodeState.RUNNING;
        }
        else
        {
            isPlaying = false;
            state = NodeState.FAILURE;
        }
        
        return state;
    }


    protected override void OnReset()
    {

    }
}
