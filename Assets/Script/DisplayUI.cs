using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayUI : MonoBehaviour
{
  public GameObject uiObject;

    // Start is called before the first frame update
    void Start()
    {
        uiObject.SetActive(false);
    }

    private void Update()
{
    if (Input.GetKeyDown(KeyCode.P))
    {
        
        uiObject.SetActive(false);
    
    }
}

    void OnTriggerEnter(Collider player) 
    {
        
     if (player.gameObject.tag == "Player")
        {
            uiObject.SetActive(true);
           // StartCoroutine("WaitForSec");
        }
    }

      void OnTriggerExit(Collider player) 
    {
     if (player.gameObject.tag == "Player")
        {
            uiObject.SetActive(false);
           // StartCoroutine("WaitForSec");
        }
    }
    IEnumerator WaitForSec()
        {
            yield return new WaitForSeconds(5);
            
        }
}
