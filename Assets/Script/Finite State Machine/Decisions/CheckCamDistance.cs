using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCamDistance : Decision
{
    SplitScreen splitScreen;

    public override bool Decide(BaseStateMachine stateMachine)
    {
        splitScreen = stateMachine.GetComponent<SplitScreen>();

        if (!splitScreen.inCutscene)
        {
            if (Vector3.Distance(splitScreen.earthPlayer.transform.position, splitScreen.celestialPlayer.transform.position) > splitScreen.distance)
            {
                return true;
            }
            else if (Vector3.Distance(splitScreen.earthPlayer.transform.position, splitScreen.celestialPlayer.transform.position) < splitScreen.distance)
            {
                return false;
            }
        }
        return false;
    }
}
