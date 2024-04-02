using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmogEncounterTrigger : MonoBehaviour
{
    public DialogueTrigger smogEncounterDialogue;

    private void OnTriggerEnter(Collider other)
    {
        // Check if earthPlayer enterted area
        if ((other.CompareTag("Player1") && other is CapsuleCollider) || (other.CompareTag("Player2") && other is CapsuleCollider))
        {
            smogEncounterDialogue.TriggerDialogue();
            // Destroy the GameObject collider
            Destroy(gameObject);
        }
    }
}
