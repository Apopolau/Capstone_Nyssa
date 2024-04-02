using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Movement", menuName = "Dialogue/Move")]
public class DialogueMoveEvent : DialogueEvent
{
    [SerializeField] private GameObject targetGameObject;
    [SerializeField] private Vector3 position;
    [SerializeField] private Quaternion rotation;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float animationTimer;
    [SerializeField] private WaitForSecondsRealtime animationTime;

    public GameObject GetObjectToMove()
    {
        return targetGameObject;
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

    public void SetMoveToThis(GameObject objectToSet)
    {
        targetGameObject = objectToSet;
    }
}
