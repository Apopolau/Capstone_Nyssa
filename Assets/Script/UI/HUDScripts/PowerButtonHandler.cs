using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerButtonHandler : MonoBehaviour
{
    private GameObject overlay;
    //[SerializeField] Image iconImage;
    [SerializeField] private string hashName;
    [SerializeField] private string itemName;
    [SerializeField] private HUDModel model;
    bool wasDisabled;

    private void Awake()
    {
        overlay = transform.GetChild(0).gameObject;
        //overlay.GetComponent<Image>().material.renderQueue = iconImage.material.renderQueue + 1;
        model.AddOverlayComponents(this.gameObject);
    }

    //Turn on overlay to darken an ability, or off to brighten it
    public void ToggleOverlay(bool on)
    {
        wasDisabled = on;
        overlay.SetActive(on);
    }

    /*
    public void ToggleOverlay(float timer)
    {
        overlay.SetActive(true);
        
        StartCoroutine(StartCooldown(timer));
    }

    private IEnumerator StartCooldown(float timer)
    {
        yield return new WaitForSeconds(timer);
        if (wasDisabled)
        {
            overlay.SetActive(false);
        }
    }
    */

    public string GetHashName()
    {
        return hashName;
    }

    public string GetItemName()
    {
        return itemName;
    }
}
