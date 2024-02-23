using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSlideInteraction : Interactable
{

    EarthPlayer earthPlayer;
    WaitForSeconds slideClearTime = new WaitForSeconds(4.542f);
    bool isAnimated = false;

    // Start is called before the first frame update
    void Start()
    {
        players = playerRuntimeSet.Items;
        foreach (GameObject p in players)
        {
            if (p.GetComponent<EarthPlayer>())
            {
                earthPlayer = p.GetComponent<EarthPlayer>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        OnEarthPlayerInteracts();
        MoveDownward();
    }

    /*
    private void OnTriggerStay(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
    */

    private void OnEarthPlayerInteracts()
    {
        if(earthPlayer.interacting && isInRange && !isAnimated)
        {
            Debug.Log("starting animation");
            isAnimated = true;
            StartCoroutine(LetTimerRun());
        }
    }

    private void MoveDownward()
    {
        if (isAnimated)
        {
            Debug.Log("moving downward");
            this.gameObject.transform.localPosition.Set(this.gameObject.transform.localPosition.x, this.gameObject.transform.localPosition.y - 0.5f, this.gameObject.transform.localPosition.z);
        }
    }

    private IEnumerator LetTimerRun()
    {
        
        yield return slideClearTime;
        isAnimated = false;
    }
}
