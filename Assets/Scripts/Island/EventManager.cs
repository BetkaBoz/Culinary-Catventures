using System;
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
    void Start()
    {
       

        //spriteEventImg = eventWindow.GetComponentInChildren()

        if (isChallenge) this.eventType = EventType.Challenge;
        else assignRandomType();
        assignSprite();
        islandManager = FindObjectOfType<IslandManager>();
    }

    public void assignRandomType()
    {
        Array values = Enum.GetValues(typeof(EventType));
        Random random = new Random(Guid.NewGuid().GetHashCode());
        this.eventType = (EventType) values.GetValue(random.Next(values.Length - 1));
    }


    public void assignSprite()
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
            SwitchEvent();
            islandManager.lowerTime(timeCost);
        }
    }
    
    
    private void SwitchEvent()
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