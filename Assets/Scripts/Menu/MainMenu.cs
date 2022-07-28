using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] List<CardBaseInfo> deck = new List<CardBaseInfo>();
    Player player = null;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void PlayGame()
    {
        //SceneManager.LoadScene("Game"); This is only for the purposes of the first build
        ResetPlayer();
        SceneManager.LoadScene("Battle"); //remove this in the next build
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit!");
    }

    private void ResetPlayer()
    {
        if (player != null)
        {
            player.rep = 100;
            player.Energy = 0;
            player.Money = 0;
            player.Deck = deck;
            player.isDead = false;
            player.isVictorious = false;
        }
    }

}
