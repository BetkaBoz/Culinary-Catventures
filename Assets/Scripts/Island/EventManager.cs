using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class EventManager : MonoBehaviour
{
    [SerializeField] private bool isChallenge;
    [SerializeField] private EventType eventType;
    [SerializeField] private RandomEventType randomEventType; //FOR NOW
    [SerializeField] private TextMeshProUGUI  btnPrompt;
    [SerializeField] private int timeCost = 1;
    
    [SerializeField] private Sprite spriteRandom;
    [SerializeField] private Sprite spriteMerchant;
    [SerializeField] private Sprite spriteHarvest;
    [SerializeField] private Sprite spriteChallenge;
    
    //EVENT WINDOW
    [SerializeField] private GameObject  eventWindow;

    private IslandManager islandManager;
    
    private bool isOnEvent;
    private bool isUsed;
    private bool canShowEventWindow = true;
    //private GameObject playerGrabber;

    
    private void Awake()
    {
        
        eventWindow = GameObject.FindGameObjectWithTag("EventWindow");
    }

    void Start()
    {
        //spriteEventImg = eventWindow.GetComponentInChildren()

        AssignRandomType();
        AssignSprite();
        eventWindow.GetComponent<EventWindowControl>().HideWindow();
        

        islandManager = FindObjectOfType<IslandManager>();
    }

    
    //Urči typ eventu, Challenge sa nastavuje z inšpektora
    private void AssignRandomType()
    {   
        if (isChallenge) eventType = EventType.Challenge;
        else {
            Array values = Enum.GetValues(typeof(EventType));
            Random random = new Random(Guid.NewGuid().GetHashCode());
            //TODO: ZMENIT -3 NA -1 PO TESTOVANI RANDOM EVENTOV
            eventType = (EventType) values.GetValue(random.Next(values.Length - 3));
        }
    }
    //Urči náhodný typ  Random eventu
    private void AssignRandomEventRandomType()
    {
        Array rvalues = Enum.GetValues(typeof(RandomEventType));
        Random random = new Random(Guid.NewGuid().GetHashCode());
        randomEventType = (RandomEventType) rvalues.GetValue(random.Next(rvalues.Length ));
    }

    private void SwitchEventWindow()
    {
        EventWindowControl eventWindowControl = eventWindow.GetComponent<EventWindowControl>();
        switch (randomEventType)
        {
            case RandomEventType.HomelessCat:
                Debug.Log("HomelessCat");
                eventWindowControl.SetUpEventWindow("Homeless cat","You found homeless cat on the street.",
                    "POMOC","","");


                
                break;
            case RandomEventType.DiceCat:
                Debug.Log("DiceCat");
                eventWindowControl.SetUpEventWindow("Dice Cat","You see a cat playing dice.",
                    "POMOC","","");

  
                
                break;
            case RandomEventType.Stumble:
                Debug.Log("Stumble");
                eventWindowControl.SetUpEventWindow("Stumble","You stumbled on a small rock and lost an ingredient."
                ,"POMOC","","");

                
                
                break;
            case RandomEventType.PerfectTomatoes:
                Debug.Log("PerfectTomatoes");
                eventWindowControl.SetUpEventWindow("Perfect tomatoes","You see perfect tomatoes behind a fence.",
                    "POMOC","","");
                
                break;
            case RandomEventType.Cave:
                Debug.Log("Cave");
                eventWindowControl.SetUpEventWindow("Cave","You see entrance to a cave.",
                    "POMOCPLS","","LEAVE");
                break;
            case RandomEventType.StuckMerchant:
                Debug.Log("StuckMerchant");
                eventWindowControl.SetUpEventWindow("Stuck Merchant","You see a stucked merchant on the road.",
                    "POMOC","","LEAVE");
                break;
            case RandomEventType.Thieves:
                Debug.Log("Thieves");
                eventWindowControl.SetUpEventWindow("Thieves","You see thieves trying to rob you.",
                    "POMOC","","LEAVE");
                break;
            default:
                Debug.Log("What are you doing here CRIMINAL SCUM?");
                break;
        }
    }

    
    
    
    //Priradí sprite podĺa typu eventu
    private void AssignSprite()
    {
        Image imageComponent = GetComponent<Image>();
        
        switch (eventType)
        {
            case EventType.Merchant:
                //Debug.Log("MERCHANT");
                imageComponent.sprite  =  spriteMerchant;
                break;
            case EventType.Random:
                //Debug.Log("RANDOM EVENT");
                imageComponent.sprite =  spriteRandom;

                AssignRandomEventRandomType();
                
                
                
                break;
            case EventType.Harvest:
                //Debug.Log("HARVEST");
                imageComponent.sprite =  spriteHarvest;
                break;
            case EventType.Challenge:
                //Debug.Log("CHALLENGE");
                imageComponent.sprite =  spriteChallenge;
                break;
            default:
                Debug.Log("What are you doing here?");
                break;
        }
    }
    
    void Update()
    {
        //spustenie eventu
        if (isOnEvent && islandManager.Time > 0 && !isUsed && Input.GetButtonDown("Jump"))
        {
            Time.timeScale = 0;
            isUsed = true;
            ClearBtnPrompt();
            RecognizeAndRunEvent();
            
           
        }
    }
    
    //Spustenie eventu na základe typu
    private void RecognizeAndRunEvent()
    {
        switch (eventType)
        {
            case EventType.Merchant:
                Debug.Log("MERCHANT");
                break;
            case EventType.Random:
                Debug.Log("RANDOM EVENT");
                SwitchEventWindow();

                if (canShowEventWindow)
                {
                    eventWindow.GetComponent<EventWindowControl>().ShowWindow();
                    eventWindow.GetComponent<EventWindowControl>().Init(timeCost);
                }

                break;
            case EventType.Harvest:
                Debug.Log("HARVEST");
                break;
            case EventType.Challenge:
                Debug.Log("CHALLENGE");
                canShowEventWindow = false;
                islandManager.StartBattle();
                break;
            default:
                Debug.Log("What are you doing here?");
                break;
        }
    }

    //Nabehnutie hráča na event
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(!isUsed && islandManager.Time > 0) btnPrompt.text = "Press SPACE to do stuff";
        isOnEvent = true;
    }
    //Odídenie hráča z eventu
    private void OnTriggerExit2D(Collider2D other)
    {
        ClearBtnPrompt();
        isOnEvent = false;
    }
    
    private void ClearBtnPrompt()
    {
        btnPrompt.text = "";
    }

    private enum EventType{
        Random = 1,
        Harvest = 2,
        Merchant = 3,
        //Sensei = 4,
        
        Challenge = 10
    }

    private enum RandomEventType
    {
        HomelessCat = 1,
        DiceCat = 2,
        Stumble = 3,
        PerfectTomatoes = 4,
        Cave = 5,
        StuckMerchant = 6,
        Thieves = 7,
    }
    
    
    
    
    
}