using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeSet<T> : ScriptableObject
{
    public List<T> Items = new List<T>();

    public void Initialize()
    {
        Items.Clear();
    }

    public T GetItemIndex(int index)
    {
        return Items[index];
    }

    public void AddToList(T thingToAdd)
    {
        if (!Items.Contains(thingToAdd))
            Items.Add(thingToAdd);
    }

    public void RemoveFromList(T thingToRemove)
    {
        if (Items.Contains(thingToRemove))
            Items.Remove(thingToRemove);
    }
}
