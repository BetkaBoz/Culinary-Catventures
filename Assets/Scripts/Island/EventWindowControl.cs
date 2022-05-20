using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventWindowControl : MonoBehaviour
{
    private IslandManager islandManager;
    
    private Text evtText;

    private Button fstBtn;

    private Button scdBtn;

    private Button trdBtn;

    private int timeCost = 1;

    //[SerializeField] private GameObject  eventWindow;
/*
    [SerializeField] private Sprite spriteEventImg;
    [SerializeField] private TextMeshProUGUI  eventWindowName;
    [SerializeField] private TextMeshProUGUI  eventWindowText;
    [SerializeField] private TextMeshProUGUI eventFirstButtonText;
    [SerializeField] private TextMeshProUGUI eventSecondButtonText;
    [SerializeField] private TextMeshProUGUI eventThirdButtonText;
*/

    [SerializeField] private GameObject  WindowName;
    [SerializeField] private GameObject  WindowText;
    [SerializeField] private GameObject  WindowImage;
    [SerializeField] private GameObject  FirstButton;
    [SerializeField] private GameObject  SecondButton;
    [SerializeField] private GameObject  ThirdButton;

    
    private void Awake()
    {
        //Zbytočné, je to v prefabe
        //Prehľadá všetky child elementy a priradí ich do premenných
        /*
        foreach (Transform child in transform)
        {
            switch (child.name)
            {
                case "EventName":
                    WindowName = child.gameObject;
                    break;
                case "EventText":
                    WindowText = child.gameObject;
                    break;
                case "EventImage":
                    WindowImage = child.gameObject;
                    break;
                case "FirstButton":
                    FirstButton = child.gameObject;
                    break;
                case "SecondButton":
                    SecondButton = child.gameObject;
                    break;
                case "ThirdButton":
                    ThirdButton = child.gameObject;
                    break;
                default:
                    Debug.Log("What are you doing here?");
                    
                    break;
            }
            //Debug.Log(child.name);

        }
       */
    }

    void Start()
    {
        
        
        //fstBtn = transform.Find("FirstButton").GetComponent<Button>();
        //scdBtn = transform.Find("SecondButton").GetComponent<Button>();
        //trdBtn = transform.Find("ThirdButton").GetComponent<Button>();

        islandManager = FindObjectOfType<IslandManager>();
        
        // Takto vyzera pridavanie funkcii buttonom zo scriptu
        //  bude sa hodit, ak sa budu funkcie na buttonoch menit za behu hry, napriklad v reakcii na hracov input
        //fstBtn.onClick.AddListener(delegate { CloseEvent(); });
        //scdBtn.onClick.AddListener(delegate { CloseEvent(); });
        //trdBtn.onClick.AddListener(delegate { CloseEvent(); });

        
        FirstButton.GetComponent<Button>().onClick.AddListener(delegate { CloseEvent(); });
        SecondButton.GetComponent<Button>().onClick.AddListener(delegate { CloseEvent(); });
        ThirdButton.GetComponent<Button>().onClick.AddListener(delegate { CloseEvent(); });

         
    }


    void Update()
    {
        
    }

    
    public void Init(int timeCost)
    {
        Debug.Log("TimeCost initialized to " + timeCost);
        this.timeCost = timeCost;
    }

    private void CloseEvent()
    {
        Time.timeScale = 1;
        islandManager.lowerTime(timeCost);
        GameObject.FindGameObjectsWithTag("EventWindow")[0].SetActive(false);
    }

    public void SetWindowName(string text)
    {
        WindowName.GetComponent<TMP_Text>().text = text;
    }
    public void SetWindowText(string text)
    {
        WindowText.GetComponent<TMP_Text>().text = text;
    }
    public void SetFirstButtonText(string text)
    {
        FirstButton.GetComponentInChildren<TMP_Text>().text = text;
    }
    public void SetSecondButtonText(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            SecondButton.SetActive(false);
        }
        else
        {
            SecondButton.SetActive(true);
            SecondButton.GetComponentInChildren<TMP_Text>().text = text;
        }
    }
    public void SetThirdButtonText(string text)
    {
        ThirdButton.GetComponentInChildren<TMP_Text>().text = text;
    }
    
    public void ShowWindow()
    {
        gameObject.SetActive(true);
        //transform.position += Vector3.forward * 2000;
    }
    public void HideWindow()
    {
        gameObject.SetActive(false);
        //transform.position -= Vector3.forward * 2000;
    }
    
    public void SetUpEventWindow(string name,string text,string firstBtn = "",string secondtBtn = "",string thirdtBtn = "")
    {
        //EventWindowControl eventWindowControl = eventWindow.GetComponent<EventWindowControl>();
        SetWindowName(name);
        SetWindowText(text);
        SetFirstButtonText(firstBtn);
        SetSecondButtonText(secondtBtn);
        SetThirdButtonText(thirdtBtn);
    }
    
}
