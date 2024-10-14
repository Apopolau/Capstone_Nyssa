using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SproutAnimationEvents : MonoBehaviour
{
    EarthPlayer earthPlayer;

    private void Awake()
    {
        earthPlayer = GameObject.Find("EarthPlayer").GetComponent<EarthPlayer>();
    }

    public void TurnOnSoftLockEnd()
    {
        earthPlayer.SetInSoftLock(true);
    }

    public void TurnOffSoftLockEnd()
    {
        earthPlayer.SetInSoftLock(false);
    }

    public void ApplyBarrier()
    {
        earthPlayer.TurnShieldOn();
        earthPlayer.BarrierWrapUp();
    }

    public void PlantPlant()
    {
        earthPlayer.HandlePlantAnimation();
    }

    public void BuildEnd()
    {
        earthPlayer.BuildWrapUp();
    }
}
