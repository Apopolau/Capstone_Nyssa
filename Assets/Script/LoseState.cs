using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseState : MonoBehaviour

{

    //public GameObject LosePlatform;


    // Start is called before the first frame update
    void Start()
    {
         

}

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Player1" || collision.gameObject.tag == "Player2")
        {
            ResetTheGame();
        }
    }

    public void ResetTheGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        print("we are restarting, collision working");
    }
}
