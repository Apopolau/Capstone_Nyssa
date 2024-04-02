using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weather State", menuName = "ManagerObject/WeatherManagers/WeatherState")]
public class WeatherState : ScriptableObject
{
    public enum TimeOfDay { DAYBREAK, DAY, EVENING, NIGHT };
    public TimeOfDay currentTimeOfDay = TimeOfDay.DAY;

    public enum AcidRainState { NONE, LIGHT, HEAVY};
    public AcidRainState acidRainState = AcidRainState.NONE;

    public enum SkyState { CLEAR, RAINY};
    public SkyState skyState = SkyState.CLEAR;

    public bool dayTime;

    public void SetDayTime(bool isDay)
    {
        dayTime = isDay;
    }

    public TimeOfDay GetTimeOfDay()
    {
        return currentTimeOfDay;
    }

    public void SetTimeOfDay(TimeOfDay timeOfDay)
    {
        currentTimeOfDay = timeOfDay;
    }
}
