using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmogAnimationEvents : MonoBehaviour
{
    SmogMonster smogMonster;

    private void Awake()
    {
        smogMonster = this.gameObject.GetComponentInParent<SmogMonster>();
    }

    public void TurnOnCollisionFrames()
    {
        Debug.Log("Turning on attack collision frames");
        smogMonster.AttackCollisionOn();
    }

    public void TurnOffCollisionFrames()
    {
        Debug.Log("Turning off attack collision frames");
        smogMonster.AttackCollisionOff();
    }
}
