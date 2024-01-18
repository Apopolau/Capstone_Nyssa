/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class CelestialGameManager : MonoBehaviour
{
    public GameObject[] spawnpoints;
    public List<PlayerInput> cplayerList = new List<PlayerInput>();
    [SerializeField] InputAction joinAction;
    [SerializeField] InputAction leaveAction;



    //INSTANCES
    public static CelestialGameManager instance = null;

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
          // Destroy(gameObject);
        }

        spawnpoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        joinAction.Enable();
        joinAction.performed += context => JoinAction(context);
        leaveAction.Enable();

    }
    private void Start()
    {
        PlayerInputManager.instance.JoinPlayer(2, -1, null);
    }
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("playerJoined the game");
        cplayerList.Add(playerInput);
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
    }
}*/