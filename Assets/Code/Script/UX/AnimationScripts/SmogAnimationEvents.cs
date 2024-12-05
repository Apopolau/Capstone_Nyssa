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
        smogMonster.AttackCollisionOn();
    }

    public void TurnOffCollisionFrames()
    {
        smogMonster.AttackCollisionOff();
    }
}
