using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalAnimator : OurAnimator
{
    public new Animator animator;
    public new int IfWalkingHash;
    public int IfEatingHash;
    public int IfPanickingHash;
    public int IfSwimmingHash;

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
        IfPanickingHash = Animator.StringToHash("IfPanicking");
        IfEatingHash = Animator.StringToHash("IfEating");
        IfSwimmingHash = Animator.StringToHash("IfSwimming");
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
            animator.SetBool(IfEatingHash, false);
            animator.SetBool(IfPanickingHash, false);
            animator.SetBool(IfSwimmingHash, false);
        }
    }
}
