using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class WinState : MonoBehaviour
{
     public TMP_Text WinText;
    public TMP_Text WinText2;
    public TMP_Text WinText3;
    private bool player1Arrived =false;
    private bool player2Arrived=true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        if (player1Arrived && player2Arrived)
        {
            WinTheGame();
        }

    }

    private void OnCollisionStay(Collision collision)
    {

        if (collision.gameObject.tag == "Player1")
        {
            player1Arrived = true;
        }
        if (collision.gameObject.tag == "Player2")
        {
            player2Arrived = true;
        }


    }

    public void WinTheGame()
    {
        WinText.SetText("Good Job! Head back to the survey!");
        WinText2.SetText("Good Job! Head back to the survey!");
        WinText3.SetText("Good Job! Head back to the survey!");
        //Application.Quit();

    }
}
