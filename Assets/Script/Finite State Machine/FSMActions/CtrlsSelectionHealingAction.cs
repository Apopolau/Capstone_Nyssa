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

        earthPlayer.SetTurnTarget(earthPlayer.GetPowerTarget().transform.position);
        earthPlayer.ToggleTurning();

        //Instantiate appropriate objects
        earthPlayer.tileOutline = Instantiate(earthPlayer.GetTileOutlinePrefab(), earthPlayer.GetPowerTarget().transform);
        earthPlayer.tileOutline.GetComponentInChildren<SpriteRenderer>().color = Color.green;

        //Update the UI
        earthPlayer.displayText.text = "Select a target to heal";
        earthPlayer.uiController.DarkenOverlay(earthPlayer.GetPlantDarkenObject());
        if (earthPlayer.spellsControlsUI != null)
        { earthPlayer.spellsControlsUI.SetActive(true); }
    }

    public override void Execute(BaseStateMachine stateMachine)
    {
        Vector3 targetLocation = new Vector3(earthPlayer.GetPowerTarget().transform.position.x, earthPlayer.GetPowerTarget().transform.position.y + 1, earthPlayer.GetPowerTarget().transform.position.z);
        earthPlayer.tileOutline.transform.position = targetLocation;
    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        //Turn our barrier controls back off
        earthPlayer.earthControls.controls.HealSelect.Disable();
        earthPlayer.ToggleTurning();

        //Restore the UI
        earthPlayer.displayText.text = "";
        Destroy(earthPlayer.tileOutline);
        earthPlayer.uiController.RestoreUI(earthPlayer.GetPlantDarkenObject());
        if (earthPlayer.spellsControlsUI != null)
        { earthPlayer.spellsControlsUI.SetActive(false); }
    }
}
