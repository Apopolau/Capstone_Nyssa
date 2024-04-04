using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddObjectToMoveAnimation : MonoBehaviour
{
    [SerializeField] private List<DialogueMoveEvent> objectToMoveToSet;
    [SerializeField] private List<DialogueMoveEvent> objectToMoveSet;

    private void OnEnable()
    {
        if(objectToMoveToSet != null)
        {
            foreach (DialogueMoveEvent move in objectToMoveToSet)
            {
                move.SetLocationToThis(this.gameObject);
            }
        }
        if(objectToMoveSet != null)
        {
            foreach (DialogueMoveEvent move in objectToMoveSet)
            {
                move.SetThisObjectToMove(this.gameObject);
            }
        }
        
    }
}
