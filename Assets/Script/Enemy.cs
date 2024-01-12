using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent enemyMeshAgent;
    public GameObject playerObj;
    public bool seesPlayer = false;
    public Vector3 playerLocation;
    // Start is called before the first frame update
    void Start()
    {
        enemyMeshAgent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("Entered collision with " + other.gameObject.name);
        if (other.gameObject == playerObj)
        {
            //Player is in range of enemy, in invading monster they can pursue the player
            seesPlayer = true;
            //Debug.Log("~~~~~~~~~~~~~~~~~~~Entered collision with " + other.gameObject.name); 
            playerLocation = other.transform.position;
     
        }
        if (other.gameObject.tag == "plant")
        {
            //Player is in range of enemy, in invading monster they can pursue the player
            seesPlayer = true;
           // Debug.Log("~~~~~~~~~~~~~~~~~~~Entered collision with " + other.gameObject.name);
            playerLocation = other.transform.position;

        }


    }
        private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Coliding with " + other.gameObject.name);
        if (other.gameObject == playerObj)
        {
            //Player is in range of enemy, in invading monster they can pursue the player
            seesPlayer = true;
            playerLocation = other.transform.position;
            //Debug.Log("~~~~~~~~~~~~~~~~~~~Coliding with " + other.gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
       // Debug.Log("Exited collision with " + other.gameObject.name);
        if (other.gameObject == playerObj)
        {
            //Player is in range of enemy, in invading monster they can pursue the player
            seesPlayer = false;
            playerLocation = other.transform.position;
            //Debug.Log("~~~~~~~~~~~~~~~~~Exited collision with " + other.gameObject.name);
        }
    }
}
