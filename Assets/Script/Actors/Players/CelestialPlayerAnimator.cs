using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialPlayerAnimator : PlayerAnimator
{
    public int IfAttackingHash;
    public int IfCastingSpellHash;
    public int IfDodgingHash;

    // Start is called before the first frame update
    void Start()
    {
        SetAnimations();
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void SetAnimations()
    {
        animator = GameObject.Find("Celestial_geo").GetComponentInChildren<Animator>();
        IfWalkingHash = Animator.StringToHash("IfWalking");
        IfAttackingHash = Animator.StringToHash("IfAttacking");
        IfDyingHash = Animator.StringToHash("IfDying");
        IfTakingHitHash = Animator.StringToHash("IfTakingHit");
        IfCastingSpellHash = Animator.StringToHash("IfCastingSpell");
        IfDodgingHash = Animator.StringToHash("IfDodging");
    }

    public override void ToggleSetWalk()
    {
        if (animator.GetBool(IfWalkingHash))
        {
            animator.SetBool(IfWalkingHash, false);
        }
        else if (!animator.GetBool(IfWalkingHash))
        {
            animator.SetBool(IfWalkingHash, true);
            animator.SetBool(IfAttackingHash, false);
            animator.SetBool(IfCastingSpellHash, false);
        }
    }
}
