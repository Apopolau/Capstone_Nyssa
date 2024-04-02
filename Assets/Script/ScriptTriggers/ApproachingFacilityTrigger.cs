using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachingFacilityTrigger : MonoBehaviour
{
    public DialogueTrigger sproutApproachTrigger;
    public DialogueTrigger celesteApproachTrigger;

    private void OnTriggerEnter(Collider other)
    {
        // Check if earthPlayer enterted area
        if (other.CompareTag("Player1") && other is CapsuleCollider)
        {
            sproutApproachTrigger.TriggerDialogue();
            // Destroy the GameObject collider
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player2") && other is CapsuleCollider)
        {
            celesteApproachTrigger.TriggerDialogue();
            // Destroy the GameObject collider
            Destroy(gameObject);
        }
    }
}
