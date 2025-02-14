using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

[CreateAssetMenu(menuName = "Architecture/FSM/Actions/SplitScreen/OneCameraAction")]
public class OneCameraAction : FSMAction
{
    SplitScreen splitScreen;

    public override void EnterState(BaseStateMachine stateMachine)
    {
        splitScreen = stateMachine.GetComponent<SplitScreen>();

        splitScreen.mainCam.gameObject.SetActive(true);
        splitScreen.mainCam.enabled = true;

        splitScreen.earthPlayer.GetComponent<EarthPlayer>().SetCamera(splitScreen.mainCam);
        splitScreen.hudManager.SetCamera(splitScreen.mainCam);

        //splitScreen.virtualMouseInput.cursorTransform.position = splitScreen.earthPlayer.GetComponent<EarthPlayer>().virtualMouseInput.cursorTransform.position;
        //splitScreen.GetHudManager().SetVirtualMousePosition(splitScreen.GetHudManager().GetVirtualMousePosition());

        if (splitScreen.earthPlayer.GetComponent<EarthPlayer>().GetIsPlantSelected() || splitScreen.earthPlayer.GetComponent<EarthPlayer>().GetIsRemovalStarted())
        {
            /*
            splitScreen.GetHudManager().ToggleVirtualMouseSprite(true);
            splitScreen.GetHudManager().SwitchVirtualMouseInputs(splitScreen.virtualMouseInput);
            splitScreen.GetHudManager().SetVirtualMouseImage(splitScreen.virtualMouseInput.cursorGraphic.GetComponent<Image>().sprite);
            //splitScreen.earthPlayer.GetComponent<EarthPlayer>().SwitchCursorIcon(splitScreen.virtualMouseInput.cursorGraphic.GetComponent<Image>().sprite);
            if (splitScreen.earthPlayer.GetComponent<EarthPlayer>().GetInPlantSelection() || splitScreen.earthPlayer.GetComponent<EarthPlayer>().GetInRemovalSelection())
            {
                splitScreen.earthPlayer.GetComponent<EarthPlayer>().TurnOnCursor();
            }
            */
            splitScreen.hudManager.SwitchCursorCanvas(HUDManager.MouseCanvasType.MAIN, true);
        }
        else
        {
            splitScreen.hudManager.SwitchCursorCanvas(HUDManager.MouseCanvasType.MAIN, false);
        }

        
        splitScreen.celestialCam.gameObject.SetActive(false);
        splitScreen.celestialCam.enabled = false;
        splitScreen.earthCam.gameObject.SetActive(false);
        splitScreen.earthCam.enabled = false;
        //go into the the plant systems main camera and make sure it is properly set
        

        splitScreen.currCam = 1;
    }

    public override void Execute(BaseStateMachine stateMachine)
    {
        if (!splitScreen.inCutscene)
        {
            Vector3 earthPos = new Vector3(splitScreen.earthPlayer.transform.position.x,
                splitScreen.earthPlayer.transform.position.y, splitScreen.earthPlayer.transform.position.z);
            Vector3 celestialPos = new Vector3(splitScreen.celestialPlayer.transform.position.x,
                splitScreen.celestialPlayer.transform.position.y, splitScreen.celestialPlayer.transform.position.z);
            splitScreen.gameObject.transform.position = Vector3.Lerp(earthPos, celestialPos, 0.3f);
        }
    }

    public override void ExitState(BaseStateMachine stateMachine)
    {

    }
}
