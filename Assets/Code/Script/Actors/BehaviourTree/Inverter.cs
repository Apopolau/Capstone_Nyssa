using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

//An inverter is used to decide what should be done in the case of a failure on a check
//If no key, bash door down instead of unlocking, for instance
public class Inverter : Decorator
{
    public Inverter(BTNode childNode) : base(childNode) { }

    protected override void OnReset() { }
    protected override NodeState OnRun()
    {
        if (children.Count == 0 || children[0] == null)
        {
            return NodeState.FAILURE;
        }

        NodeState originalStatus = (children[0] as BTNode).Run();

        switch (originalStatus)
        {
            case NodeState.FAILURE:
                return NodeState.SUCCESS;
            case NodeState.SUCCESS:
                return NodeState.FAILURE;
        }

        return originalStatus;
    }
}
