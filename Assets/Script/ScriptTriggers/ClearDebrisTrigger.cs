using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearDebrisTrigger : MonoBehaviour
{
    public DialogueTrigger approachDebrisDialogue;
    bool hasTriggered;

    private void OnTriggerEnter(Collider other)
    {
        // Check if earthPlayer enterted area
        if ((other.CompareTag("Player1") && other is CapsuleCollider) || (other.CompareTag("Player2") && other is CapsuleCollider))
        {
            if (!hasTriggered)
            {
                approachDebrisDialogue.TriggerDialogue();
                hasTriggered = true;
            }
            
            // Destroy the GameObject collider
            //Destroy(gameObject);
        }
    }
}
