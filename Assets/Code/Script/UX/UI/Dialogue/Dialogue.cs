using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "UI/Dialogue/Dialogue")]
public class Dialogue : ScriptableObject
{
    public UserSettingsManager userSettingsManager;
    public bool levelEndDialogue;
    public List<DialogueEvent> dialogueEvents = new List<DialogueEvent>();
}
