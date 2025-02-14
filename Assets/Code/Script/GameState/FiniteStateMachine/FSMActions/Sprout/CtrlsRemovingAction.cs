using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Actions/Controls/Removing")]
public class CtrlsRemovingAction : FSMAction
{
    EarthPlayer earthPlayer;
    HUDManager hudManager;

    public override void EnterState(BaseStateMachine stateMachine)
    {
        earthPlayer = stateMachine.GetComponent<EarthPlayer>();
        hudManager = earthPlayer.GetHudManager();

        //Enable and disable the correct controls
        earthPlayer.earthControls.controls.RemovingPlant.Enable();

        earthPlayer.earthControls.controls.DialogueControls.Disable();
        earthPlayer.earthControls.controls.EarthPlayerDefault.Disable();
        earthPlayer.earthControls.controls.PlantIsSelected.Disable();
        earthPlayer.earthControls.controls.BarrierSelect.Disable();
        earthPlayer.earthControls.controls.HealSelect.Disable();

        //Create necessary prefabs
        //earthPlayer.tileOutline = Instantiate(earthPlayer.GetTileOutlinePrefab(), earthPlayer.transform);

        //Update the UI
        //earthPlayer.SwitchCursorIcon(earthPlayer.i_shovel);
        hudManager.SetVirtualMouseImage(earthPlayer.GetShovelIcon());
        //TurnOnTileSelect manages enabling most of the functionality of tile selection
        earthPlayer.SetTileOutline(Instantiate(earthPlayer.GetTileOutlinePrefab(), earthPlayer.transform));
        earthPlayer.TurnOnTileSelect(earthPlayer.transform);

        hudManager.TurnOnPopUpText("Select a Tile", "Veuillez sélectionner une tuile");
        hudManager.ToggleSproutPanel(true);

        //Set our appropriate bools
        earthPlayer.SetIsRemovalStarted(true);
        earthPlayer.SetIsTileSelected(false);
    }

    public override void Execute(BaseStateMachine stateMachine)
    {

    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        //Turn our controls back off
        earthPlayer.earthControls.controls.RemovingPlant.Disable();

        earthPlayer.SetIsTileSelected(true);

        //Update UI components
        //TurnOffTileSelect manages disabling most of the functionality of tile selection
        earthPlayer.TurnOffTileSelect();
        hudManager.TurnOffPopUpText();
        /*
         if (earthPlayer.plantingControlsUI != null)
        { earthPlayer.plantingControlsUI.SetActive(false); }
        */
        //earthPlayer.ResetImageColor(earthPlayer.GetPlantDarkenObject());
        hudManager.ToggleSproutPanel(false);
    }
}
