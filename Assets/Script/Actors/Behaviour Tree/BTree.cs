using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public abstract class BTree : MonoBehaviour
{
    private BTNode _root = null;

    protected void Start()
    {
        _root = SetupTree();
    }

    private void Update()
    {
        if (_root != null)
            _root.Run();
    }

    protected abstract BTNode SetupTree();
}
