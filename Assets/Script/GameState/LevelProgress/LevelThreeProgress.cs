using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Three Progress", menuName = "ManagerObject/LevelProgress/LevelThree")]
public class LevelThreeProgress : LevelProgress
{
    bool hasOpenedBridge1;
    bool hasOpenedBridge2;

    public override void SetPowers(bool active)
    {
        //
    }

    protected override void OnAllObjectivesComplete()
    {

    }

    protected override void OnPlayerWin()
    {

    }

    protected override void OnPlayerLoss()
    {

    }
}
