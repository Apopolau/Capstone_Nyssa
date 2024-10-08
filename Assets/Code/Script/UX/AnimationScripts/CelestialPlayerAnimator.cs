using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialPlayerAnimator : PlayerAnimator
{
    [Header("Animation State Machine Flags")]
    private bool inDodgeAnimation;
    private bool inAttackAnimation;
    private bool inCastAnimation;

    [Header("Animation names")]
    private const string CELESTE_IDLE = "Celeste_Idle";
    private const string CELESTE_WALK = "Celeste_Walk";
    private const string CELESTE_DODGE = "Celeste_Dodge";
    private const string CELESTE_ATTACK = "Celeste_Attack";
    private const string CELESTE_CAST = "Celeste_Cast";
    private const string CELESTE_HURT = "Celeste_Hurt";
    private const string CELESTE_DIE = "Celeste_Die";

    [Header("Character Animations")]
    [SerializeField] private CharacterAnimation dodgeAnimation;
    [SerializeField] private CharacterAnimation attackAnimation;
    [SerializeField] private CharacterAnimation castAnimation;
    

    // Start is called before the first frame update
    void Awake()
    {
        //Initialize a dictionary of our animation flags
        animationFlags = new Dictionary<string, bool>();
        animationFlags.Add("move", inMovingAnimation);
        animationFlags.Add("dodge", inDodgeAnimation);
        animationFlags.Add("attack", inAttackAnimation);
        animationFlags.Add("cast", inCastAnimation);
        animationFlags.Add("hurt", inHurtAnimation);
        animationFlags.Add("die", inDyingAnimation);

        //Initialize a dictionary with all our animation state names
        animations = new Dictionary<string, CharacterAnimation>();
        animations.Add("idle", idleAnimation);
        animations.Add("move", moveAnimation);
        animations.Add("dodge", dodgeAnimation);
        animations.Add("attack", attackAnimation);
        animations.Add("cast", castAnimation);
        animations.Add("hurt", hurtAnimation);
        animations.Add("die", deathAnimation);
    }

    
}
