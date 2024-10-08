using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasticBagMonsterAnimator : EnemyAnimator
{
    //Animation variables
    private bool inChokeAnim = false;

    private const string BAG_IDLE = "Bag_Idle";
    private const string BAG_FLOAT = "Bag_Float";
    private const string BAG_CHOKE = "Bag_Choke";
    private const string BAG_HURT = "Bag_Hurt";
    private const string BAG_DIE = "Bag_Die";

    [SerializeField] protected CharacterAnimation chokeAnimation;

    // Start is called before the first frame update
    void Awake()
    {
        //Initialize animation flags
        animationFlags = new Dictionary<string, bool>();
        animationFlags.Add("move", inMovingAnimation);
        animationFlags.Add("choke", inChokeAnim);
        animationFlags.Add("hurt", inHurtAnimation);
        animationFlags.Add("die", inDyingAnimation);

        //Initialize a dictionary with all our animation state names
        animations = new Dictionary<string, CharacterAnimation>();
        animations.Add("idle", idleAnimation);
        animations.Add("move", moveAnimation);
        animations.Add("choke", chokeAnimation);
        animations.Add("hurt", hurtAnimation);
        animations.Add("die", deathAnimation);

    }

}
