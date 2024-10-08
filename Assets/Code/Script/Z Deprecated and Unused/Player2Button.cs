using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Material player1MatRef;
    public Material player2MatRef;
    public Material defaultMat;
    public GameObject Bridge;




    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player2")
        {

            //print("I have found my master");
            //GetComponent.<Renderer>().material = newMat;
            Bridge.GetComponent<Renderer>().material = player1MatRef;
            Bridge.GetComponent<BoxCollider>().enabled = true;

        }
        else
        {
            Bridge.GetComponent<Renderer>().material = defaultMat;
            Bridge.GetComponent<BoxCollider>().enabled = false;
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player2")
        {

            Bridge.GetComponent<Renderer>().material = defaultMat;
            Bridge.GetComponent<BoxCollider>().enabled = false;
        }
    }



}
