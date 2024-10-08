using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class CheckIfNight : BTCondition
{
    WeatherState weatherState;
    public CheckIfNight(WeatherState incWeatherState)
    {
        weatherState = incWeatherState;
    }

    //Returns true if it's night time
    protected override NodeState OnRun()
    {
        if (weatherState.GetTimeOfDay() == WeatherState.TimeOfDay.NIGHT)
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
