using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void PlayGame()
    {
        //SceneManager.LoadScene("Game"); This is only for the purposes of the first build
        SceneManager.LoadScene("Battle"); //remove this in the next build
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit!");
    }
    
}
