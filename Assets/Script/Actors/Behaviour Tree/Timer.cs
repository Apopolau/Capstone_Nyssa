using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

//Tells the AI to wait a certain time after starting a node to finish
//Use this for tasks that should take a certain amount of time to finish
public class Timer : Decorator
{
    private float m_StartTime;
    private bool m_UseFixedTime;
    private float m_TimeToWait;

    public Timer(float timeToWait, BTNode childNode, bool useFixedTime = false) :
        base(childNode)
    {
        m_UseFixedTime = useFixedTime;
        m_TimeToWait = timeToWait;
    }

    protected override void OnReset() { }

    protected override NodeState OnRun()
    {
        if (children.Count == 0 || children[0] == null)
        {
            return NodeState.FAILURE;
        }

        NodeState originalStatus = (children[0] as BTNode).Run();

        if (originalStatus == NodeState.FAILURE)
        {
            return NodeState.FAILURE;
        }

        if (EvaluationCount == 0)
        {
            m_StartTime = m_UseFixedTime ? Time.fixedTime : Time.time;
        }

        float elapsedTime = Time.fixedTime - m_StartTime;

        if (elapsedTime > m_TimeToWait)
        {
            return NodeState.SUCCESS;
        }
        return NodeState.RUNNING;
    }

}
