using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthPlayerAnimator : MonoBehaviour
{
    public Animator animator;
    public int IfWalkingHash;
    public int IfPlantingHash;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        IfWalkingHash = Animator.StringToHash("IfWalking");
        IfPlantingHash = Animator.StringToHash("IfPlanting");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
