using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Line", menuName = "UI/Dialogue/SpriteLibrary")]
public class SpriteLibrary : ScriptableObject
{
    // List of sprites that can be used
    public Sprite Default;
    public Sprite Silhouette;
    public Sprite Happy;
    public Sprite Upset;
    public Sprite Angry;
    public Sprite Determined;

}
