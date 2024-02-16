using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class GameManager : MonoBehaviour
{
  /*  public GameObject[] spawnpoints;
    public List<PlayerInput> playerList = new List<PlayerInput>();
    public GameObject playerA;
    public GameObject playerB;
    public GameObject otherPrefab;



    [SerializeField] InputAction joinAction;
    [SerializeField] InputAction leaveAction;
    public PlayerInputManager inputManager;

    private int intIndex = 0;

    //INSTANCES
    public static GameManager instance = null;

    //EVENTS
    public event System.Action<PlayerInput> PlayerJoinedGame;

    public event System.Action<PlayerInput> PlayerLeftGame;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        //if there is already anothergame bject in the world destroy it
        else if (instance != null)
        {
            Destroy(gameObject);
            //instance = this;
        }

        spawnpoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        joinAction.Enable();
        joinAction.performed += context => JoinAction(context);
        leaveAction.Enable();

    }
    private void Start()
    {
        PlayerInputManager.instance.JoinPlayer(0, -1, null);
        // PlayerInputManager.instance.JoinPlayer(1, -1, null);
    }
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("playerJoined the game");
        playerList.Add(playerInput);




        Debug.Log(playerList.Count);

        // if (PlayerJoinedGame != null)
        if (playerA == null)
        {
            // PlayerJoinedGame(playerInput);
            playerA = playerInput.gameObject;
            inputManager.playerPrefab = otherPrefab;
        }
        else {
            playerB = playerInput.gameObject;



















        }

    }
    private void OnPlayerLeft(PlayerInput playerInput)
    {

    }
    void JoinAction(InputAction.CallbackContext context)

    {
        PlayerInputManager.instance.JoinPlayerFromActionIfNotAlreadyJoined(context);
    }

    /*
    public GameObject[] spawnpoints;
    public List<PlayerInput> playerList = new List<PlayerInput>();
    [SerializeField] InputAction joinAction;
    [SerializeField] InputAction leaveAction;
    public PlayerInputManager inputManager;

    private int intIndex=0;

    //INSTANCES
    public static GameManager instance = null;

    //EVENTS
    public event System.Action<PlayerInput> PlayerJoinedGame;

    public event System.Action<PlayerInput> PlayerLeftGame;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        //if there is already anothergame bject in the world destroy it
        else if (instance != null)
        {
            //Destroy(gameObject);
            instance = this;
        }

        spawnpoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        joinAction.Enable();
        joinAction.performed += context => JoinAction(context);
        leaveAction.Enable();

    }
    private void Start()
    {
        PlayerInputManager.instance.JoinPlayer(0, -1, null);
       // PlayerInputManager.instance.JoinPlayer(1, -1, null);
    }
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("playerJoined the game");
        playerList.Add(playerInput);
        Debug.Log(playerList.Count);

     if (PlayerJoinedGame != null)
      {
            PlayerJoinedGame(playerInput);
       }

    }
    private void OnPlayerLeft(PlayerInput playerInput)
    {

    }
    void JoinAction(InputAction.CallbackContext context)

    {
        PlayerInputManager.instance.JoinPlayerFromActionIfNotAlreadyJoined(context);
    }*/
}