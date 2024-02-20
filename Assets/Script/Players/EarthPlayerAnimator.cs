using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthPlayerAnimator : OurAnimator
{
    public int IfPlantingHash;

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
        }
    }
}
