using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTwoIntroDialogueTrigger : MonoBehaviour
{
    public DialogueTrigger introDialogue;

    private void OnTriggerEnter(Collider other)
    {
        // Check if earthPlayer enterted area
        if ((other.CompareTag("Player1") && other is CapsuleCollider) || (other.CompareTag("Player2") && other is CapsuleCollider))
        {
            introDialogue.GetDialogueManager().enabled = true;
            //DialogueManager.Instance.enabled = true;
            introDialogue.TriggerDialogue();
            // Destroy the GameObject collider
            Destroy(gameObject);
        }
    }
}
