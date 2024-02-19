using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OurAnimator : MonoBehaviour
{
    protected Animator animator;
    protected int IfWalkingHash;

    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponentInChildren<Animator>();
        //IfWalkingHash = Animator.StringToHash("IfWalking");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected abstract void SetAnimations();
    public abstract void ToggleSetWalk();
}
