using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weather State", menuName = "Manager Object/Weather Managers/Weather State")]
public class WeatherState : ScriptableObject
{
    [SerializeField] LevelEventManager levelEventManager;

    public enum TimeOfDay { DAYBREAK, DAY, EVENING, NIGHT };
    private TimeOfDay currentTimeOfDay = TimeOfDay.DAY;

    public enum AcidRainState { NONE, LIGHT, HEAVY};
    private AcidRainState acidRainState = AcidRainState.NONE;

    public enum SkyState { CLEAR, RAINY};
    private SkyState skyState = SkyState.CLEAR;

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

    public SkyState GetSkyState()
    {
        return skyState;
    }

    public void SetRainyState(bool isRaining)
    {
        if (isRaining)
        {
            skyState = SkyState.RAINY;
            levelEventManager.ChangeRainAmbience(true);
        }
        else
        {
            skyState = SkyState.CLEAR;
            levelEventManager.ChangeRainAmbience(false);
        }
    }

    public AcidRainState GetAcidRainState()
    {
        return acidRainState;
    }

    public void SetAcidState(AcidRainState incAcidRainState)
    {
        acidRainState = incAcidRainState;
    }

    public void SetEventManager(LevelEventManager levelEventManager)
    {
        this.levelEventManager = levelEventManager;
    }
}
