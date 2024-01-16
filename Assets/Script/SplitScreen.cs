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
    EarthPlayer earthPlayerActiveCam;

    void Start()
    {
         earthPlayerActiveCam = GetComponent<EarthPlayer>();

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

            //go into the the plant systems main camera and make sure it is properly set
            earthPlayerActiveCam.mainCamera = earthCam;
        }
        else if(Vector3.Distance(earthPlayer.transform.position, celestialPlayer.transform.position) < 20f)
        {
            mainCam.gameObject.SetActive(true);
            mainCam.enabled = true;
            celestialCam.gameObject.SetActive(false);
            celestialCam.enabled = false;
            earthCam.gameObject.SetActive(false);
           earthCam.enabled = false;
            //go into the the plant systems main camera and make sure it is properly set
            earthPlayerActiveCam.mainCamera = mainCam;

        }


    }
}
