using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalAnimator : OurAnimator
{
    [Header("Animation flags")]
    protected bool inEatingAnimation = false;
    protected bool inPanicAnimation = false;

    [SerializeField] protected CharacterAnimation eatAnimation;
    [SerializeField] protected CharacterAnimation panicAnimation;
}
