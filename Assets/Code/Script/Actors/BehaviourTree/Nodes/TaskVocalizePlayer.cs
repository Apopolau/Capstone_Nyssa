using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class TaskVocalizePlayer : BTNode
{
    NavMeshAgent thisAgent;
    float playerRange;
    Animal thisAnimal;
    Sprite thisSprite;
    bool isPlaying = false;

    public TaskVocalizePlayer(NavMeshAgent thisAgent, Animal animal, float range)
    {
        this.thisAgent = thisAgent;
        playerRange = range;
        thisAnimal = animal;
    }


    protected override NodeState OnRun()
    {
        float distance = (thisAgent.transform.position - thisAnimal.GetClosestPlayer().transform.position).magnitude;
        if(distance <= playerRange)
        {
            thisAgent.GetComponent<Transform>().LookAt(thisAnimal.GetClosestPlayer().transform);
            if (thisAnimal.GetClosestPlayer().GetComponent<EarthPlayer>())
            {
                thisSprite = thisAnimal.GetVocalizeImage("sproutImage");
            }
            else if (thisAnimal.GetClosestPlayer().GetComponent<CelestialPlayer>())
            {
                thisSprite = thisAnimal.GetVocalizeImage("celesteImage");
            }
            if (!isPlaying)
            {
                thisAnimal.PlayVocal();
                thisAnimal.TurnOnSpeechPopup(thisSprite);
                isPlaying = true;
            }
        }
        else
        {
            state = NodeState.FAILURE;
            isPlaying = false;
        }
        
        return state;
    }


    protected override void OnReset()
    {

    }
}
