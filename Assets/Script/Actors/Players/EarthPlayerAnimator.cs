using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthPlayerAnimator : PlayerAnimator
{
    public int IfPlantingHash;
    public int IfBuildingHash;
    public int IfTurningHash;
    public int IfHealingHash;
    public int IfShieldingHash;

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
        animator = GameObject.Find("earth_geo").GetComponentInChildren<Animator>();
        IfWalkingHash = Animator.StringToHash("IfWalking");
        IfPlantingHash = Animator.StringToHash("IfPlanting");
        IfBuildingHash = Animator.StringToHash("IfBuilding");
        IfTurningHash = Animator.StringToHash("IfTurning");
        IfHealingHash = Animator.StringToHash("IfHealing");
        IfShieldingHash = Animator.StringToHash("IfShielding");
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
            animator.SetBool(IfPlantingHash, false);
            animator.SetBool(IfBuildingHash, false);
            animator.SetBool(IfTurningHash, false);
            animator.SetBool(IfHealingHash, false);
            animator.SetBool(IfShieldingHash, false);
        }
    }
}
