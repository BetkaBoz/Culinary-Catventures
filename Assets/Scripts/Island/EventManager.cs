using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


public class EventManager : MonoBehaviour
{
    #region Private Vars
    //[SerializeField] private bool isChallenge;
    //[SerializeField] private EventType eventType;
    //[SerializeField] private RandomEventType randomEventType; 
    [SerializeField] public TextMeshProUGUI  btnPrompt; //FOR NOW
    //[SerializeField] private int timeCost = 1;
    
    //EVENT SPRITES
    [SerializeField] public Sprite spriteRandom;
    [SerializeField] public Sprite spriteMerchant;
    [SerializeField] public Sprite spriteHarvest;
    [SerializeField] public Sprite spriteChallenge;
    [SerializeField] public Sprite spriteSensei;

    [SerializeField] public List<Event> allEvents;
    //EVENT WINDOW
    private EventWindowControl  eventWindow;
    // MERCHARNT WINDOW
    private MerchantWindowControl  merchantWindow;
    // SENSEI WINDOW
    private SenseiWindowControl  senseiWindow;
    
    
    //ISLAND MANAGER
    public IslandManager islandManager;
    
    //private bool isOnEvent;
    
    public int merchantCount ;
    public int senseiCount;
    public int gatherCount;

    //CONSTANTS
    public const int MaxMerchantCount = 1;
    public const int MaxSenseiCount = 1;
    public const int MaxGatherCount = 2;


    //private bool canShowEventWindow = true;

    

    #endregion
    
    private void Awake()
    {   
        eventWindow = FindObjectOfType<EventWindowControl>();
        merchantWindow = FindObjectOfType<MerchantWindowControl>();
        senseiWindow = FindObjectOfType<SenseiWindowControl>();
        
        islandManager = FindObjectOfType<IslandManager>();

    }

    void Start()
    {
        AssignRandomTypes();
        ClearBtnPrompt();
        //AssignRandomType();
        //AssignSprite();
    }


    //NACITANIE VSETKYCH EVENTOV DO LISTU PODLA TAGU
    private void GetAllEvents()
    {
        GameObject[] tmpEvents = GameObject.FindGameObjectsWithTag("Event");
        foreach (GameObject eventik in tmpEvents)
        {
            Event @event = eventik.GetComponent<Event>();
            allEvents.Add(@event);
        }
    }
    //VYBERA NAHODNY EVENT NA MAPE A PRIDAVAMU TYP A SPRITE
    private void AssignRandomTypes()
    {
        int iterations = allEvents.Count;
        for (int i = 0; i < iterations; i++)
        {
            //Debug.Log("TOLKOTO KRAT: "+ i);
            //Debug.Log("VELKOST: "+ allEvents.Count);
            
            int random = Random.Range(0, allEvents.Count);
            Event @event = allEvents[random];
            @event.AssignRandomType(); 
            @event.AssignSprite();

            allEvents.RemoveAt(random);
        }
        GetAllEvents();
    }
    //SPUSTENIE EVENTU PODLA TYPU
    public void RecognizeAndRunEvent(Event eventt)
    {
        switch (eventt.eventType)
        {
            case Event.EventType.Merchant:
                //Debug.Log("MERCHANT");
                merchantWindow.StartWindow();
                break;
            case Event.EventType.Random:
                //Debug.Log("RANDOM EVENT");
                eventWindow.StartWindow(eventt.randomEventType);
                
                break;
            case Event.EventType.Gather:
                eventWindow.Gather();
                
                break;
            case Event.EventType.Sensei:
                senseiWindow.StartWindow();
                
                break;
            case Event.EventType.Challenge:
                //Debug.Log("CHALLENGE");
                //canShowEventWindow = false;
                islandManager.StartBattle();
                break;
            default:
                Debug.Log("What are you doing here?");
                break;
        }
    }
    
    public void ClearBtnPrompt()
    {
        btnPrompt.text = "";
    }

    #region USELESS CODE
            //URČI TYP EVENTU, CHALLENGE SA NASTAVUJE Z INŠPEKTORA
    /*
    private void AssignRandomType()
    {
        foreach (Event oneEvent in allEvents)
        {
            if (isChallenge) eventType = EventType.Challenge;
            else if(merchantCount < MaxMerchantCount )
            {
                merchantCount++;
                eventType = EventType.Merchant;
            }
            else if(senseiCount < MaxSenseiCount )
            {
                senseiCount++;
                eventType = EventType.Sensei;
            }
            else {
                Array values = Enum.GetValues(typeof(EventType));
                Random random = new Random(Guid.NewGuid().GetHashCode());
                //TODO: ZMENIT  NA -1 PO TESTOVANI RANDOM EVENTOV
                eventType = (EventType) values.GetValue(random.Next(values.Length - 3));
            }
        }
    }*/
    //URČI NÁHODNÝ TYP RANDOM EVENTU
    /*
    private void AssignRandomEventRandomType()
    {
        Array rvalues = Enum.GetValues(typeof(RandomEventType));
        Random random = new Random(Guid.NewGuid().GetHashCode());
        randomEventType = (RandomEventType) rvalues.GetValue(random.Next(rvalues.Length ));
    }
   */

    
    //PRIRADÍ SPRITE PODĹA TYPU EVENTU
    /*
    private void AssignSprite()
    {
        Image imageComponent = GetComponent<Image>();
        
        switch (eventType)
        {
            case EventType.Random:
                imageComponent.sprite =  spriteRandom;

                AssignRandomEventRandomType();
                break;
            case EventType.Gather:
                imageComponent.sprite =  spriteHarvest;
                break;
            case EventType.Merchant:
                imageComponent.sprite  =  spriteMerchant;
                break;
            case EventType.Sensei:
                imageComponent.sprite  =  spriteSensei;
                break;
            
            case EventType.Challenge:
                imageComponent.sprite =  spriteChallenge;
                break;
            default:
                Debug.Log("What are you doing here CRIMINAL SCUM?");
                break;
        }
    }*/
    
    //SPUSTENIE EVENTU NA ZÁKLADE TYPU

    //NABEHNUTIE HRÁČA NA EVENT
    /*
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(!isUsed && islandManager.Time > 0) btnPrompt.text = "Press SPACE to do stuff";
        isOnEvent = true;
        ChangeEventScale();

        
    }
    //ODÍDENIE HRÁČA Z EVENTU
    private void OnTriggerExit2D(Collider2D other)
    {
        ClearBtnPrompt();
        isOnEvent = false;
        ChangeEventScale();

    }
    */
    
    
    
    /*
    public enum EventType{
        Random = 1,
        Gather = 2,
        Merchant = 3,
        Sensei = 4,
        
        Challenge = 10
    }

    public enum RandomEventType
    {
        HomelessCat = 1,
        DiceCat = 2,
        Stumble = 3,
        PerfectTomatoes = 4,
        Cave = 5,
        StuckMerchant = 6,
        Thieves = 7,
    }
*/
    /*
    private void ChangeEventScale( )
    {
        //CircleCollider2D circleCollider2D = GetComponent<CircleCollider2D>();
        //circleCollider2D.enabled = false;
        if (!isUsed)
        {
            if (isOnEvent)
            {
                transform.localScale = new Vector3(1.5f, 1.5f, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
*/
/*
    private void ActivateEvent()
    {
        if (isOnEvent && islandManager.Time > 0 && !isUsed && Input.GetButtonDown("Jump"))
        {
            Time.timeScale = 0;
            isUsed = true;
            ClearBtnPrompt();
            RecognizeAndRunEvent();
            GetComponent<Image>().color = new Color32(125,125,125,255);
            transform.localScale = new Vector3(1, 1, 1);
        }
    }*/
    

    #endregion

}