using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToCamera : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        //mainCamera = mainCamera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
      // Get the camera's rotation
        Quaternion cameraRotation = mainCamera.transform.rotation;

        // Ensure that the pickupTarget is always facing the camera, including its rotation
        transform.LookAt(mainCamera.transform.position);
        transform.rotation *= Quaternion.Euler(cameraRotation.eulerAngles.x, cameraRotation.eulerAngles.y, 0);
    }
}
