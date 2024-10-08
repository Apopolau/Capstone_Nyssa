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
        animal.SetIsHiding(true);
        state = NodeState.SUCCESS;
        return state;
    }

    protected override void OnReset() { }
}
