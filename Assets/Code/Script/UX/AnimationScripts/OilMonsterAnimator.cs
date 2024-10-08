using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilMonsterAnimator : KidnapperAnimator
{
    private const string OIL_IDLE = "Oil_Idle";
    //Note there's no separate movement animation
    private const string OIL_MOVE = "Oil_Idle";
    private const string OIL_ATTACK = "Oil_Attack";
    private const string OIL_HURT = "Oil_Hurt";
    private const string OIL_DIE = "Oil_Die";

    private void Awake()
    {
        animationFlags = new Dictionary<string, bool>();
        animationFlags.Add("move", inMovingAnimation);
        animationFlags.Add("attack", inAttackAnim);
        animationFlags.Add("hurt", inHurtAnimation);
        animationFlags.Add("die", inDyingAnimation);

        //Initialize a dictionary with all our animation state names
        animations = new Dictionary<string, CharacterAnimation>();
        animations.Add("idle", idleAnimation);
        animations.Add("move", moveAnimation);
        animations.Add("attack", attackAnimation);
        animations.Add("hurt", hurtAnimation);
        animations.Add("die", deathAnimation);
    }

}
