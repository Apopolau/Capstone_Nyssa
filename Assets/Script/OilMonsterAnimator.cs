using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilMonsterAnimator : MonoBehaviour
{

    public Animator animator;
    public int IfAttackingHash;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        IfAttackingHash = Animator.StringToHash("IfAttacking");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
