using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Movement", menuName = "UI/Dialogue/Move")]
public class DialogueMoveEvent : DialogueEvent
{
    //If not checked off and player skips, the target will be instantly moved to the destination
    [Tooltip("Whether or not the player is forced to watch the entire movement play out.")]
    [SerializeField] private bool playsOut;

    [Tooltip("Whether or not the object is supposed to physically move")]
    [SerializeField] private bool hasMove;
    [Tooltip("Whether or not the object that is being moved is supposed to move to a specific location.")]
    [SerializeField] private bool hasLocationMove;
    [Tooltip("The object that is being moved. This needs to be set, but may be set by another script.")]
    [SerializeField] private GameObject targetMoveObject;
    [Tooltip("Whether or not the object that is being moved is supposed to move to another object.")]
    [SerializeField] private bool hasObjectMove;
    [Tooltip("The location object that the moved object is supposed to move to. Only set if the bool is set.")]
    [SerializeField] private GameObject targetLocationObject;
    [Tooltip("Location the object is supposed to move to. Only set if the bool is set.")]
    [SerializeField] private Vector3 position;
    [Tooltip("Whether or not the object is supposed to rotate")]
    [SerializeField] private bool hasRotation;
    [Tooltip("The angle of the rotation. Only set if the bool is set.")]
    [SerializeField] private Quaternion rotation;
    [Tooltip("The maximum speed the object can move at during the animation.")]
    [SerializeField] private float movementSpeed;
    [Tooltip("Maximum speed the object can rotate.")]
    [SerializeField] private float rotationSpeed;
    [Tooltip("The time over seconds that the animation plays out.")]
    [SerializeField] private float animationTimer;
    //This field doesn't appear in the inspector. It's set here in the script
    [SerializeField] private WaitForSecondsRealtime animationTime;


    public bool HasMove()
    {
        return hasMove;
    }

    public bool IsLocationMoveType()
    {
        return hasLocationMove;
    }

    public bool IsObjectMoveType()
    {
        return hasObjectMove;
    }

    public bool HasRotation()
    {
        return hasRotation;
    }

    public GameObject GetObjectToMove()
    {
        return targetMoveObject;
    }

    public GameObject GetObjectToMoveTo()
    {
        return targetLocationObject;
    }

    //Get the position of the object
    public Vector3 GetPosition()
    {
        return position;
    }

    //Get the angle of the rotation
    public Quaternion GetRotation()
    {
        return rotation;
    }

    //Get the speed of the movement
    public float GetMovementSpeed()
    {
        return movementSpeed;
    }

    //Get the speed of the rotation
    public float GetRotationSpeed()
    {
        return rotationSpeed;
    }

    //Get the animation time in WaitForSeconds
    public WaitForSecondsRealtime GetAnimationTimer()
    {
        animationTime = new WaitForSecondsRealtime(animationTimer);
        return animationTime;
    }

    //Get the time the movement is supposed to play out over
    public float GetAnimationSpeed()
    {
        return animationTimer;
    }

    //Sets which object is supposed to be moved
    public void SetThisObjectToMove(GameObject objectToSet)
    {
        targetMoveObject = objectToSet;
    }

    //Move the object instantly
    public void SetLocationToThis(GameObject objectToSet)
    {
        targetLocationObject = objectToSet;
    }

    //Returns whether or not the move action has to finish before the player can proceed
    public bool MovePlaysOut()
    {
        return playsOut;
    }
}
