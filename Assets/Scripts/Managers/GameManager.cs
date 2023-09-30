using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    public GameObject player1;
    public GameObject player2;
    

    public GameObject winScreen;

    public Text playerWin;

    
    public void EndGame()
    {

        Debug.Log("Fin de la partida");

        if (player1.GetComponent<PlayerController>().statusScript.isDead)
        {
            winScreen.SetActive(true);
            playerWin.text = "Player 2 wins";
        }
        else
        {
            winScreen.SetActive(true);
            playerWin.text = "Player 1 wins";
        }
    }


}
