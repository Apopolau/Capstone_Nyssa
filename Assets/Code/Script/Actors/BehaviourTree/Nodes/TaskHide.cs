using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class TaskHide : BTNode
{
    Animal animal;

    public TaskHide(Animal animal)
    {
        this.animal = animal;
    }

    protected override NodeState OnRun()
    {
        if (animal.GetComponent<Animal>())
        {
            if (animal.GetComponent<Animal>().GetIsKidnapped())
            {
                animal.SetIsHiding(false);
                return NodeState.FAILURE;
            }
        }

        if (animal.GetWeatherManager().dayTime || animal.GetIsEscorted())
        {
            animal.SetIsHiding(false);
            return NodeState.SUCCESS;
        }

        animal.SetIsHiding(true);
        state = NodeState.RUNNING;
        return state;
    }

    protected override void OnReset() { }
}
