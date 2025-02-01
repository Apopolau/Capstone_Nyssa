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
        if(earthPlayer == null || hudManager == null)
        {
            earthPlayer = stateMachine.GetComponent<EarthPlayer>();
            hudManager = earthPlayer.GetHudManager();
        }
        

        earthPlayer.earthControls.ShutOffControls();
        earthPlayer.earthControls.controls.HoldingNyssa.Enable();

        hudManager.SetSproutOccupied(true);
        hudManager.ToggleSproutPanel(false);
    }

    public override void Execute(BaseStateMachine stateMachine)
    {

    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        earthPlayer.SetHoldingNyssa(false);
        hudManager.SetSproutOccupied(false);
        hudManager.ToggleSproutPanel(true);
        earthPlayer.earthControls.controls.HoldingNyssa.Disable();
    }
}
