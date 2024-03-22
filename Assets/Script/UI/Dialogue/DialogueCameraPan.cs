using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Camera Pan", menuName = "Dialogue/CameraPan")]
public class DialogueCameraPan : DialogueEvent
{
    //Use this to determine if we're panning to a particular entity, or location
    public enum PanType { OBJECT, LOCATION };
    public PanType panType;

    //You'll set one of these two based on previous setting
    [SerializeField] GameObject panToObject;
    [SerializeField] Vector3 pointToPanTo;
    [SerializeField] Vector3 panOffset;

    //Controls how fast we pan, and how far we zoom in
    [SerializeField] float panSpeed;
    [SerializeField] float zoomSpeed;
    [SerializeField] float zoomAmount;

    WaitForSecondsRealtime animationTime;
    [SerializeField] float f_animationTime;

    private void OnEnable()
    {
        animationTime = new WaitForSecondsRealtime(f_animationTime);
    }

    public float GetPanSpeed()
    {
        return panSpeed;
    }

    public float GetZoomSpeed()
    {
        return zoomSpeed;
    }

    public float GetZoomAmount()
    {
        return zoomAmount;
    }

    public GameObject GetPanObject()
    {
        return panToObject;
    }

    public Vector3 GetPanLocation()
    {
        return pointToPanTo;
    }

    public PanType GetPanType()
    {
        return panType;
    }

    public void SetPanToThis(GameObject objectToSet)
    {
        panToObject = objectToSet;
    }

    public Vector3 GetPanOffset()
    {
        return panOffset;
    }

    public WaitForSecondsRealtime GetAnimationTime()
    {
        return animationTime;
    }
}
