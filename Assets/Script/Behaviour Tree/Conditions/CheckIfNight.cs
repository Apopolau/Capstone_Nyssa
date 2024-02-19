using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class CheckIfNight : BTCondition
{
    DayNightCycle timeOfDayManager;
    public CheckIfNight(DayNightCycle dayNightCycle)
    {
        timeOfDayManager = dayNightCycle;
    }

    //Returns true if it's night time
    protected override NodeState OnRun()
    {
        if (timeOfDayManager.currentTimeOfDay == DayNightCycle.TimeOfDay.NIGHT)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }

    protected override void OnReset() { }
}
