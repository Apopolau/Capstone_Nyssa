using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Level 2 Final Oils", fileName = "L2FinalOilsDeath")]
public class LevelTwoFinalOils : EnemyDeathBehaviour
{
    //Get your Level Manager object's levelEvents script
    [SerializeField] LevelManagerObject levelTwoManager;
    [SerializeField] LevelTwoEvents levelTwoEvents;

    public override void CheckIfDead(Enemy enemy)
    {
        levelTwoEvents = (LevelTwoEvents)levelTwoManager.eventManager;
        levelTwoEvents.CountDownArea4Monsters();

    }
}
