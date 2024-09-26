using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Three Progress", menuName = "Data Type/Level Progress/Level Three")]
public class LevelThreeProgress : LevelProgress
{
    LevelEventManager levelThreeEvents;
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

    public override void SetEventManager(LevelEventManager eventManager)
    {
        levelThreeEvents = eventManager;
    }

    public override LevelEventManager GetEventManager()
    {
        return levelThreeEvents;
    }
}
