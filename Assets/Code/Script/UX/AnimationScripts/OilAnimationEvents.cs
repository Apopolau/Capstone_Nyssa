using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilAnimationEvents : MonoBehaviour
{
    OilMonster oilMonster;

    private void Awake()
    {
        oilMonster = this.gameObject.GetComponentInParent<OilMonster>();
    }

    public void TurnOnCollisionFrames()
    {
        //Debug.Log("Turning on attack collision frames");
        oilMonster.AttackCollisionOn();
    }

    public void TurnOffCollisionFrames()
    {
        //Debug.Log("Turning off attack collision frames");
        oilMonster.AttackCollisionOff();
    }
}
