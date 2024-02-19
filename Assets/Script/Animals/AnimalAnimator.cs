using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalAnimator : MonoBehaviour
{
    public Animator animator;
    public int IfWalkingHash;
    public int IfEatingHash;
    public int IfPanickingHash;
    public int IfSwimmingHash;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        IfWalkingHash = Animator.StringToHash("IfWalking");
        IfPanickingHash = Animator.StringToHash("IfPanicking");
        IfEatingHash = Animator.StringToHash("IfEating");
        IfSwimmingHash = Animator.StringToHash("IfSwimming");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
