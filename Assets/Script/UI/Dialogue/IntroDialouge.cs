using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroDialouge : MonoBehaviour
{
    
    public DialogueTrigger introDialouge;
    private SphereCollider dialogueCollider;

    private void Awake()
    {
        dialogueCollider = GetComponent<SphereCollider>();
    }
    void Start()
    {
        dialogueCollider.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if earthChar entered collider which is the initial position
        if (other.CompareTag("Player1"))
        {
            
            introDialouge.TriggerDialogue();
             // Destroy the GameObject collider
            Destroy(gameObject);
        }

       
    }
}
