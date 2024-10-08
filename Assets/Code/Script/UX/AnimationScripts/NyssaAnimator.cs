using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NyssaAnimator : AnimalAnimator
{
    private void Awake()
    {
        animationFlags = new Dictionary<string, bool>();
        animationFlags.Add("move", inMovingAnimation);

        
    }
}
