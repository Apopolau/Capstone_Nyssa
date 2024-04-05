using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mission End", menuName = "Dialogue/MissionEnd")]
public class DialogueMissionEnd : DialogueEvent
{
    [SerializeField] PlayerInputActions celestialControls;
    [SerializeField] PlayerInputActions earthControls;
    [SerializeField] string targetScene;

    public string GetTargetScene()
    {
        return targetScene;
    }

    public PlayerInputActions GetCelestialControls()
    {
        return celestialControls;
    }

    public PlayerInputActions GetEarthControls()
    {
        return earthControls;
    }
}
