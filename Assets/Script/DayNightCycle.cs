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
