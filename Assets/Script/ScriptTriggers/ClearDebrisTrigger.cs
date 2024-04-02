using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearDebrisTrigger : MonoBehaviour
{
    [SerializeField] private LevelTwoProgress levelTwoProgress;
    [SerializeField] private DialogueTrigger prePowerApproachDialogue;
    [SerializeField] private DialogueTrigger postPowerApproachDialogue;
    bool hasTriggered1;
    bool hasTriggered2;

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
}
