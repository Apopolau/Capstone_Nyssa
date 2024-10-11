using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilMonster : KidnappingEnemy
{
    protected override void Awake()
    {
        InitializeAnimator();
        base.Awake();
    }

    protected override void InitializeAnimator()
    {
        animator = this.GetComponent<OilMonsterAnimator>();
    }
}
