using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Actions/Controls/Holding Nyssa")]
public class CtrlsHoldingNyssaAction : FSMAction
{
    HUDManager hudManager;
    EarthPlayer earthPlayer;

    public override void EnterState(BaseStateMachine stateMachine)
    {
        earthPlayer = stateMachine.GetComponent<EarthPlayer>();
        hudManager = earthPlayer.GetHudManager();

        earthPlayer.earthControls.controls.DialogueControls.Disable();
        earthPlayer.earthControls.controls.EarthPlayerDefault.Disable();
        earthPlayer.earthControls.controls.PlantIsSelected.Disable();
        earthPlayer.earthControls.controls.RemovingPlant.Disable();
        earthPlayer.earthControls.controls.HealSelect.Disable();
        earthPlayer.earthControls.controls.BarrierSelect.Disable();
        earthPlayer.earthControls.controls.HoldingNyssa.Enable();

        //earthPlayer.DarkenAllImages(earthPlayer.GetPlantDarkenObject());
        hudManager.ToggleSproutPanel(false);
    }

    public override void Execute(BaseStateMachine stateMachine)
    {

    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        earthPlayer.SetHoldingNyssa(false);
        //earthPlayer.uiController.RestoreUI(earthPlayer.GetPlantDarkenObject());
        hudManager.ToggleSproutPanel(true);
        earthPlayer.earthControls.controls.HoldingNyssa.Disable();
    }
}
