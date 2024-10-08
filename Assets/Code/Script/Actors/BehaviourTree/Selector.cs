using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class Selector : Composite
{
    public Selector(List<BTNode> childNodes) : base(childNodes) { }

    protected override NodeState OnRun()
    {
        if (CurrentChildIndex >= children.Count)
        {
            return NodeState.FAILURE;
        }

        NodeState nodeState = (children[CurrentChildIndex] as BTNode).Run();

        switch (nodeState)
        {
            case NodeState.FAILURE:
                CurrentChildIndex++;
                break;
            case NodeState.SUCCESS:
                return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
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
