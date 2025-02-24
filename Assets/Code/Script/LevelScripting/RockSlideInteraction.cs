using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RockSlideInteraction : Interactable
{
    [SerializeField] GameObject rockslideObject;
    [SerializeField] VisualEffect dustEffect;

    WaitForSeconds slideClearTime = new WaitForSeconds(3.5f);
    bool isAnimated = false;
    [SerializeField] InstanceSoundEvent soundEvent;
    

    private void Awake()
    {
        isEarthInteractable = true;
        isCelestialInteractable = false;
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
            uiObject.SetActive(false);
            dustEffect.gameObject.SetActive(true);
            dustEffect.Play();
            StartCoroutine(LetTimerRun());
        }
    }

    private void MoveDownward()
    {
        if (isAnimated)
        {
            rockslideObject.transform.localPosition = new Vector3(rockslideObject.transform.localPosition.x, rockslideObject.transform.localPosition.y - 0.06f, rockslideObject.transform.localPosition.z);
        }
    }

    private IEnumerator LetTimerRun()
    {
        
        yield return slideClearTime;
        isAnimated = false;
        dustEffect.Stop();
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
