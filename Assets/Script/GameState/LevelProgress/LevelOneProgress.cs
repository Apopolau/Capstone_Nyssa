using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level One Progress", menuName = "ManagerObject/LevelProgress/LevelOne")]
public class LevelOneProgress : LevelProgress
{
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
}
