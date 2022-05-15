using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventWindowControl : MonoBehaviour
{
    private Text evtText;

    private Button fstBtn;

    private Button scdBtn;

    private Button trdBtn;
    // Start is called before the first frame update
    void Start()
    {
        fstBtn = transform.Find("FirstButton").GetComponent<Button>();
        scdBtn = transform.Find("SecondButton").GetComponent<Button>();
        trdBtn = transform.Find("ThirdButton").GetComponent<Button>();
        
        // Takto vyzera pridavanie funkcii buttonom zo scriptu
        //  bude sa hodit, ak sa budu funkcie na buttonoch menit za behu hry, napriklad v reakcii na hracov input
        fstBtn.onClick.AddListener(delegate { CloseEvent(); });
        scdBtn.onClick.AddListener(delegate { CloseEvent(); });
        trdBtn.onClick.AddListener(delegate { CloseEvent(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CloseEvent()
    {
        Time.timeScale = 1;
        GameObject.FindGameObjectsWithTag("EventWindow")[0].SetActive(false);
    }
    
    
}
