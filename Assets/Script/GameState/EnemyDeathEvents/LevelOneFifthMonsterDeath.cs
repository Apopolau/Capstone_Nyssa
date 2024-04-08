using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/L1FinalOils", fileName = "L1OilPairDeath")]
public class LevelOneFifthMonsterDeath : EnemyDeathBehaviour
{
    //Get your Level Manager object's levelEvents script
    [SerializeField] LevelManagerObject levelOneManager;
    [SerializeField] LevelOneEvents levelOneEvents;

    public override void CheckIfDead()
    {
        levelOneEvents = (LevelOneEvents)levelOneManager.eventManager;
        levelOneEvents.CountDownFinalMonsters();
    }
}
