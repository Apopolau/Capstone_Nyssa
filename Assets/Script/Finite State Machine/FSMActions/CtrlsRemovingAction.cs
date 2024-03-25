using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/Controls/Removing")]
public class CtrlsRemovingAction : FSMAction
{
    EarthPlayer earthPlayer;

    public override void EnterState(BaseStateMachine stateMachine)
    {
        earthPlayer = stateMachine.GetComponent<EarthPlayer>();

        //Enable and disable the correct controls
        earthPlayer.earthControls.controls.RemovingPlant.Enable();

        earthPlayer.earthControls.controls.DialogueControls.Disable();
        earthPlayer.earthControls.controls.EarthPlayerDefault.Disable();
        earthPlayer.earthControls.controls.PlantIsSelected.Disable();
        earthPlayer.earthControls.controls.BarrierSelect.Disable();
        earthPlayer.earthControls.controls.HealSelect.Disable();

        //Set our appropriate bools
        earthPlayer.isRemovalStarted = true;
        earthPlayer.isATileSelected = false;

        //Create necessary prefabs
        //earthPlayer.tileOutline = Instantiate(earthPlayer.GetTileOutlinePrefab(), earthPlayer.transform);

        //Update the UI
        earthPlayer.SwitchCursorIcon(earthPlayer.i_shovel);
        //TurnOnTileSelect manages enabling most of the functionality of tile selection
        earthPlayer.TurnOnTileSelect(earthPlayer.transform);
    }

    public override void Execute(BaseStateMachine stateMachine)
    {

    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        //Turn our controls back off
        earthPlayer.earthControls.controls.RemovingPlant.Disable();

        earthPlayer.isATileSelected = true;

        //Update UI components
        //TurnOffTileSelect manages disabling most of the functionality of tile selection
        earthPlayer.TurnOffTileSelect();
        earthPlayer.HideTileText();
        earthPlayer.ResetImageColor(earthPlayer.GetPlantDarkenObject());
    }
}
