using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Subtype of RuntimeSet that includes a list of gameobjects
[CreateAssetMenu(fileName = "New Game Object Set", menuName = "RuntimeSet/GameObjectSet")]
public class GameObjectRuntimeSet : RuntimeSet<GameObject>
{

}
