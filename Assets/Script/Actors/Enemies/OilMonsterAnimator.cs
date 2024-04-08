using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilMonsterAnimator : OurAnimator
{
    public int IfAttackingHash;
    public int IfDyingHash;
    public int IfTakingHitHash;
    public int IfSmotheringHash;

    private void Awake()
    {
        SetAnimations();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void SetAnimations()
    {
        animator = GetComponentInChildren<Animator>();
        IfWalkingHash = Animator.StringToHash("IfMoving");
        IfAttackingHash = Animator.StringToHash("IfAttacking");
        IfDyingHash = Animator.StringToHash("IfDying");
        IfTakingHitHash = Animator.StringToHash("IfTakingHit");
        IfSmotheringHash = Animator.StringToHash("IfChoking");
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
            animator.SetBool(IfTakingHitHash, false);
        }
    }
    
}
