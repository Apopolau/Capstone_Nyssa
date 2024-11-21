using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSlideInteraction : Interactable
{
    WaitForSeconds slideClearTime = new WaitForSeconds(4.542f);
    bool isAnimated = false;
    [SerializeField] InstanceSoundEvent soundEvent;
    

    private void Awake()
    {
        isEarthInteractable = true;
    }

    private void Start()
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
        OnEarthPlayerInteracts();
        MoveDownward();
        UpdateUIElement();
    }

    

    private void OnEarthPlayerInteracts()
    {
        if(earthPlayer.GetIsInteracting() && p1IsInRange && !isAnimated)
        {
            isAnimated = true;
            soundEvent.Play();
            isEarthInteractable = false;
            StartCoroutine(LetTimerRun());
        }
    }

    private void MoveDownward()
    {
        if (isAnimated)
        {
            this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x, this.gameObject.transform.localPosition.y - 0.3f, this.gameObject.transform.localPosition.z);
        }
    }

    private IEnumerator LetTimerRun()
    {
        
        yield return slideClearTime;
        isAnimated = false;
    }

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
