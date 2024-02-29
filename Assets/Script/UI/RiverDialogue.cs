using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverDialogue : MonoBehaviour
{
    public DialogueTrigger riverDialougeEarth;
    public DialogueTrigger riverlideDialougeCelest;
    // Start is called before the first frame update
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
            riverDialougeEarth.TriggerDialogue();
            // Destroy the GameObject collider
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player2") && other is CapsuleCollider)
        {
           riverlideDialougeCelest.TriggerDialogue();
            // Destroy the GameObject collider
            Destroy(gameObject);
        }
    }
}
