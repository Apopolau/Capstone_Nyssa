using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

//A sequence is a subset of actions that should all lead from one to the next, to complete an action
//If any elements in the sequence fail, the sequence fails
public class Sequence : Composite
{
    public Sequence(List<BTNode> childNodes) : base(childNodes) { }

    protected override NodeState OnRun()
    {
        NodeState childNodeStatus = (children[CurrentChildIndex] as BTNode).Run();

        switch (childNodeStatus)
        {
            case NodeState.FAILURE:
                return childNodeStatus;
            case NodeState.SUCCESS:
                CurrentChildIndex++;
                break;
        }

        if (CurrentChildIndex >= children.Count)
        {
            return NodeState.SUCCESS;
        }

        return childNodeStatus == NodeState.SUCCESS ? OnRun() : NodeState.RUNNING;
    }

    protected override void OnReset()
    {
        CurrentChildIndex = 0;

        for (int i = 0; i < children.Count; i++)
        {
            (children[i] as BTNode).Reset();
        }
    }
}
