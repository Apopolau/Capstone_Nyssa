using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
     [SerializeField] Image image;

    public Item _item;   
    public Item Item {
        get { return _item; }
        set { 
            _item = value;
            if (_item == null) {
                image.enabled = false;
            } else {
                image.sprite = _item.Icon;
                image.enabled = true;
            }
        }

    }
 

    private void OnValidate () {

        if (image == null)
            image = GetComponent<Image>();

    }
}
