using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Actor : MonoBehaviour
{
    [Header("Actor variables")]
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected OurAnimator animator;
    

    //Sets the agent path while also turning on their movement animation flag
    public void SetAgentPath(Vector3 targetLocation)
    {
        if(agent != null)
        {
            if (!agent.enabled)
            {
                agent.enabled = true;
            }
            agent.SetDestination(targetLocation);
            animator.SetAnimationFlag("move", true);
        }
    }

    //Resets the agent's path and also turns off their movement animation
    public void ResetAgentPath()
    {
        if(agent != null)
        {
            if (agent.enabled && agent.hasPath)
            {
                agent.ResetPath();
                animator.SetAnimationFlag("move", false);
            }
        }
    }

    public OurAnimator GetAnimator()
    {
        return animator;
    }

    public void SetInSoftLock(bool softLock)
    {
        animator.SetInSoftLock(softLock);
    }
}
