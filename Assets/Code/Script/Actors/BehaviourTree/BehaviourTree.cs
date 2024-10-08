using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//From https://github.com/MinaPecheux/UnityTutorials-BehaviourTrees/blob/master/Assets/Scripts/BehaviorTree/Node.cs
namespace BehaviourTree
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }
}
