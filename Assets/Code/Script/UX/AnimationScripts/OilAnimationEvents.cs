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
        oilMonster.AttackCollisionOn();
    }

    public void TurnOffCollisionFrames()
    {
        oilMonster.AttackCollisionOff();
    }
}
