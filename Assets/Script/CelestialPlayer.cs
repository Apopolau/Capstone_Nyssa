using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.AI;

public class CelestialPlayer : MonoBehaviour
{
    
    [SerializeField] private VirtualMouseInput virtualMouseInput;
    [SerializeField] public Camera mainCamera;
    [SerializeField] private LayerMask tileMask;

    [Header("Rain System")]
    [SerializeField] public bool isRaining=false;
    [SerializeField] public GameObject RainParticleSystem;

    //private CelestialPlayerInputActions celestialPlayerInput;
    private PlayerInput playerInput;
    // [Header("Lightning System")]
    // Start is called before the first frame update
    void Start()
    {
        // celestialPlayerInput = GetComponent<CelestialPlayerInputActions>();
        playerInput = GetComponent<PlayerInput>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    //if player selects raindrop
    public void OnRainDropSelected() {
       
    }
    public void OnSnowFlakeSelected() { }
}
