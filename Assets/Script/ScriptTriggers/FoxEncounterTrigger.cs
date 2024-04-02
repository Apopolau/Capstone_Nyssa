using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxEncounterTrigger : MonoBehaviour
{
    [SerializeField] GameObject fox;
    [SerializeField] GameObject foxTrigger2;
    public DialogueTrigger foxEncounter1;

    private void OnTriggerEnter(Collider other)
    {
        // Check if earthPlayer enterted area
        if ((other.CompareTag("Player1") && other is CapsuleCollider) || (other.CompareTag("Player2") && other is CapsuleCollider))
        {
            foxEncounter1.TriggerDialogue();
            foxTrigger2.SetActive(true);
            fox.GetComponent<Fox>().GoToTriggerSpot();
            // Destroy the GameObject collider
            Destroy(gameObject);
        }
    }
}
