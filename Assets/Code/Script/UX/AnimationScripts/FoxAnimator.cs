using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxAnimator : AnimalAnimator
{
    //Animation variables
    private const string FOX_IDLE = "Fox_Idle";
    private const string FOX_WALK = "Fox_Walk";
    private const string FOX_PANIC = "Fox_Panic";

    private void Awake()
    {
        animationFlags = new Dictionary<string, bool>();
        animationFlags.Add("move", inMovingAnimation);
        animationFlags.Add("panic", inPanicAnimation);

        //Initialize Animation Names
        animations = new Dictionary<string, CharacterAnimation>();
        animations.Add("idle", idleAnimation);
        animations.Add("move", moveAnimation);
        animations.Add("panic", panicAnimation);
    }
}
