using System;
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
    [SerializeField] private Light sun;
    [SerializeField] private GameObject playerLight;
    
    [SerializeField] private GameObject lights;

     public int Time => time;



    private void Awake()
    {
        timeText.text = "Time: " + time;
        sun  = GameObject.FindGameObjectWithTag("Light").GetComponent<Light>();
        playerLight  = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).gameObject;

        LightControl();
    }

    void Start()
    {
    }
    
    //ZNIŽOVANIE ČASU O LOWERBY
    public void LowerTime(int lowerBy)
    {
        time -= lowerBy;

        LightControl();
        //AK VYPRŠÍ TAK SPAWNI RUKU
        if (time <= 0)
        {
            time = 0;
            Instantiate(grabberPrefab);
        }
        timeText.text = "Time: " + time;
    }
    private void LightControl()
    {
        sun.intensity = time * 0.2f;

        if (sun.intensity <= 0.4)
        {
            lights.SetActive(true);
            playerLight.SetActive(true);
            
        }
        else
        {            
            lights.SetActive(false);
            playerLight.SetActive(false);

        }
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