using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/Controls/HoldingNyssa")]
public class CtrlsHoldingNyssaAction : FSMAction
{
    EarthPlayer earthPlayer;

    public override void EnterState(BaseStateMachine stateMachine)
    {
        earthPlayer = stateMachine.GetComponent<EarthPlayer>();

        earthPlayer.earthControls.controls.DialogueControls.Disable();
        earthPlayer.earthControls.controls.EarthPlayerDefault.Disable();
        earthPlayer.earthControls.controls.PlantIsSelected.Disable();
        earthPlayer.earthControls.controls.RemovingPlant.Disable();
        earthPlayer.earthControls.controls.HealSelect.Disable();
        earthPlayer.earthControls.controls.BarrierSelect.Disable();
        earthPlayer.earthControls.controls.HoldingNyssa.Enable();

        earthPlayer.DarkenAllImages(earthPlayer.GetPlantDarkenObject());
    }

    public override void Execute(BaseStateMachine stateMachine)
    {

    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        earthPlayer.SetHoldingNyssa(false);
        earthPlayer.uiController.RestoreUI(earthPlayer.GetPlantDarkenObject());
        earthPlayer.earthControls.controls.HoldingNyssa.Disable();
    }
}
