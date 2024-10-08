using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAnimator : OurAnimator
{
    [Header("Animation flags")]
    protected bool inHurtAnimation = false;
    protected bool inDyingAnimation = false;

    [SerializeField] protected CharacterAnimation hurtAnimation;
    [SerializeField] protected CharacterAnimation deathAnimation;
}
