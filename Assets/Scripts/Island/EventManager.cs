using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class EventManager : MonoBehaviour
{
    [SerializeField] private bool isChallenge = false;
    [SerializeField] private EventType eventType;
    [SerializeField] private TextMeshProUGUI  btnPrompt;
    [SerializeField] private int timeCost = 1;
    
    [SerializeField] private Sprite spriteRandom;
    [SerializeField] private Sprite spriteMerchant;
    [SerializeField] private Sprite spriteHarvest;
    [SerializeField] private Sprite spriteChallenge;
    
    //EVENT WINDOW
    [SerializeField] private GameObject  eventWindow;

    [SerializeField] private Sprite spriteEventImg;
    [SerializeField] private TextMeshProUGUI  eventWindowName;
    [SerializeField] private TextMeshProUGUI  eventWindowText;
    [SerializeField] private TextMeshProUGUI eventFirstButtontext;
    [SerializeField] private TextMeshProUGUI eventSecondButtontext;
    [SerializeField] private TextMeshProUGUI eventThirdButtontext;

    private IslandManager islandManager;
    
    private bool isOnEvent = false;
    private bool isUsed = false;
    private bool canShowEventWindow = true;
    //private GameObject playerGrabber;

    private void Awake()
    {
        eventWindow = GameObject.FindGameObjectsWithTag("EventWindow")[0];
    }

    void Start()
    {
        //spriteEventImg = eventWindow.GetComponentInChildren()

        AssignType();
        AssignSprite();
        eventWindow.SetActive(false);
        islandManager = FindObjectOfType<IslandManager>();
    }
    
    //Určí typ eventu, Challenge sa nastavuje z inšpektora
    private void AssignType()
    {
        if (isChallenge) eventType = EventType.Challenge;
        else AssignRandomType();
    }
    
    //Urči náhodne typ eventu
    private void AssignRandomType()
    {
        Array values = Enum.GetValues(typeof(EventType));
        Random random = new Random(Guid.NewGuid().GetHashCode());
        eventType = (EventType) values.GetValue(random.Next(values.Length - 1));
    }
    
    //Priradí sprite podĺa typu eventu
    private void AssignSprite()
    {
        Image imageComponent = GetComponent<Image>();
        

        switch (eventType)
        {
            case EventType.Merchant:
                Debug.Log("MERCHANT");
                imageComponent.sprite  =  spriteMerchant;
                break;
            case EventType.Random:
                Debug.Log("RANDOM EVENT");
                imageComponent.sprite =  spriteRandom;

                break;
            case EventType.Harvest:
                Debug.Log("HARVEST");
                imageComponent.sprite =  spriteHarvest;

                break;
            case EventType.Challenge:
                Debug.Log("CHALLENGE");
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

            if (canShowEventWindow)
            {
                eventWindow.GetComponent<EventWindowControl>().Init(timeCost);
                eventWindow.SetActive(true);   
            }
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

    enum EventType : int
    {
        Random = 1,
        Harvest = 2,
        Merchant = 3,
        //Sensei = 4,
        
        Challenge = 10
    }
}