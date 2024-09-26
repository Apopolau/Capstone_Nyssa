using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Movement", menuName = "UI/Dialogue/Move")]
public class DialogueMoveEvent : DialogueEvent
{
    [SerializeField] private bool playsOut;

    [SerializeField] private bool hasMove;
    [SerializeField] private bool hasLocationMove;
    [SerializeField] private GameObject targetMoveObject;
    [SerializeField] private bool hasObjectMove;
    [SerializeField] private GameObject targetLocationObject;
    [SerializeField] private Vector3 position;
    [SerializeField] private bool hasRotation;
    [SerializeField] private Quaternion rotation;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float animationTimer;
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

    public Vector3 GetPosition()
    {
        return position;
    }

    public Quaternion GetRotation()
    {
        return rotation;
    }

    public float GetMovementSpeed()
    {
        return movementSpeed;
    }

    public float GetRotationSpeed()
    {
        return rotationSpeed;
    }

    public WaitForSecondsRealtime GetAnimationTimer()
    {
        animationTime = new WaitForSecondsRealtime(animationTimer);
        return animationTime;
    }

    public float GetAnimationSpeed()
    {
        return animationTimer;
    }

    public void SetThisObjectToMove(GameObject objectToSet)
    {
        targetMoveObject = objectToSet;
    }

    public void SetLocationToThis(GameObject objectToSet)
    {
        targetLocationObject = objectToSet;
    }

    public bool MovePlaysOut()
    {
        return playsOut;
    }
}
