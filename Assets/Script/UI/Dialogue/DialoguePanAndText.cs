using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Because this inherits from DialogueLine, it has all the same functionality
[CreateAssetMenu(fileName = "New Pan and line", menuName = "UI/Dialogue/Pan&Line")]
public class DialoguePanAndText : DialogueEvent
{
    [SerializeField] public DialogueCameraPan dialogueCameraPan;
    [SerializeField] public DialogueLine dialogueLine;
}
