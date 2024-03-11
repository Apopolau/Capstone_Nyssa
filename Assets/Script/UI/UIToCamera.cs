using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToCamera : MonoBehaviour
{
    [SerializeField] Transform mainCamera;
    // Start is called before the first frame update
    void Awake()
    {
        mainCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
       // Get the camera's rotation
        //Quaternion cameraRotation = mainCamera.transform.rotation;

        // Ensure that the pickupTarget is always facing the camera, including its rotation
        //transform.LookAt(mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, mainCamera.transform.position.z)));

        // Extract the Z-axis rotation from the camera's rotation
        //Quaternion rotation = Quaternion.Euler(cameraRotation.eulerAngles.x, cameraRotation.eulerAngles.z - GetComponentInParent<Transform>().rotation.y, cameraRotation.eulerAngles.y);

        // Apply rotation around the Z-axis only
        transform.forward = mainCamera.forward;
        //transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
