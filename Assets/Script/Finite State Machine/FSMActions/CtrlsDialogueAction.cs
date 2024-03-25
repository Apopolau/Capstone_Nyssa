using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/Controls/Dialogue")]
public class CtrlsDialogueAction : FSMAction
{
    EarthPlayer earthPlayer;

    public override void EnterState(BaseStateMachine stateMachine)
    {
        earthPlayer = stateMachine.GetComponent<EarthPlayer>();

        earthPlayer.earthControls.controls.DialogueControls.Enable();

        earthPlayer.earthControls.controls.EarthPlayerDefault.Disable();
        earthPlayer.earthControls.controls.PlantIsSelected.Disable();
        earthPlayer.earthControls.controls.RemovingPlant.Disable();
        earthPlayer.earthControls.controls.HealSelect.Disable();
        earthPlayer.earthControls.controls.BarrierSelect.Disable();
    }

    public override void Execute(BaseStateMachine stateMachine)
    {
        
    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        //Debug.Log("Leaving dialogue");
        earthPlayer.earthControls.controls.DialogueControls.Disable();
    }
}
