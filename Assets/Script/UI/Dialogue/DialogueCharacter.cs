using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Line", menuName = "Dialogue/Character")]
public class DialogueCharacter : ScriptableObject
{
    public string characterName;
    //This will have all of the sprites for a single character
    [SerializeField] public SpriteLibrary characterSprites;

    // Icon for the left side of the dialogue box
    public Sprite iconLeft;

    // Icon for the right side of the dialogue box
    public Sprite iconRight;

    // Whether the character should appear on the left side of the dialogue box
    public bool isLeft;
}
