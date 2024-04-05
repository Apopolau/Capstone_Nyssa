using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mission End", menuName = "Dialogue/MissionEnd")]
public class DialogueMissionEnd : DialogueEvent
{

    [SerializeField] string targetScene;

    public string GetTargetScene()
    {
        return targetScene;
    }
}
