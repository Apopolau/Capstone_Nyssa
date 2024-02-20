using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weather State", menuName = "WeatherManagers/WeatherState")]
public class WeatherState : ScriptableObject
{
    public enum TimeOfDay { DAYBREAK, DAY, EVENING, NIGHT };
    public TimeOfDay currentTimeOfDay = TimeOfDay.DAY;

    public enum AcidRainState { NONE, LIGHT, HEAVY};
    public AcidRainState acidRainState = AcidRainState.NONE;

    public enum SkyState { CLEAR, RAINY};
    public SkyState skyState = SkyState.CLEAR;
}
