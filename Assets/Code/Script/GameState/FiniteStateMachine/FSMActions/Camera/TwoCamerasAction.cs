using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

[CreateAssetMenu(menuName = "Architecture/FSM/Actions/SplitScreen/TwoCamerasAction")]
public class TwoCamerasAction : FSMAction
{
    SplitScreen splitScreen;

    public override void EnterState(BaseStateMachine stateMachine)
    {

        splitScreen = stateMachine.GetComponent<SplitScreen>();
        splitScreen.celestialCam.gameObject.SetActive(true);
        splitScreen.celestialCam.enabled = true;
        splitScreen.earthCam.gameObject.SetActive(true);
        splitScreen.earthCam.enabled = true;
        
        //go into the the plant systems main camera and make sure it is properly set
        splitScreen.earthPlayer.GetComponent<EarthPlayer>().SetCamera(splitScreen.earthCam);
        splitScreen.hudManager.SetCamera(splitScreen.earthCam);

        if (splitScreen.earthPlayer.GetComponent<EarthPlayer>().GetIsPlantSelected() || splitScreen.earthPlayer.GetComponent<EarthPlayer>().GetIsRemovalStarted())
        {
            splitScreen.hudManager.SwitchCursorCanvas(HUDManager.MouseCanvasType.SPROUT, true);
        }
        else
        {
            splitScreen.hudManager.SwitchCursorCanvas(HUDManager.MouseCanvasType.SPROUT, false) ;
        }
        splitScreen.mainCam.gameObject.SetActive(false);
        splitScreen.mainCam.enabled = false;
        splitScreen.currCam = 2;
    }

    public override void Execute(BaseStateMachine stateMachine)
    {



    }

    public override void ExitState(BaseStateMachine stateMachine)
    {

    }
}
