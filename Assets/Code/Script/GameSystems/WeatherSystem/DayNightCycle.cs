using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//References https://www.youtube.com/watch?v=2H6hD-rH6wM
//
public class DayNightCycle : MonoBehaviour
{
    [Header("Major References")]
    [SerializeField] private HUDModel model;
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

    //These are what we want to set
    private Gradient currentDirectionalLightColor;
    private Gradient currentDirectionalLightIntensity;
    private Gradient currentSkyColor;
    private Gradient currentEquatorColor;

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
        weatherState.SetTimeOfDay(WeatherState.TimeOfDay.DAY);

        SwapSkyColours(false);
    }

    private void OnEnable()
    {
        UpdateLighting();
    }

    private void Start()
    {
        weatherState.SetRainyState(false);
    }

    void Update()
    {
        if (Application.isPlaying && timeOfDayChanges)
        {
            CalculateTimeOfDay();
        }

    }

    private void UpdateSun()
    {

    }

    private void UpdateTimeOfDay()
    {
        //float timeFraction = timeOfDay / 24;

        if (timeOfDay < 2)
        {
            weatherState.SetTimeOfDay(WeatherState.TimeOfDay.DAYBREAK);
            weatherState.SetDayTime(true);
        }
        else if (timeOfDay > 2 && timeOfDay < 16)
        {//////20.83----25
            weatherState.SetTimeOfDay(WeatherState.TimeOfDay.DAY);
            weatherState.SetDayTime(true);
        }
        else if (timeOfDay > 16 && timeOfDay < 18)
        {
            weatherState.SetTimeOfDay(WeatherState.TimeOfDay.EVENING);
            weatherState.SetDayTime(true);
        }
        else if (timeOfDay > 18)
        {//83.3 ------ 87.5
            weatherState.SetTimeOfDay(WeatherState.TimeOfDay.NIGHT);
            weatherState.SetDayTime(false);
        }
        /*
        if (weatherState.acidRainState == WeatherState.AcidRainState.NONE)
        {
            RenderSettings.ambientEquatorColor = equatorColor.Evaluate(timeOfDay / 24);
            RenderSettings.ambientSkyColor = equatorColor.Evaluate(timeOfDay / 24);
            directionalLight.color = directionalLightColor.Evaluate(timeOfDay / 24);
            float H, S, V;
            Color.RGBToHSV(directionalLightIntensity.Evaluate(timeOfDay / 24), out H, out S, out V);
            directionalLight.intensity = V;
        }
         else if(weatherState.acidRainState == WeatherState.AcidRainState.LIGHT || weatherState.acidRainState == WeatherState.AcidRainState.HEAVY)
        {
            RenderSettings.ambientEquatorColor = dirtyEquatorColor.Evaluate(timeOfDay / 24);
            RenderSettings.ambientSkyColor = dirtyEquatorColor.Evaluate(timeOfDay / 24);
            directionalLight.color = dirtyDirectionalLightColor.Evaluate(timeOfDay / 24);
            float H, S, V;
            Color.RGBToHSV(dirtyDirectionalLightIntensity.Evaluate(timeOfDay / 24), out H, out S, out V);
            directionalLight.intensity = V;
        }
        */

        

        //print(directionalLightColor.Evaluate(timeFraction));
        //print(V);
    }

    private void CalculateTimeOfDay()
    {
        timeRaw += Time.deltaTime;
        scaledTime = timeRaw / timeScale;
        nightsPassed = (int)scaledTime / 24;
        timeOfDay = scaledTime % 24;
        UpdateTimeOfDay();
        UpdateLighting();
        model.GetManager().UpdateDayNightDisplay(timeOfDay);
    }

    public int NightsPassed()
    {
        return nightsPassed;
    }

    public void SwapSkyColours(bool isClean)
    {
        if (isClean)
        {
            currentEquatorColor = equatorColor;
            currentSkyColor = skyColor;
            currentDirectionalLightColor = directionalLightColor;
            currentDirectionalLightIntensity = directionalLightIntensity;
            UpdateLighting();
        }
        else
        {
            currentEquatorColor = dirtyEquatorColor;
            currentSkyColor = dirtySkyColor;
            currentDirectionalLightColor = dirtyDirectionalLightColor;
            currentDirectionalLightIntensity = dirtyDirectionalLightIntensity;
            UpdateLighting();
        }
    }

    public void UpdateLighting()
    {
        RenderSettings.ambientEquatorColor = currentEquatorColor.Evaluate(timeOfDay / 24);
        RenderSettings.ambientSkyColor = currentEquatorColor.Evaluate(timeOfDay / 24);
        directionalLight.color = currentDirectionalLightColor.Evaluate(timeOfDay / 24);
        float H, S, V;
        Color.RGBToHSV(currentDirectionalLightIntensity.Evaluate(timeOfDay / 24), out H, out S, out V);
        directionalLight.intensity = V;
    }

    public float GetTimeOfDay()
    {
        return timeOfDay;
    }
}
