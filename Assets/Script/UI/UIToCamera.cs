using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToCamera : MonoBehaviour
{
    //[SerializeField] Transform mainCamera;
    [SerializeField] GameObjectRuntimeSet cameraSet;
    private WaitForSeconds turnInterval;
    // Start is called before the first frame update

    private void Start()
    {
        turnInterval = new WaitForSeconds(0.1f);
        StartCoroutine(TurnToCamera());
        foreach (var camera in cameraSet.Items)
        {
            if (camera != null)
            {
                transform.forward = camera.GetComponent<Camera>().transform.forward;
                break;
            }
        }
    }

    private IEnumerator TurnToCamera()
    {
        while (true)
        {
            yield return turnInterval;

            foreach (var camera in cameraSet.Items)
            {
                if (camera != null)
                {
                    transform.forward = camera.GetComponent<Camera>().transform.forward;
                    break;
                }
            }
        }
    }
}
