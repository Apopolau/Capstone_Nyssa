using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Stats", menuName = "Data Type/Stats/Item Stats")]
public class ItemStats : ScriptableObject
{
    [SerializeField] private string currentName;
    [SerializeField] private string itemNameEN;
    [SerializeField] private string itemNameFR;
    [SerializeField] private Sprite icon;
    //public int Quantity;
    //public GameObject itemPrefab;

    public string GetCurrentName()
    {
        return currentName;
    }

    public void SetCurrentName(string newName)
    {
        currentName = newName;
    }

    public string GetItemNameEN()
    {
        return itemNameEN;
    }

    public void SetItemNameEN(string newNameEN)
    {
        itemNameEN = newNameEN;
    }

    public string GetItemNameFR()
    {
        return itemNameFR;
    }

    public void SetItemNameFR(string newNameFR)
    {
        itemNameFR = newNameFR;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public void SetIcon(Sprite newIcon)
    {
        icon = newIcon;
    }
}
