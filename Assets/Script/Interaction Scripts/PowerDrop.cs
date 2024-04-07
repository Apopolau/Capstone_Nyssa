using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerDrop : Interactable
{
    public CelestialPlayer.Power powerDrop;

    //public CelestialPlayer.coldSnapFill coldSnapFill;
    //[SerializeField] public Image coldSnapFill;
    public PowerBehaviour powerBehaviour;
    [SerializeField] private LevelProgress levelProgress;
    /// <summary>
    /// start a dialogue
    /// Tell Celest ow to use it
    /// enable the particular power up 
    /// </summary>
    // Start is called before the first frame update
    private void Awake()
    {
        foreach (GameObject player in playerSet.Items)
        {
            if (player.GetComponent<EarthPlayer>())
            {
                earthPlayer = player.GetComponent<EarthPlayer>();
            }
            else if (player.GetComponent<CelestialPlayer>())
            {
                celestialPlayer = player.GetComponent<CelestialPlayer>();
            }
        }
    }

    private void Start()
    {
        powerBehaviour = GetComponent<PowerBehaviour>();

    }

    // Update is called once per frame
    void Update()
    {
        PowerPickup();

    }

    /// <summary>
    /// APOLINE use this function for handling what happens when the player tries to pick up the power
    /// </summary>
    public void PowerPickup()
    {

        if ((p1IsInRange && earthPlayer.interacting) || (p2IsInRange && celestialPlayer.interacting))
        {
            Debug.Log("Item is OCCURRING");
            PowerStats currPowerStats = getPowerDropStatRef(powerDrop);
            powerBehaviour.setEnabled(currPowerStats);


            //////////////////////////////////Will need to create a new UI for this

            // Destroy(this.GetComponentInParent<Transform>().gameObject);
            Destroy(this.gameObject);

        }
    }

    private PowerStats getPowerDropStatRef(CelestialPlayer.Power powerDrop)
    {
        if (powerDrop == CelestialPlayer.Power.COLDSNAP)
        {
            Debug.Log("return cold snap stats");
            levelProgress.SetPowers(true);
            celestialPlayer.coldSnapFill.enabled = false;
            celestialPlayer.CTRLColdSnapFill.enabled = false;
            StartCoroutine(celestialPlayer.CoolDownImageFill(celestialPlayer.coldSnapFill));
            celestialPlayer.coldSnapFill.enabled = true;
            celestialPlayer.CTRLColdSnapFill.enabled = true;
            return powerBehaviour.ColdSnapStats;
        }
        if (powerDrop == CelestialPlayer.Power.MOONTIDE)
        {
            Debug.Log("return cold snap stats");
            levelProgress.SetPowers(true);
            celestialPlayer.moonTideFill.enabled = false;
            celestialPlayer.CTRLMoonTideFill.enabled = false;
            StartCoroutine(celestialPlayer.CoolDownImageFill(celestialPlayer.moonTideFill));
            celestialPlayer.moonTideFill.enabled = true;
            celestialPlayer.moonTideFill.enabled = true;
            celestialPlayer.CTRLMoonTideFill.enabled = true;
            return powerBehaviour.MoonTideAttackStats;
        }
        return null;
    }

    //You may want a UI function here to turn up the button corresponding to this power, here

    //Then call it in PowerPickup

    //Make sure there's a collider on it for this
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<EarthPlayer>() && other.GetType() == typeof(CapsuleCollider))
        {
            p1IsInRange = true;
        }
        if (other.GetComponent<CelestialPlayer>() && other.GetType() == typeof(CapsuleCollider))
        {
            p2IsInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<EarthPlayer>() && other.GetType() == typeof(CapsuleCollider))
        {
            p1IsInRange = false;
        }
        if (other.GetComponent<CelestialPlayer>() && other.GetType() == typeof(CapsuleCollider))
        {
            p2IsInRange = false;
        }
    }
}
