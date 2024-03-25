using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

[CreateAssetMenu(menuName = "FSM/Actions/SplitScreen/TwoCamerasAction")]
public class TwoCamerasAction : FSMAction
{
    SplitScreen splitScreen;

    public override void EnterState(BaseStateMachine stateMachine)
    {
        splitScreen = stateMachine.GetComponent<SplitScreen>();
        splitScreen.mainCam.gameObject.SetActive(false);
        splitScreen.mainCam.enabled = false;
        splitScreen.celestialCam.gameObject.SetActive(true);
        splitScreen.celestialCam.enabled = true;
        splitScreen.earthCam.gameObject.SetActive(true);
        splitScreen.earthCam.enabled = true;

        //go into the the plant systems main camera and make sure it is properly set
        splitScreen.earthPlayer.GetComponent<EarthPlayer>().SetCamera(splitScreen.earthCam);
        splitScreen.earthVirtualMouseInput.cursorTransform.position = splitScreen.earthPlayer.GetComponent<EarthPlayer>().virtualMouseInput.cursorTransform.position;
        if (splitScreen.earthPlayer.GetComponent<EarthPlayer>().isPlantSelected || splitScreen.earthPlayer.GetComponent<EarthPlayer>().isRemovalStarted)
        {

            splitScreen.earthPlayer.GetComponent<EarthPlayer>().TurnOffCursor();
            splitScreen.earthPlayer.GetComponent<EarthPlayer>().virtualMouseInput = splitScreen.earthVirtualMouseInput;
            splitScreen.earthPlayer.GetComponent<EarthPlayer>().SwitchCursorIcon(splitScreen.virtualMouseInput.cursorGraphic.GetComponent<Image>().sprite);
            if(splitScreen.earthPlayer.GetComponent<EarthPlayer>().GetInPlantSelection() || splitScreen.earthPlayer.GetComponent<EarthPlayer>().GetInRemovalSelection())
            {
                splitScreen.earthPlayer.GetComponent<EarthPlayer>().TurnOnCursor();
            }
        }
        splitScreen.currCam = 2;
    }

    public override void Execute(BaseStateMachine stateMachine)
    {



    }

    public override void ExitState(BaseStateMachine stateMachine)
    {

    }
}
