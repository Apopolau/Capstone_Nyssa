using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Actions/Controls/Planting")]
public class CtrlsPlantingAction : FSMAction
{
    EarthPlayer earthPlayer;
    HUDManager hudManager;

    public override void EnterState(BaseStateMachine stateMachine)
    {
        earthPlayer = stateMachine.GetComponent<EarthPlayer>();
        hudManager = earthPlayer.GetHudManager();

        //Activate and deactivate the appropriate controls
        earthPlayer.earthControls.controls.PlantIsSelected.Enable();

        earthPlayer.earthControls.controls.DialogueControls.Disable();
        earthPlayer.earthControls.controls.EarthPlayerDefault.Disable();
        earthPlayer.earthControls.controls.BarrierSelect.Disable();
        earthPlayer.earthControls.controls.RemovingPlant.Disable();
        earthPlayer.earthControls.controls.HealSelect.Disable();

        //Set our appropriate bools
        earthPlayer.SetIsPlantSelected(true);
        earthPlayer.SetIsTileSelected(false);

        //Set transforms for relevant objects
        if(earthPlayer.GetSelectedPlant() == null)
            earthPlayer.InitializePlantPreview();
        earthPlayer.GetSelectedPlant().transform.position = earthPlayer.transform.position;
        earthPlayer.SetTileOutline(Instantiate(earthPlayer.GetTileOutlinePrefab(), earthPlayer.transform));

        earthPlayer.TurnOnTileSelect(earthPlayer.transform);
        hudManager.ToggleSproutPanel(true);
    }

    public override void Execute(BaseStateMachine stateMachine)
    {

    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        //Set our bools to facilitate a clean change in UI
        earthPlayer.SetIsTileSelected(true);
        
        //Turn our controls back off
        earthPlayer.earthControls.controls.PlantIsSelected.Disable();

        //Remove the plant preview
        Destroy(earthPlayer.GetSelectedPlant());

        //Update UI components
        earthPlayer.TurnOffTileSelect();
        /*
        if (earthPlayer.plantingControlsUI != null)
            { earthPlayer.plantingControlsUI.SetActive(false); }
        */
        hudManager.ToggleSproutPanel(false);
    }
}
