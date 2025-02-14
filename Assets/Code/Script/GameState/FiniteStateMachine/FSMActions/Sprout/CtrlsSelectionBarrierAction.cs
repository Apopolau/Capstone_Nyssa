using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Actions/Controls/SelectionBarrier")]
public class CtrlsSelectionBarrierAction : FSMAction
{
    EarthPlayer earthPlayer;
    HUDManager hudManager;

    public override void EnterState(BaseStateMachine stateMachine)
    {
        earthPlayer = stateMachine.GetComponent<EarthPlayer>();
        hudManager = earthPlayer.GetHudManager();

        //Set all of the appropriate controls
        earthPlayer.earthControls.controls.BarrierSelect.Enable();

        earthPlayer.earthControls.controls.DialogueControls.Disable();
        earthPlayer.earthControls.controls.EarthPlayerDefault.Disable();
        earthPlayer.earthControls.controls.PlantIsSelected.Disable();
        earthPlayer.earthControls.controls.RemovingPlant.Disable();
        earthPlayer.earthControls.controls.HealSelect.Disable();

        //Get targets for shielding
        earthPlayer.PickClosestTarget();
        earthPlayer.SetTurnTarget(earthPlayer.GetPowerTarget());
        earthPlayer.ToggleTurning(true);

        //Instantiate appropriate objects
        earthPlayer.SetTileOutline(Instantiate(earthPlayer.GetTileOutlinePrefab(), earthPlayer.GetPowerTarget().transform));
        earthPlayer.GetTileOutline().GetComponentInChildren<SpriteRenderer>().color = Color.green;

        //Update UI
        hudManager.TurnOnPopUpText("Select a target to shield", "Sélectionnez une cible à Shield");
        hudManager.ToggleSproutPanel(true);
    }

    public override void Execute(BaseStateMachine stateMachine)
    {
        Vector3 targetLocation = new Vector3(earthPlayer.GetPowerTarget().transform.position.x, earthPlayer.GetPowerTarget().transform.position.y + 1, earthPlayer.GetPowerTarget().transform.position.z);
        earthPlayer.GetTileOutline().transform.position = targetLocation;
    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        //Turn these controls off
        earthPlayer.earthControls.controls.BarrierSelect.Disable();
        earthPlayer.ToggleTurning(false);

        //Update UI
        hudManager.TurnOffPopUpText();
        Destroy(earthPlayer.GetTileOutline());
        hudManager.ToggleSproutPanel(false);
    }
}
