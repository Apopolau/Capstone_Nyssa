using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Four Progress", menuName = "ManagerObject/LevelProgress/LevelFour")]
public class LevelFourProgress : LevelProgress
{
    bool hasFoundRabbit;

    bool securedWater;

    public bool animalHasEnoughFood = false;
    //public bool animalHasEnoughWater = false;
    //public bool animalHasShelter = false;
    public bool animalIsSafe = false;

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
