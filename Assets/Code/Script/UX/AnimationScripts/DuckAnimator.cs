using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckAnimator : AnimalAnimator
{
    //Animation variables
    private bool inSwimAnimation = false;

    private const string DUCK_IDLE = "Duck_Idle";
    private const string DUCK_WALK = "Duck_Walk";
    private const string DUCK_SWIM = "Duck_Swim";
    private const string DUCK_EAT = "Duck_Eat";
    private const string DUCK_PANIC = "Duck_Panic";

    [SerializeField] CharacterAnimation swimAnimation;

    private void Awake()
    {
        animationFlags = new Dictionary<string, bool>();
        animationFlags.Add("move", inMovingAnimation);
        animationFlags.Add("swim", inSwimAnimation);
        animationFlags.Add("eat", inEatingAnimation);
        animationFlags.Add("panic", inPanicAnimation);

        //Initialize animation names
        animations = new Dictionary<string, CharacterAnimation>();
        animations.Add("idle", idleAnimation);
        animations.Add("move", moveAnimation);
        animations.Add("swim", swimAnimation);
        animations.Add("eat", eatAnimation);
        animations.Add("panic", panicAnimation);
    }
}
