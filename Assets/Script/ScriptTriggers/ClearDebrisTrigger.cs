using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearDebrisTrigger : MonoBehaviour
{
    [SerializeField] private LevelTwoProgress levelTwoProgress;
    [SerializeField] private DialogueTrigger prePowerApproachDialogue;
    [SerializeField] private DialogueTrigger postPowerApproachDialogue;
    bool hasTriggered1 = false;
    bool hasTriggered2 = false;

    WaitForSeconds debrisClearTime = new WaitForSeconds(4.542f);
    bool isAnimated = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if earthPlayer enterted area
        if ((other.CompareTag("Player1") && other is CapsuleCollider) || (other.CompareTag("Player2") && other is CapsuleCollider))
        {
            if (!hasTriggered1 && !levelTwoProgress.GetMoonTideStatus())
            {
                prePowerApproachDialogue.TriggerDialogue();
                hasTriggered1 = true;
            }
            else if(!hasTriggered2 && levelTwoProgress.GetMoonTideStatus())
            {
                postPowerApproachDialogue.TriggerDialogue();
                hasTriggered2 = true;
            }
            
            // Destroy the GameObject collider
            //Destroy(gameObject);
        }
    }

    private void ClearDebris()
    {
        if (isAnimated)
        {
            //Debug.Log("moving downward");
            this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x, this.gameObject.transform.localPosition.y - 0.3f, this.gameObject.transform.localPosition.z);
        }
    }

    private IEnumerator LetTimerRun()
    {

        yield return debrisClearTime;
        isAnimated = false;
    }
}
