using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToAnimationObject : MonoBehaviour
{
    [SerializeField] private List<DialogueAnimation> animationToSet;

    private void OnEnable()
    {
        foreach(DialogueAnimation animation in animationToSet)
        {
            animation.SetAnimatorToThis(this.gameObject);
        }
    }
}
