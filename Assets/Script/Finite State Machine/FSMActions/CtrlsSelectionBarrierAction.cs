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
        earthPlayer.SetTurnTarget(earthPlayer.GetPowerTarget().transform.position);
        earthPlayer.ToggleTurning();

        //Instantiate appropriate objects
        earthPlayer.tileOutline = Instantiate(earthPlayer.GetTileOutlinePrefab(), earthPlayer.GetPowerTarget().transform);
        earthPlayer.tileOutline.GetComponentInChildren<SpriteRenderer>().color = Color.green;

        //Update UI
        earthPlayer.displayText.text = "Select a target to shield";
        earthPlayer.uiController.DarkenOverlay(earthPlayer.GetPlantDarkenObject());
    }

    public override void Execute(BaseStateMachine stateMachine)
    {
        
        Vector3 targetLocation = new Vector3(earthPlayer.GetPowerTarget().transform.position.x, earthPlayer.GetPowerTarget().transform.position.y + 1, earthPlayer.GetPowerTarget().transform.position.z);
        earthPlayer.tileOutline.transform.position = targetLocation;
    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        //Turn these controls off
        earthPlayer.earthControls.controls.BarrierSelect.Disable();
        earthPlayer.ToggleTurning();

        //Update UI
        earthPlayer.displayText.text = "";
        Destroy(earthPlayer.tileOutline);
    }
}
