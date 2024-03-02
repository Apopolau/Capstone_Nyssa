using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Line", menuName = "Dialogue/Line")]
public class DialogueLine : ScriptableObject
{
    public enum Character { CELESTE, SPROUT, DUCK, HEDGEHOG, FOX, NYSSA };
    [SerializeField] public Character speaker;
    //We may want to expand on these options later, but I figured I'd start it off
    public enum Emotion { DEFAULT, SILHOUETTE, HAPPY, UPSET, ANGRY, DETERMINED };
    [SerializeField] public Emotion emotion;

    //public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
    [TextArea(3, 10)]
    public string lineFR; // Text for French language
}
