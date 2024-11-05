using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnergyNode : MonoBehaviour
{
    TextMeshProUGUI energyCounterText;
    bool fadingOut = false;


    private void Awake()
    {
        energyCounterText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        //GetComponent<CanvasRenderer>().SetAlpha(0.6f);
    }

    private void FixedUpdate()
    {
        Move();
        Fade();
        Die();
    }

    public void SetEnergyAmount(int energy)
    {
        energyCounterText.text = "+" + energy.ToString();
    }

    public void TurnCounterOff()
    {
        energyCounterText.enabled = false;
    }

    private void Move()
    {
        Vector3 newLocation = new Vector3(GetComponent<RectTransform>().localPosition.x, GetComponent<RectTransform>().localPosition.y + 2, GetComponent<RectTransform>().localPosition.z);
        this.GetComponent<RectTransform>().localPosition = newLocation;
        //this.gameObject.GetComponent<RectTransform>().localPosition.Set(this.GetComponent<RectTransform>().localPosition.x, this.GetComponent<RectTransform>().localPosition.y + 1, this.GetComponent<RectTransform>().localPosition.z);
    }

    private void Fade()
    {
        if (!fadingOut)
        {
            //GetComponent<Image>().CrossFadeAlpha(1, 0.1f, false);
            //energyCounterText.CrossFadeAlpha(1, 0.1f, false);
            Color newColor = GetComponent<Image>().color;
            newColor.a = newColor.a + 0.05f;
            GetComponent<Image>().color = newColor;
            energyCounterText.color = newColor;

            //Debug.Log("Alpha is " + GetComponent<CanvasRenderer>().GetAlpha());

            //if (GetComponent<CanvasRenderer>().GetAlpha() >= 1)
            if (GetComponent<Image>().color.a >= 1)
            {
                fadingOut = true;
            }
        }
        else
        {
            //GetComponent<Image>().CrossFadeAlpha(0, 0.5f, false);
            //energyCounterText.CrossFadeAlpha(0, 0.5f, false);
            //Debug.Log("Alpha is " + GetComponent<CanvasRenderer>().GetAlpha());
            Color newColor = GetComponent<Image>().color;
            newColor.a = newColor.a - 0.05f;
            GetComponent<Image>().color = newColor;
            energyCounterText.color = newColor;
        }
    }

    private void Die()
    {
        //if (GetComponent<CanvasRenderer>().GetAlpha() <= 0)
        if (GetComponent<Image>().color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
