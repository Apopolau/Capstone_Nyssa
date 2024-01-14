using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitScreen : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]GameObject earthPlayer;
    [SerializeField] GameObject celestialPlayer;
    [SerializeField] Camera earthCam;
    [SerializeField] Camera celestialCam;
    [SerializeField] Camera mainCam;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { if (Vector3.Distance(earthPlayer.transform.position,celestialPlayer.transform.position) > 20f)
        {
            mainCam.gameObject.SetActive(false);
            mainCam.enabled=false;
            celestialCam.gameObject.SetActive(true);
            celestialCam.enabled = true;
            earthCam.gameObject.SetActive(true);
            earthCam.enabled = true;
        }
        else if(Vector3.Distance(earthPlayer.transform.position, celestialPlayer.transform.position) < 20f)
        {
            mainCam.gameObject.SetActive(true);
            mainCam.enabled = true;
            celestialCam.gameObject.SetActive(false);
            celestialCam.enabled = false;
            earthCam.gameObject.SetActive(false);
           earthCam.enabled = false;

        }

        
    }
}
