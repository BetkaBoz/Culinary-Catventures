using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class EventManager : MonoBehaviour
{
    [SerializeField] bool isChallenge = false;
    [SerializeField] EventType eventType;
    [SerializeField] TextMeshProUGUI  BtnPrompt;
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

    private void Awake()
    {
        eventWindow = GameObject.FindGameObjectsWithTag("EventWindow")[0];
    }

    void Start()
    {
        //spriteEventImg = eventWindow.GetComponentInChildren()

        AssignType();
        islandManager = FindObjectOfType<IslandManager>();
        eventWindow.SetActive(false);
    }

    private void AssignType()
    {
        if (isChallenge) this.eventType = EventType.Challenge;
        else AssignRandomType();
        AssignSprite();
    }

    public void AssignRandomType()
    {
        Array values = Enum.GetValues(typeof(EventType));
        Random random = new Random(Guid.NewGuid().GetHashCode());
        this.eventType = (EventType) values.GetValue(random.Next(values.Length - 1));
    }
    
    public void AssignSprite()
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
        if (isOnEvent && !isUsed && Input.GetButtonDown("Jump"))
        {
            isUsed = true;
            ClearBtnPrompt();
            RecognizeAndRunEvent();
            islandManager.lowerTime(timeCost);
            
            eventWindow.SetActive(true);
        }
    }
    
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
                break;
            default:
                Debug.Log("What are you doing here?");
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(!isUsed) BtnPrompt.text = "Press SPACE to do stuff";
        isOnEvent = true;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        ClearBtnPrompt();
        isOnEvent = false;
    }
    
    private void ClearBtnPrompt()
    {
        BtnPrompt.text = "";
    }

    enum EventType : int
    {
        Merchant = 1,
        Random = 2,
        Harvest = 3,

        Challenge = 10
    }
}