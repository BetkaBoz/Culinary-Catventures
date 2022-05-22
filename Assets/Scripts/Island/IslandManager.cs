using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IslandManager : MonoBehaviour
{
    [SerializeField] private int time;
    [SerializeField] private TextMeshProUGUI  timeText;
    [SerializeField] private GameObject grabberPrefab;
    //[SerializeField] private TextMeshProUGUI  coinText;
    //[SerializeField] private TextMeshProUGUI  repText; IN UILayer


    
    
    
    public int Time => time;

    void Start()
    {
        timeText.text = "Time: " + time;
    }
    
    //Znižovanie času o lowerBy
    public void LowerTime(int lowerBy)
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
        //Uloženie hodnôt do PLAYERPREFS
        //NOT GOOD
        //{PlayerPrefs.SetInt("reputation", 100);}
        
        SceneManager.LoadScene("Battle", LoadSceneMode.Single);
    }
}