using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelesteAnimationEvents : MonoBehaviour
{
    CelestialPlayer celestialPlayer;

    private void Awake()
    {
        celestialPlayer = GameObject.Find("CelestialPlayer").GetComponent<CelestialPlayer>();
    }

    public void StartIFrames()
    {
        Debug.Log("Starting iFrames");
        celestialPlayer.StartIFrames();
    }

    public void StopIFrames()
    {
        Debug.Log("Stopping iFrames");
        celestialPlayer.EndIFrames();
    }

    public void StopDodgeMove()
    {
        Debug.Log("Stopping dodge move");
        celestialPlayer.ToggleDodgeMoving();
    }
}
