using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilMonster : KidnappingEnemy
{
    protected override void Awake()
    {
        animator = GetComponent<OilMonsterAnimator>();
        base.Awake();
    }

    
}
