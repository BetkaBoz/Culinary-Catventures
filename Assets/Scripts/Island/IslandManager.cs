using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IslandManager : MonoBehaviour
{
    [SerializeField] private int time;
    [SerializeField] TextMeshProUGUI  timeText;
    [SerializeField] private GameObject grabberPrefab;

    public int Time {get => time;}
    
    void Start()
    {
        timeText.text = "Time: " + time;
    }
    
    //Znižovanie času o lowerBy
    public void lowerTime(int lowerBy)
    {
        time -= lowerBy;
        
        //Ak vyprší tak spawni ruku
        if (time <= 0)
        {
            time = 0;
            Instantiate(grabberPrefab);
        }
        timeText.text = "Time: " + time;
    }
    
    //Spusti scénu boja
    public void StartBattle()
    {
        SceneManager.LoadScene("Battle", LoadSceneMode.Single);
    }
}