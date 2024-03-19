using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The point of this script is that you can throw it on to an object,
/// and at runtime it will automatically add itself to the list you drag into gameObjectRunTimeSet in the inspector
/// This is a suitable replacement for a Singleton manager object and you won't have to implement it in new scenes
/// </summary>
public class AddGameObjectToRuntimeSet : MonoBehaviour
{
    public GameObjectRuntimeSet gameObjectRuntimeSet;

    private void OnEnable()
    {
        gameObjectRuntimeSet.AddToList(this.gameObject);
    }

    private void OnDisable()
    {
        gameObjectRuntimeSet.RemoveFromList(this.gameObject);
    }
}
