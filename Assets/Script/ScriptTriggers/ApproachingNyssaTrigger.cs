using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachingNyssaTrigger : MonoBehaviour
{
    public DialogueTrigger approachNyssaDialogue;

    private void OnTriggerEnter(Collider other)
    {
        // Check if earthPlayer enterted area
        if ((other.CompareTag("Player1") && other is CapsuleCollider) || (other.CompareTag("Player2") && other is CapsuleCollider))
        {
            approachNyssaDialogue.TriggerDialogue();
            // Destroy the GameObject collider
            Destroy(gameObject);
        }
    }
}
