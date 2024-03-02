using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandslideDialouge : MonoBehaviour
{
    public DialogueTrigger landslideDialouge1;
    public DialogueTrigger landslideDialouge2;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if earthPlayer enterted area
        if (other.CompareTag("Player1"))
        {   
            landslideDialouge1.TriggerDialogue();
            // Destroy the GameObject collider
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player2") && other is CapsuleCollider)
        {
           landslideDialouge2.TriggerDialogue();
            // Destroy the GameObject collider
            Destroy(gameObject);
        }
    }
}


