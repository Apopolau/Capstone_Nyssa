using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Actions/Controls/SelectionHeal")]
public class CtrlsSelectionHealingBarrierAction : FSMAction
{
    EarthPlayer earthPlayer;
    HUDManager hudManager;

    public override void EnterState(BaseStateMachine stateMachine)
    {
        earthPlayer = stateMachine.GetComponent<EarthPlayer>();
        hudManager = earthPlayer.GetHudManager();

        //Set all the appropriate controls
        earthPlayer.earthControls.controls.HealSelect.Enable();

        earthPlayer.earthControls.controls.DialogueControls.Disable();
        earthPlayer.earthControls.controls.EarthPlayerDefault.Disable();
        earthPlayer.earthControls.controls.PlantIsSelected.Disable();
        earthPlayer.earthControls.controls.RemovingPlant.Disable();
        earthPlayer.earthControls.controls.BarrierSelect.Disable();

        //Initiate targeting
        earthPlayer.PickClosestTarget();

        earthPlayer.SetTurnTarget(earthPlayer.GetPowerTarget());
        earthPlayer.ToggleTurning(true);

        //Instantiate appropriate objects
        earthPlayer.SetTileOutline(Instantiate(earthPlayer.GetTileOutlinePrefab(), earthPlayer.GetPowerTarget().transform));
        earthPlayer.GetTileOutline().GetComponentInChildren<SpriteRenderer>().color = Color.green;

        //Update the UI
        hudManager.TurnOnPopUpText("Select a target to heal", "Sélectionnez une cible pour guérir");
        hudManager.ToggleSproutPanel(true);
    }

    public override void Execute(BaseStateMachine stateMachine)
    {
        Vector3 targetLocation = new Vector3(earthPlayer.GetPowerTarget().transform.position.x, earthPlayer.GetPowerTarget().transform.position.y + 1, earthPlayer.GetPowerTarget().transform.position.z);
        earthPlayer.GetTileOutline().transform.position = targetLocation;
    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        //Turn our barrier controls back off
        earthPlayer.earthControls.controls.HealSelect.Disable();
        earthPlayer.ToggleTurning(false);

        //Restore the UI
        hudManager.TurnOffPopUpText();
        Destroy(earthPlayer.GetTileOutline());
        hudManager.ToggleSproutPanel(false);
    }
}
