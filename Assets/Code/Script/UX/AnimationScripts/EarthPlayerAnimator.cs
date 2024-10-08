using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthPlayerAnimator : PlayerAnimator
{
    [Header("Animation State Machine Elements")]
    private bool inCastBarrierAnimation = false;
    private bool inCastHealAnimation = false;
    private bool inPlantAnimation = false;
    private bool inBuildAnimation = false;
    private bool inRotateAnimation = false;
    private bool inCarryAnimation = false;

    [Header("Animation Names")]
    private const string SPROUT_IDLE = "Sprout_Idle";
    private const string SPROUT_WALK = "Sprout_Walk";
    private const string SPROUT_CASTBARRIER = "Sprout_CastBarrier";
    private const string SPROUT_CASTHEAL = "Sprout_CastHeal";
    private const string SPROUT_HURT = "Sprout_Hurt";
    private const string SPROUT_DIE = "Sprout_Die";
    private const string SPROUT_PLANT = "Sprout_Plant";
    private const string SPROUT_BUILD = "Sprout_Build";
    private const string SPROUT_ROTATE = "Sprout_Rotate";
    private const string SPROUT_CARRY = "Sprout_Carry";

    [Header("Animation clips")]
    [SerializeField] CharacterAnimation plantAnimation;
    [SerializeField] CharacterAnimation healCastAnimation;
    [SerializeField] CharacterAnimation barrierCastAnimation;
    [SerializeField] CharacterAnimation buildAnimation;
    [SerializeField] CharacterAnimation rotateAnimation;
    [SerializeField] CharacterAnimation carryAnimation;

    // Start is called before the first frame update
    protected void Awake()
    {
        //Initialize a dictionary of our animation flags
        animationFlags = new Dictionary<string, bool>();
        animationFlags.Add("move", inMovingAnimation);
        animationFlags.Add("castBarrier", inCastBarrierAnimation);
        animationFlags.Add("castHeal", inCastHealAnimation);
        animationFlags.Add("hurt", inHurtAnimation);
        animationFlags.Add("die", inDyingAnimation);
        animationFlags.Add("plant", inPlantAnimation);
        animationFlags.Add("build", inBuildAnimation);
        animationFlags.Add("rotate", inRotateAnimation);
        animationFlags.Add("carry", inCarryAnimation);

        //Initialize a dictionary with all our animation state names
        animations = new Dictionary<string, CharacterAnimation>();
        animations.Add("idle", idleAnimation);
        animations.Add("move", moveAnimation);
        animations.Add("castBarrier", barrierCastAnimation);
        animations.Add("castHeal", healCastAnimation);
        animations.Add("hurt", hurtAnimation);
        animations.Add("die", deathAnimation);
        animations.Add("plant", plantAnimation);
        animations.Add("build", buildAnimation);
        animations.Add("rotate", rotateAnimation);
        animations.Add("carry", carryAnimation);
    }

    

}
