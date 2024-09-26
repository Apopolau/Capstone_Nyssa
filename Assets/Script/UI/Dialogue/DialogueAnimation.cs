using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Animation", menuName = "UI/Dialogue/Animation")]
public class DialogueAnimation : DialogueEvent
{
    [SerializeField] string animationName;

    WaitForSecondsRealtime animationTime;
    [SerializeField] float animationDuration;
    WaitForSecondsRealtime delayTime;
    [SerializeField] float animationDelay;
    [SerializeField] OurAnimator targetAnimator;
    GameObject objectToAnimate;

    private void OnEnable()
    {
        delayTime = new WaitForSecondsRealtime(animationDelay);
        animationTime = new WaitForSecondsRealtime(animationDuration - 0.02f);
        //targetAnimator = objectToAnimate.GetComponent<OurAnimator>();
    }

    public string GetAnimation()
    {
        return animationName;
    }

    public WaitForSecondsRealtime GetAnimationDelay()
    {
        return delayTime;
    }

    public WaitForSecondsRealtime GetAnimationTime()
    {
        return animationTime;
    }

    public OurAnimator GetTargetAnimator()
    {
        return targetAnimator;
    }

    public void SetAnimatorToThis(GameObject objectToSet)
    {
        objectToAnimate = objectToSet;
        targetAnimator = objectToAnimate.GetComponent<OurAnimator>();
    }
}
