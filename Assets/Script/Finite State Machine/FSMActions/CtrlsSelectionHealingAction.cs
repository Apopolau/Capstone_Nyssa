using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/Controls/SelectionHeal")]
public class CtrlsSelectionHealingBarrierAction : FSMAction
{
    EarthPlayer earthPlayer;

    public override void EnterState(BaseStateMachine stateMachine)
    {
        earthPlayer = stateMachine.GetComponent<EarthPlayer>();

        //Set all the appropriate controls
        earthPlayer.earthControls.controls.HealSelect.Enable();

        earthPlayer.earthControls.controls.DialogueControls.Disable();
        earthPlayer.earthControls.controls.EarthPlayerDefault.Disable();
        earthPlayer.earthControls.controls.PlantIsSelected.Disable();
        earthPlayer.earthControls.controls.RemovingPlant.Disable();
        earthPlayer.earthControls.controls.BarrierSelect.Disable();

        //Initiate targeting
        earthPlayer.PickClosestTarget();

        //Instantiate appropriate objects
        earthPlayer.tileOutline = Instantiate(earthPlayer.GetTileOutlinePrefab(), earthPlayer.GetPowerTarget().transform);
        earthPlayer.tileOutline.GetComponentInChildren<SpriteRenderer>().color = Color.green;

        //Update the UI
        earthPlayer.displayText.text = "Select a target to heal";
        earthPlayer.uiController.DarkenOverlay(earthPlayer.GetPlantDarkenObject());
    }

    public override void Execute(BaseStateMachine stateMachine)
    {
        
    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        //Turn our barrier controls back off
        earthPlayer.earthControls.controls.HealSelect.Disable();

        //Restore the UI
        earthPlayer.displayText.text = "";
        Destroy(earthPlayer.tileOutline);
        earthPlayer.uiController.RestoreUI(earthPlayer.GetPlantDarkenObject());
    }
}
