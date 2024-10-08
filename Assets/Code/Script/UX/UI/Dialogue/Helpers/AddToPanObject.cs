using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToPanObject : MonoBehaviour
{
    [SerializeField] private List<DialogueCameraPan> objectToPanTo;

    private void OnEnable()
    {
        foreach (DialogueCameraPan pan in objectToPanTo)
        {
            pan.SetPanToThis(this.gameObject);
        }
    }
}
