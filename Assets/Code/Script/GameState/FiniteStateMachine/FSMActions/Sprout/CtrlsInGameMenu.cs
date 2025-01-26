using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Actions/Controls/InGameMenu")]
public class CtrlsInGameMenu : FSMAction
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

        //earthPlayer.earthControls.controls.MenuControls.Enable();
        earthPlayer.earthControls.controls.UI.Enable();
    }

    public override void Execute(BaseStateMachine stateMachine)
    {

    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        //earthPlayer.earthControls.controls.MenuControls.Disable();
        earthPlayer.earthControls.controls.UI.Disable();
    }
}
