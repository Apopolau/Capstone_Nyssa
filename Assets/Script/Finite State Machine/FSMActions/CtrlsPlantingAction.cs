using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/Controls/Planting")]
public class CtrlsPlantingAction : FSMAction
{
    EarthPlayer earthPlayer;

    public override void EnterState(BaseStateMachine stateMachine)
    {
        Debug.Log("Entering planting");
        earthPlayer = stateMachine.GetComponent<EarthPlayer>();

        //Activate and deactivate the appropriate controls
        earthPlayer.earthControls.controls.PlantIsSelected.Enable();

        earthPlayer.earthControls.controls.DialogueControls.Disable();
        earthPlayer.earthControls.controls.EarthPlayerDefault.Disable();
        earthPlayer.earthControls.controls.BarrierSelect.Disable();
        earthPlayer.earthControls.controls.RemovingPlant.Disable();
        earthPlayer.earthControls.controls.HealSelect.Disable();

        //Set our appropriate bools
        earthPlayer.isPlantSelected = true;
        earthPlayer.isATileSelected = false;

        //Set transforms for relevant objects
        earthPlayer.plantSelected.transform.position = earthPlayer.transform.position;
        earthPlayer.tileOutline = Instantiate(earthPlayer.GetTileOutlinePrefab(), earthPlayer.transform);

        //Adjust the UI accordingly
        earthPlayer.TurnOnTileSelect(earthPlayer.transform);
        //earthPlayer.DisplayTileText();
        earthPlayer.DarkenAllImages(earthPlayer.GetPlantDarkenObject());
    }

    public override void Execute(BaseStateMachine stateMachine)
    {

    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        //Set our bools to facilitate a clean change in UI
        earthPlayer.isATileSelected = true;
        
        //Turn our controls back off
        earthPlayer.earthControls.controls.PlantIsSelected.Disable();

        //Remove the plant preview
        Destroy(earthPlayer.plantSelected);

        //Update UI components
        earthPlayer.TurnOffTileSelect();

        Debug.Log("Turning off planting");
    }
}
