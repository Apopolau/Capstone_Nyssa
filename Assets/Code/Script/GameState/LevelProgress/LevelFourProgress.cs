using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Four Progress", menuName = "Data Type/Level Progress/Level Four")]
public class LevelFourProgress : LevelProgress
{
    LevelEventManager levelFourEvents;
    bool hasFoundRabbit;

    /*
    public override bool EvaluateFood()
    {
        if (EvaluateTrees() && EvaluateGrass() && EvaluateCattails() && EvaluateFlowers() && EvaluateLilies())
        {
            animalHasEnoughFood = true;
            return true;
        }
        return false;
    }

    public override bool EvaluateLevelProgress()
    {
        if (animalHasEnoughFood && cleanWater && animalIsSafe)
        {
            OnAllObjectivesComplete();
            return true;
        }
        return false;
    }
    */

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
        levelFourEvents = eventManager;
    }

    public override LevelEventManager GetEventManager()
    {
        return levelFourEvents;
    }
}
