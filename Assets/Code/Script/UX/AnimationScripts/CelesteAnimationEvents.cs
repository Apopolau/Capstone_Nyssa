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
        //Debug.Log("Starting iFrames");
        celestialPlayer.StartIFrames();
    }

    public void StopIFrames()
    {
        //Debug.Log("Stopping iFrames");
        celestialPlayer.EndIFrames();
    }

    public void StopDodgeMove()
    {
        //Debug.Log("Stopping dodge move");
        celestialPlayer.StopDodgeMovement();
    }

    public void TurnOnCollisionFrames()
    {
        //Debug.Log("Turning on attack collision frames");
        celestialPlayer.AttackCollisionOn();
    }

    public void TurnOffCollisionFrames()
    {
        //Debug.Log("Turning off attack collision frames");
        celestialPlayer.AttackCollisionOff();
    }

    public void TurnOnSoftLockEnd()
    {
        celestialPlayer.SetInSoftLock(true);
    }

    public void TurnOffSoftLockEnd()
    {
        celestialPlayer.SetInSoftLock(false);
    }
}
