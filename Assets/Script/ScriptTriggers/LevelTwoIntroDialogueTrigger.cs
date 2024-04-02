using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTwoIntroDialogueTrigger : MonoBehaviour
{
    [SerializeField] GameObject dialogueManager;
    public DialogueTrigger introDialogue;

    private void OnTriggerEnter(Collider other)
    {
        // Check if earthPlayer enterted area
        if ((other.CompareTag("Player1") && other is CapsuleCollider) || (other.CompareTag("Player2") && other is CapsuleCollider))
        {
            dialogueManager.SetActive(true);
            introDialogue.TriggerDialogue();
            // Destroy the GameObject collider
            Destroy(gameObject);
        }
    }
}
