using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/Controls/SelectionBarrier")]
public class CtrlsSelectionBarrierAction : FSMAction
{
    EarthPlayer earthPlayer;

    public override void EnterState(BaseStateMachine stateMachine)
    {
        earthPlayer = stateMachine.GetComponent<EarthPlayer>();

        //Set all of the appropriate controls
        earthPlayer.earthControls.controls.BarrierSelect.Enable();

        earthPlayer.earthControls.controls.DialogueControls.Disable();
        earthPlayer.earthControls.controls.EarthPlayerDefault.Disable();
        earthPlayer.earthControls.controls.PlantIsSelected.Disable();
        earthPlayer.earthControls.controls.RemovingPlant.Disable();
        earthPlayer.earthControls.controls.HealSelect.Disable();

        //Get targets for shielding
        earthPlayer.PickClosestTarget();

        //Instantiate appropriate objects
        earthPlayer.tileOutline = Instantiate(earthPlayer.GetTileOutlinePrefab(), earthPlayer.GetPowerTarget().transform);
        earthPlayer.tileOutline.GetComponentInChildren<SpriteRenderer>().color = Color.green;

        //Update UI
        earthPlayer.displayText.text = "Select a target to shield";
        earthPlayer.uiController.DarkenOverlay(earthPlayer.GetPlantDarkenObject());
    }

    public override void Execute(BaseStateMachine stateMachine)
    {

    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        //Turn these controls off
        earthPlayer.earthControls.controls.BarrierSelect.Disable();

        //Update UI
        earthPlayer.displayText.text = "";
        Destroy(earthPlayer.tileOutline);
    }
}
