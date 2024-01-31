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
    void Update()
    {
       transform.LookAt(mainCamera.transform.position + mainCamera.transform.forward);
       transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
}
