using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmogMonster : KidnappingEnemy
{
    protected override void Awake()
    {
        animator = GetComponent<SmogMonsterAnimator>();
        base.Awake();
    }
}
