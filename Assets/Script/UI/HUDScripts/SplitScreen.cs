using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] public GameObject earthPlayer;
    [SerializeField] public GameObject celestialPlayer;
    [SerializeField] public HUDManager hudManager;
    [SerializeField] public Camera earthCam;
    [SerializeField] public Camera celestialCam;
    [SerializeField] public Camera mainCam;
    [SerializeField] public float distance;
    [SerializeField] public VirtualMouseInput virtualMouseInput;
    [SerializeField] public VirtualMouseInput earthVirtualMouseInput;
    

    public DialogueManager dialogue;
    public bool inCutscene = false;

    public bool switching = false;
    public int currCam = 1;
    public int Manager = 0;

    private void Start()
    {
        SetOneCam();
        virtualMouseInput = hudManager.GetVirtualMouseInput();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        //If two players are close enough make camera one, if players are far enough split camera
        if (!inCutscene)
        {
            if (Vector3.Distance(earthPlayer.transform.position, celestialPlayer.transform.position) > distance)
            {
                SetTwoCams();
                // Changed();
            }
            else if (Vector3.Distance(earthPlayer.transform.position, celestialPlayer.transform.position) < distance)
            {
                SetOneCam();
                //Changed();
            }
        }
        */
    }

    public void EnterCutscene()
    {
        inCutscene = true;
    }

    public void ExitCutscene()
    {
        inCutscene = false;
    }

    private void Changed()
    {
        GetComponent<Animator>().SetTrigger("Change");
    }

    /*
    public void ManageCamera()
    {
        if (Manager == 0)
        {
            SetTwoCams();
            Manager = 1;
        }
        else
        {
            SetOneCam();
            Manager = 0;
        }
    }
    */

    /*
    private void SetTwoCams()
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
        earthVirtualMouseInput.cursorTransform.position = earthPlayer.GetComponent<EarthPlayer>().virtualMouseInput.cursorTransform.position;
        if (earthPlayer.GetComponent<EarthPlayer>().isPlantSelected || earthPlayer.GetComponent<EarthPlayer>().isRemovalStarted)
        {
            
            earthPlayer.GetComponent<EarthPlayer>().TurnOffCursor();
            earthPlayer.GetComponent<EarthPlayer>().virtualMouseInput = earthVirtualMouseInput;
            earthPlayer.GetComponent<EarthPlayer>().SwitchCursorIcon(virtualMouseInput.cursorGraphic.GetComponent<Image>().sprite);
            earthPlayer.GetComponent<EarthPlayer>().TurnOnCursor();
        }
        if (currCam == 1)
        {
            // Changed();
            currCam = 2;
        }
    }
    */

    private void SetOneCam()
    {
        if (currCam == 2)
        {
            currCam = 1;
            // Changed();

        }

        //mainCam.transform.position = Vector3.Lerp(earthPlayer.transform.position, celestialPlayer.transform.position, 0.5f);

        mainCam.gameObject.SetActive(true);
        mainCam.enabled = true;
        celestialCam.gameObject.SetActive(false);
        celestialCam.enabled = false;
        earthCam.gameObject.SetActive(false);
        earthCam.enabled = false;
        //go into the the plant systems main camera and make sure it is properly set
        earthPlayer.GetComponent<EarthPlayer>().SetCamera(mainCam);
        //virtualMouseInput.cursorTransform.position = earthPlayer.GetComponent<EarthPlayer>().virtualMouseInput.cursorTransform.position;

        if (virtualMouseInput != null)
        {
            
            //virtualMouseInput.cursorTransform.position = hudManager.GetVirtualMousePosition();

            if (earthPlayer.GetComponent<EarthPlayer>().isPlantSelected || earthPlayer.GetComponent<EarthPlayer>().isRemovalStarted)
            {
                /*
                hudManager.ToggleVirtualMouseSprite(false);
                hudManager.SwitchVirtualMouseInputs(virtualMouseInput);
                hudManager.SetVirtualMouseImage(virtualMouseInput.cursorGraphic.GetComponent<Image>().sprite);
                //earthPlayer.GetComponent<EarthPlayer>().TurnOffCursor();
                //earthPlayer.GetComponent<EarthPlayer>().virtualMouseInput = virtualMouseInput;
                //earthPlayer.GetComponent<EarthPlayer>().SwitchCursorIcon();
                //earthPlayer.GetComponent<EarthPlayer>().TurnOnCursor();
                hudManager.ToggleVirtualMouseSprite(true);
                */
                hudManager.SwitchCursorCanvas(HUDManager.MouseCanvasType.MAIN, true);
            }
            else
            {
                hudManager.SwitchCursorCanvas(HUDManager.MouseCanvasType.MAIN, false);
            }
            
        }
        else{
            virtualMouseInput = hudManager.GetVirtualMouseInput();
        }
        
    }


    public void camDistExtend()
    {
        distance = 1300;
    }
    public void camDistDec()
    {
        distance = 1300;
    }

    public HUDManager GetHudManager()
    {
        return hudManager;
    }
}
