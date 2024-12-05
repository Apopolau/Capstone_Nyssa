using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxFollowupTrigger : MonoBehaviour
{
    [SerializeField] private GameObject fox;
    public DialogueTrigger foxEncounter2;

    private void OnTriggerEnter(Collider other)
    {
        // Check if earthPlayer enterted area
        if ((other.CompareTag("Player1") && other is CapsuleCollider) || (other.CompareTag("Player2") && other is CapsuleCollider))
        {
            foxEncounter2.TriggerDialogue();
            Fox foxScript = fox.GetComponent<Fox>();
            foxScript.SetStuck(false);
            foxScript.SetIsWaiting(false);
            foxScript.SetOpeningDialogueDone(true);
            // Destroy the GameObject collider
            Destroy(gameObject);
        }
    }
}
