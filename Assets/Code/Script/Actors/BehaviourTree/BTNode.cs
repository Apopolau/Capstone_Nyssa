using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public abstract class BTNode
{

    public int EvaluationCount;
    public string Name;

    protected NodeState state;

    public BTNode parent;
    protected List<BTNode> children = new List<BTNode>();

    private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

    protected BTNode()
    {
        parent = null;
    }

    protected BTNode(List<BTNode> children)
    {
        foreach (BTNode child in children)
        {
            _Attach(child);
        }
    }

    protected BTNode(BTNode child)
    {
        _Attach(child);
    }

    public virtual NodeState Run()
    {
        NodeState nodeState = OnRun();

        EvaluationCount++;

        if (nodeState != NodeState.RUNNING)
        {
            Reset();
        }

        return nodeState;
    }

    public void Reset()
    {
        EvaluationCount = 0;
        OnReset();
    }

    private void _Attach(BTNode node)
    {
        node.parent = this;
        children.Add(node);
    }

    //public virtual NodeState Evaluate() => NodeState.FAILURE;

    public void SetData(string key, object value)
    {
        _dataContext[key] = value;
    }

    public object GetData(string key)
    {
        object value = null;
        if (_dataContext.TryGetValue(key, out value))
            return value;

        BTNode node = parent;
        while (node != null)
        {
            value = node.GetData(key);
            if (value != null)
                return value;
            node = node.parent;
        }
        return null;
    }

    public bool ClearData(string key)
    {
        if (_dataContext.ContainsKey(key))
        {
            _dataContext.Remove(key);
            return true;
        }

        BTNode node = parent;
        while (node != null)
        {
            bool cleared = node.ClearData(key);
            if (cleared)
                return true;
            node = node.parent;
        }
        return false;
    }

    protected abstract NodeState OnRun();
    protected abstract void OnReset();
}
