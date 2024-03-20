using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

[CreateAssetMenu(menuName = "FSM/Actions/SplitScreen/OneCameraAction")]
public class OneCameraAction : FSMAction
{
    SplitScreen splitScreen;

    public override void Execute(BaseStateMachine stateMachine)
    {
        splitScreen = stateMachine.GetComponent<SplitScreen>();

        
        if (splitScreen.currCam == 2)
        {
            splitScreen.mainCam.gameObject.SetActive(true);
            splitScreen.mainCam.enabled = true;
            splitScreen.celestialCam.gameObject.SetActive(false);
            splitScreen.celestialCam.enabled = false;
            splitScreen.earthCam.gameObject.SetActive(false);
            splitScreen.earthCam.enabled = false;
            //go into the the plant systems main camera and make sure it is properly set
            splitScreen.earthPlayer.GetComponent<EarthPlayer>().SetCamera(splitScreen.mainCam);
            splitScreen.virtualMouseInput.cursorTransform.position = splitScreen.earthPlayer.GetComponent<EarthPlayer>().virtualMouseInput.cursorTransform.position;

            if (splitScreen.earthPlayer.GetComponent<EarthPlayer>().isPlantSelected || splitScreen.earthPlayer.GetComponent<EarthPlayer>().isRemovalStarted)
            {
                splitScreen.earthPlayer.GetComponent<EarthPlayer>().TurnOffCursor();
                splitScreen.earthPlayer.GetComponent<EarthPlayer>().virtualMouseInput = splitScreen.virtualMouseInput;
                splitScreen.earthPlayer.GetComponent<EarthPlayer>().SwitchCursorIcon(splitScreen.virtualMouseInput.cursorGraphic.GetComponent<Image>().sprite);
                splitScreen.earthPlayer.GetComponent<EarthPlayer>().TurnOnCursor();
            }
            splitScreen.currCam = 1;
        }

        if (!splitScreen.inCutscene)
        {
            splitScreen.mainCam.transform.position = Vector3.Lerp(splitScreen.earthPlayer.transform.position, splitScreen.celestialPlayer.transform.position, 0.5f);
        }
    }
}
