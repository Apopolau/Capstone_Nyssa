using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue")]
public class Dialogue : ScriptableObject
{
    public UserSettingsManager userSettingsManager;
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}
