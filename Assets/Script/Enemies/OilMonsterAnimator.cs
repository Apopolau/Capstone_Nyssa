using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilMonsterAnimator : OurAnimator
{

    public new Animator animator;
    public int IfAttackingHash;
    public new int IfWalkingHash;

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
        animator = GetComponentInChildren<Animator>();
        IfWalkingHash = Animator.StringToHash("IfWalking");
        IfAttackingHash = Animator.StringToHash("IfAttacking");
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
        }
    }
}
