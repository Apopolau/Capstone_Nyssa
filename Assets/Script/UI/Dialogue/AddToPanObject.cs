using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToPanObject : MonoBehaviour
{
    [SerializeField] private DialogueCameraPan returnToOrigin;

    private void Start()
    {
        returnToOrigin.SetPanToThis(this.gameObject);
    }
}
