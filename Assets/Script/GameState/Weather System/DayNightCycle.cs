using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//References https://www.youtube.com/watch?v=2H6hD-rH6wM
//
public class DayNightCycle : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Lights in Scene")]
    [SerializeField] private Light directionalLight;

    [Header("Time of Day")]
    [SerializeField, Range(0, 24)] private float timeOfDay;
    [SerializeField] private float timeRaw;
    [SerializeField] private float scaledTime;
    [SerializeField] private float timeScale;
    [SerializeField] private int nightsPassed;

    [Header("Clean World Light Presets")]
    [SerializeField] private Gradient directionalLightColor;
    [SerializeField] private Gradient directionalLightIntensity;
    [SerializeField] private Gradient skyColor;
    [SerializeField] private Gradient equatorColor;

    [Header("Dirty World Light Presets")]
    [SerializeField] private Gradient dirtyDirectionalLightColor;
    [SerializeField] private Gradient dirtyDirectionalLightIntensity;
    [SerializeField] private Gradient dirtySkyColor;
    [SerializeField] private Gradient dirtyEquatorColor;

    //Stores data on the weather state
    [SerializeField] WeatherState weatherState;

    bool timeOfDayChanges = false;

    //Level 1 Day colour clean: FFFFED
    //Level 1 Day colour polluted: BCBC9A

    //Level 2 Day colour clean: FFFFED
    //Level 2 Day colour polluted: BCBC9A
    //Level 2 Night colour clean: 38364D
    //Level 2 Night colour polluted: 291927

    //Level 3 Day colour clean: FFFFED
    //Level 3 Day colour polluted: BCBC9A
    //Level 3 Night colour clean: 38364D
    //Level 3 Night colour polluted: 291927

    //Level 4 Day colour clean: FFFFED
    //Level 4 Day colour polluted: BCBC9A
    //Level 4 Night colour clean: 38364D
    //Level 4 Night colour polluted: 291927

    // Update is called once per frame

    private void Awake()
    {
        if(GetComponent<MainManagerScript>().levelManager.currentLevel == 1)
        {
            timeOfDayChanges = false;
        }
        else
        {
            timeOfDay = 10;
            timeOfDayChanges = true;
        }
        weatherState.SetDayTime(true);
        weatherState.currentTimeOfDay = WeatherState.TimeOfDay.DAY;
        weatherState.skyState = WeatherState.SkyState.CLEAR;
    }

    private void OnEnable()
    {
        UpdateLighting();
    }

    void Update()
    {
        if (Application.isPlaying && timeOfDayChanges)
        {
            /*
            timeOfDay += Time.deltaTime;
            timeOfDay %= 24;
            */
            timeRaw += Time.deltaTime;
            scaledTime = timeRaw / timeScale;
            nightsPassed = (int)scaledTime / 24;
            timeOfDay = scaledTime % 24;
            UpdateLighting();
        }

    }

    private void UpdateSun()
    {

    }

    private void UpdateLighting()
    {
        //float timeFraction = timeOfDay / 24;

        if (timeOfDay < 5)
        {
            weatherState.SetTimeOfDay(WeatherState.TimeOfDay.NIGHT);
            weatherState.SetDayTime(false);
        }
        else if (timeOfDay > 5 && timeOfDay < 6)
        {
            weatherState.SetTimeOfDay(WeatherState.TimeOfDay.DAYBREAK);
            weatherState.SetDayTime(true);
        }
        else if (timeOfDay > 6 && timeOfDay < 18)
        {
            weatherState.SetTimeOfDay(WeatherState.TimeOfDay.DAY);
            weatherState.SetDayTime(true);
        }
        else if (timeOfDay > 18 && timeOfDay < 21)
        {
            weatherState.SetTimeOfDay(WeatherState.TimeOfDay.EVENING);
            weatherState.SetDayTime(true);
        }
        else if (timeOfDay > 21)
        {
            weatherState.SetTimeOfDay(WeatherState.TimeOfDay.NIGHT);
            weatherState.SetDayTime(false);
        }

        RenderSettings.ambientEquatorColor = equatorColor.Evaluate(timeOfDay);
        RenderSettings.ambientSkyColor = equatorColor.Evaluate(timeOfDay);
        directionalLight.color = directionalLightColor.Evaluate(timeOfDay);
        float H, S, V;
        Color.RGBToHSV(directionalLightIntensity.Evaluate(timeOfDay), out H, out S, out V);
        directionalLight.intensity = V;
        //print(directionalLightColor.Evaluate(timeFraction));
        //print(V);
    }

    public int NightsPassed()
    {
        return nightsPassed;
    }
}
