using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.AI;

public class CelestialPlayer : MonoBehaviour
{
    
    [SerializeField] private VirtualMouseInput virtualMouseInput;
    [SerializeField] public Camera mainCamera;
    [SerializeField] private LayerMask tileMask;

    private NavMeshAgent celestialAgent;

    [Header("Rain System")]
    [SerializeField] public bool isRaining=false;
    [SerializeField] public GameObject RainParticleSystem;


    [Header("Attack")]
    [SerializeField] public bool isAttacking = false;
    private Vector3 OrigPos = new Vector3(70,7,-40);
    ColdSnapBehaviour coldSnap;
    //private CelestialPlayerInputActions celestialPlayerInput;
    private PlayerInput playerInput;
    // [Header("Lightning System")]
    // Start is called before the first frame update


    //Battle 
    private int healthPoints;

    private void Awake()
    {
        celestialAgent = GetComponent<NavMeshAgent>();
        celestialAgent.enabled = false;
        healthPoints=100;
        virtualMouseInput.gameObject.GetComponentInChildren<Image>().enabled = false;
    }


    void Start()
    {
        // celestialPlayerInput = GetComponent<CelestialPlayerInputActions>();
        playerInput = GetComponent<PlayerInput>();
        coldSnap = GetComponent<ColdSnapBehaviour>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    public bool TakeHit()
    {

       healthPoints -= 10;
        
        Debug.Log(healthPoints);
        bool isDead = healthPoints <= 0;
        if (isDead)
        {
           Respawn();
        }

        return isDead;

    }
    private void Respawn()
    {

        healthPoints = 100;
        gameObject.transform.position = OrigPos;

    }

    //if player selects raindrop
    public void OnRainDropSelected() {
        //RainParticleSystem.SetActive(true);
        isRaining = true;

    }
    public IEnumerator ResetRain()
    {
        Debug.Log("It is currently raining");
        //if (isRaining)
       // {
            yield return new WaitForSeconds(5f);
            Debug.Log("******It is no longer raining****");
        isRaining = true;

        // RainParticleSystem.SetActive(false);


        //}

    }
 
    public void OnSnowFlakeSelected() { 
    
    }
    public IEnumerator ResetColdSnap()
    {
        GameObject coldOrb = coldSnap.GetComponent<ColdSnapBehaviour>().ColdSnapStats.visualDisplay;
        Instantiate(coldOrb, gameObject.transform.position, Quaternion.identity);
        Rigidbody coldRB = coldOrb.GetComponent<Rigidbody>();
        coldRB.AddForce(0, 0, 2f,ForceMode.Acceleration);

        //////////////coldOrb.transform.forward.
  
            isAttacking = true;
        yield return new WaitForSeconds(2f);
       
    }
    public IEnumerator ResetThundrerStrike()
    {
        yield return new WaitForSeconds(1f);
       
    }
}
