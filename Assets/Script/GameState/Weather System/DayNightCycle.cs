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

    [Header("World Light Presets")]
    [SerializeField] private Gradient directionalLightColor;
    [SerializeField] private Gradient directionalLightIntensity;
    [SerializeField] private Gradient skyColor;
    [SerializeField] private Gradient equatorColor;

    //Stores data on the weather state
    [SerializeField] WeatherState weatherState;

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
    void Update()
    {
        if (Application.isPlaying)
        {
            timeOfDay += Time.deltaTime;
            timeOfDay %= 24;
            UpdateLighting();
        }

    }

    private void OnValidate()
    {
        UpdateLighting();
    }
    private void UpdateSun()
    {

    }
    private void UpdateLighting()
    {
        float timeFraction = timeOfDay / 24;
        if(timeFraction < 5)
        {
            weatherState.currentTimeOfDay = WeatherState.TimeOfDay.NIGHT;
        }
        else if (timeFraction > 5 && timeFraction < 6)
        {
            weatherState.currentTimeOfDay = WeatherState.TimeOfDay.DAYBREAK;
        }
        else if(timeFraction > 6 && timeFraction < 18)
        {
            weatherState.currentTimeOfDay = WeatherState.TimeOfDay.DAY;
        }
        else if(timeFraction > 18 && timeFraction < 21)
        {
            weatherState.currentTimeOfDay = WeatherState.TimeOfDay.EVENING;
        }
        else if (timeFraction > 21)
        {
            weatherState.currentTimeOfDay = WeatherState.TimeOfDay.NIGHT;
        }
        RenderSettings.ambientEquatorColor = equatorColor.Evaluate(timeFraction);
        RenderSettings.ambientSkyColor = equatorColor.Evaluate(timeFraction);
        directionalLight.color = directionalLightColor.Evaluate(timeFraction);
        float H, S, V;
        Color.RGBToHSV((directionalLightIntensity.Evaluate(timeFraction)), out H, out S, out V);
        directionalLight.intensity =V;
        //print(directionalLightColor.Evaluate(timeFraction));
        //print(V);
    }

    void Start()
    {
        
    }

   
}
