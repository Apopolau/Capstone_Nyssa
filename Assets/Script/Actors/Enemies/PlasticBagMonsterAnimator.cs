using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasticBagMonsterAnimator : OurAnimator
{
    public int IfChokingHash;
    public int IfDyingHash;
    public int IfTakingHitHash;

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
        IfWalkingHash = Animator.StringToHash("IfWalking");
        IfChokingHash = Animator.StringToHash("IfChoking");
        IfDyingHash = Animator.StringToHash("IfDying");
        IfTakingHitHash = Animator.StringToHash("IfTakingHit");
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
            animator.SetBool(IfChokingHash, false);
            animator.SetBool(IfTakingHitHash, false);
        }
    }
}
