using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundNyssaTrigger : MonoBehaviour
{
    public DialogueTrigger nyssaDialogue;

    private void OnTriggerEnter(Collider other)
    {
        // Check if earthPlayer enterted area
        if ((other.CompareTag("Player1") && other is CapsuleCollider) || (other.CompareTag("Player2") && other is CapsuleCollider))
        {
            nyssaDialogue.TriggerDialogue();

            //In here we want to have some kind of level-wide event that causes the next spawn point to turn on
            
            Destroy(gameObject);
        }
    }
}
