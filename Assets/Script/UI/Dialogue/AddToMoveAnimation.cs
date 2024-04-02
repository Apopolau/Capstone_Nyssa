using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToMoveAnimation : MonoBehaviour
{
    [SerializeField] private List<DialogueMoveEvent> moveToSet;

    private void OnEnable()
    {
        foreach (DialogueMoveEvent move in moveToSet)
        {
            move.SetMoveToThis(this.gameObject);
        }
    }
}
