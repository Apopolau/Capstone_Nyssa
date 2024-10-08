using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAnimator : OurAnimator
{
    [Header("Animation State Machine states")]
    protected bool inHurtAnimation = false;
    protected bool inDyingAnimation = false;

    [Header("Player Animation Stats")]
    [SerializeField] protected CharacterAnimation hurtAnimation;
    [SerializeField] protected CharacterAnimation deathAnimation;
}
