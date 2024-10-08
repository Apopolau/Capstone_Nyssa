using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FH_PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f;
    private float horizontalInput;
    private float forwardInput;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //get player input
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        //Move the player forward
        transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
         transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput); 
    }
}
