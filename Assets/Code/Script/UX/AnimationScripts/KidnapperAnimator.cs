using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KidnapperAnimator : EnemyAnimator
{
    //Animation flags
    protected bool inAttackAnim = false;

    [SerializeField] protected CharacterAnimation attackAnimation;

}
