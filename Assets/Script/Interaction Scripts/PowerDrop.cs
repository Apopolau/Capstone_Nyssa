using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerDrop : Interactable
{


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



    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// APOLINE use this function for handling what happens when the player tries to pick up the power
    /// </summary>
    public void PowerPickup()
    {
        if ((p1IsInRange && earthPlayer.interacting) || (p2IsInRange && celestialPlayer.interacting))
        {
            //Stick your functionality here
            Destroy(this.GetComponentInParent<Transform>().gameObject);
            
        }
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
