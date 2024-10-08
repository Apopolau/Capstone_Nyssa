using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputHandler : MonoBehaviour
{
    public GameObject playerPrefab;
    playerMovement playController;
    // Start is called before the first frame update

    Vector3 startPos= new Vector3 (0,0,0);
    void Awake()
    {
        
        if (playerPrefab != null) {
            playController = GameObject.Instantiate(playerPrefab, startPos, transform.rotation).GetComponent<playerMovement>();
            transform.parent = playController.transform;
                
                
                }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
