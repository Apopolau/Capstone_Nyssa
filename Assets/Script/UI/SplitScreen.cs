using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
//https://youtu.be/_yR9FL4LXGE?si=PVnjn0KDhAxmIdZT
//switch cam animation
public class SplitScreen : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject earthPlayer;
    [SerializeField] GameObject celestialPlayer;
    [SerializeField] public Camera earthCam;
    [SerializeField] public Camera celestialCam;
    [SerializeField] public Camera mainCam;
    [SerializeField] float distance;
    [SerializeField] public VirtualMouseInput virtualMouseInput;
    [SerializeField] public VirtualMouseInput earthVirtualMouseInput;
    bool switching = false;
    int currCam = 1;
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

       

        if (Vector3.Distance(earthPlayer.transform.position, celestialPlayer.transform.position) > distance)
        {
            SetTwoCams();
        }
        else if (Vector3.Distance(earthPlayer.transform.position, celestialPlayer.transform.position) < distance)
        {
            SetOneCam();
        }


    }
    private void Changed()
    {
        GetComponent<Animator>().SetTrigger("Change");

    }
    private void SetTwoCams ()
    {
     
            //GetComponent<Animator>().SetTrigger("Change");
            mainCam.gameObject.SetActive(false);
            mainCam.enabled = false;
            celestialCam.gameObject.SetActive(true);
            celestialCam.enabled = true;
            earthCam.gameObject.SetActive(true);
            earthCam.enabled = true;

            //go into the the plant systems main camera and make sure it is properly set
            earthPlayer.GetComponent<EarthPlayer>().SetCamera(earthCam);
        earthPlayer.GetComponent<EarthPlayer>().virtualMouseInput = earthVirtualMouseInput;
        if (currCam == 1)
        {
            Changed();
            currCam = 2;
        }


    }
    private void SetOneCam()
    {

        mainCam.transform.position = Vector3.Lerp(earthPlayer.transform.position, celestialPlayer.transform.position, 0.5f);

        mainCam.gameObject.SetActive(true);
        mainCam.enabled = true;
        celestialCam.gameObject.SetActive(false);
        celestialCam.enabled = false;
        earthCam.gameObject.SetActive(false);
        earthCam.enabled = false;
        //go into the the plant systems main camera and make sure it is properly set
        earthPlayer.GetComponent<EarthPlayer>().SetCamera(mainCam);
        earthPlayer.GetComponent<EarthPlayer>().virtualMouseInput = virtualMouseInput;

        if (currCam == 2)
        {
            Changed();
            currCam = 1;
        }
    }

}
