using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAnimator : OurAnimator
{
    public int IfDyingHash;
    public int IfTakingHitHash;

    // Start is called before the first frame update
    void Start()
    {
        SetAnimations();
    }

    // Update is called once per frame
    void Update()
    {

    }

}
