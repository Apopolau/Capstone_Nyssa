using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;


public class TaskGetKidnapped: BTNode
{
    Animal thisAnimal;
    KidnappingEnemy kidnapper;
    NavMeshAgent thisAgent;
    Transform transformPos;


    public  TaskGetKidnapped (Animal animal, NavMeshAgent animalMeshAgent, Transform transform)
    {
        thisAnimal = animal;
        thisAgent = animalMeshAgent;
        transformPos = transform;
    }
    protected override NodeState OnRun()
    { 
        if (!thisAnimal.GetIsKidnapped())
        {
            state = NodeState.FAILURE;
            return state;
        }

        if (thisAnimal.GetIsKidnapped())
        {
            kidnapper = thisAnimal.GetComponent<Animal>().GetKidnapper();
            float distance = Vector3.Distance(transformPos.position, kidnapper.transform.position);
            float stoppingDistance = thisAgent.stoppingDistance;
            if (distance < stoppingDistance)
            {
                state = NodeState.RUNNING;

            }
            else
            {
                thisAnimal.SetAgentPath(kidnapper.transform.position);
                transformPos.LookAt(kidnapper.transform.position);

                // make it follow its kidnapper
                state = NodeState.RUNNING;
            }

           
        }

        return state;
    }

    protected override void OnReset()
    {
    }
}
