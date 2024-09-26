using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level One Progress", menuName = "Data Type/Level Progress/Level One")]
public class LevelOneProgress : LevelProgress
{
    LevelEventManager levelOneEvents;
    //Drag this from the plant prefabs folder
    [SerializeField]public GameObject treeSeedPrefab;
    [SerializeField] public GameObject grassSeedPrefab;
    private bool hasColdsnap;

    public override void SetPowers(bool active)
    {
        hasColdsnap = active;
    }

    protected override void OnAllObjectivesComplete()
    {
        readyToLeave = true;
    }

    protected override void OnPlayerWin()
    {

    }

    protected override void OnPlayerLoss()
    {

    }

    public override void SetEventManager(LevelEventManager eventManager)
    {
        levelOneEvents = eventManager;
    }

    public override LevelEventManager GetEventManager()
    {
        return levelOneEvents;
    }
}
