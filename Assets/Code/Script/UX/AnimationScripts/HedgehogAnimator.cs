using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HedgehogAnimator : AnimalAnimator
{
    //Animation variables

    private bool inEatAnimation = false;

    private const string HEDGEHOG_IDLE = "Hedgehog_Idle";
    private const string HEDGEHOG_WALK = "Hedgehog_Walk";
    private const string HEDGEHOG_EAT = "Hedgehog_Eat";
    private const string HEDGEHOG_PANIC = "Hedgehog_Panic";

    private void Awake()
    {
        //Initialize animation flags
        animationFlags = new Dictionary<string, bool>();
        animationFlags.Add("move", inMovingAnimation);
        animationFlags.Add("eat", inEatAnimation);
        animationFlags.Add("panic", inPanicAnimation);

        //Initialize animation names
        animations = new Dictionary<string, CharacterAnimation>();
        animations.Add("idle", idleAnimation);
        animations.Add("move", moveAnimation);
        animations.Add("eat", eatAnimation);
        animations.Add("panic", panicAnimation);
    }
}
