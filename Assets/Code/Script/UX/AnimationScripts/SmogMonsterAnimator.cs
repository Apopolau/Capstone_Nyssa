using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmogMonsterAnimator : KidnapperAnimator
{
    [Header("Smog monster variables")]
    private const string SMOG_IDLE = "Smog_Idle";
    private const string SMOG_MOVE = "Smog_Move";
    private const string SMOG_ATTACK = "Smog_Attack";
    private const string SMOG_HURT = "Smog_Hurt";
    private const string SMOG_DIE = "Smog_Die";

    // Start is called before the first frame update
    void Awake()
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
