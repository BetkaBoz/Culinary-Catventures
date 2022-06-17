using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class EventManager : MonoBehaviour
{
    #region Private Vars
    [SerializeField] private bool isChallenge;
    [SerializeField] private EventType eventType;
    [SerializeField] private RandomEventType randomEventType; //FOR NOW
    [SerializeField] private TextMeshProUGUI  btnPrompt; //FOR NOW
    //[SerializeField] private int timeCost = 1;
    
    //EVENT SPRITES
    [SerializeField] private Sprite spriteRandom;
    [SerializeField] private Sprite spriteMerchant;
    [SerializeField] private Sprite spriteHarvest;
    [SerializeField] private Sprite spriteChallenge;
    
    //EVENT WINDOW
    private EventWindowControl  eventWindow;
    //UILAYER
    private UILayer  uiLayer;
    // MERCHARNT WINDOW
    private MerchantWindowControl  merchantWindow;
    //ISLAND MANAGER
    private IslandManager islandManager;
    
    private bool isOnEvent;
    private bool isUsed;
    //private bool canShowEventWindow = true;

    

    #endregion
    
    private void Awake()
    {   
        eventWindow = FindObjectOfType<EventWindowControl>();
        //merchantWindow = GameObject.FindGameObjectsWithTag("EventWindow")[1].GetComponent<MerchantWindowControl>();
        merchantWindow = FindObjectOfType<MerchantWindowControl>();
        //uiLayer = GameObject.FindGameObjectWithTag("UILayer").GetComponent<UILayer>();
        uiLayer = FindObjectOfType<UILayer>();
        uiLayer.UpdateUI();
        islandManager = FindObjectOfType<IslandManager>();
    }

    void Start()
    {
        AssignRandomType();
        AssignSprite();
    }

    void Update()
    {

        //SPUSTENIE EVENTU AK HO AKTIVUJEME
        ActivateEvent();
        
    }
    //URČI TYP EVENTU, CHALLENGE SA NASTAVUJE Z INŠPEKTORA
    private void AssignRandomType()
    {   
        if (isChallenge) eventType = EventType.Challenge;
        else {
            Array values = Enum.GetValues(typeof(EventType));
            Random random = new Random(Guid.NewGuid().GetHashCode());
            //TODO: ZMENIT  NA -1 PO TESTOVANI RANDOM EVENTOV
            eventType = (EventType) values.GetValue(random.Next(values.Length - 1));
        }
    }
    //URČI NÁHODNÝ TYP RANDOM EVENTU
    private void AssignRandomEventRandomType()
    {
        Array rvalues = Enum.GetValues(typeof(RandomEventType));
        Random random = new Random(Guid.NewGuid().GetHashCode());
        randomEventType = (RandomEventType) rvalues.GetValue(random.Next(rvalues.Length ));
    }
   

    
    //PRIRADÍ SPRITE PODĹA TYPU EVENTU
    private void AssignSprite()
    {
        Image imageComponent = GetComponent<Image>();
        
        switch (eventType)
        {
            case EventType.Random:
                //Debug.Log("RANDOM EVENT");
                imageComponent.sprite =  spriteRandom;

                AssignRandomEventRandomType();

                break;
            case EventType.Gather:
                //Debug.Log("HARVEST");
                imageComponent.sprite =  spriteHarvest;
                break;
            case EventType.Merchant:
                //Debug.Log("MERCHANT");
                imageComponent.sprite  =  spriteMerchant;
                break;
            
            
            case EventType.Challenge:
                //Debug.Log("CHALLENGE");
                imageComponent.sprite =  spriteChallenge;
                break;
            default:
                Debug.Log("What are you doing here CRIMINAL SCUM?");
                break;
        }
    }
    //SPUSTENIE EVENTU NA ZÁKLADE TYPU
    private void RecognizeAndRunEvent()
    {
        //EventWindowControl eventWindowControl = eventWindow.GetComponent<EventWindowControl>();

        switch (eventType)
        {
            case EventType.Merchant:
                //Debug.Log("MERCHANT");
                merchantWindow.StartWindow();
                break;
            case EventType.Random:
                //Debug.Log("RANDOM EVENT");
                eventWindow.StartWindow(randomEventType);
                /*
                if (canShowEventWindow)
                {
                    eventWindowControl.ShowWindow();
                    eventWindowControl.Init(timeCost);
                }
*/
                break;
            case EventType.Gather:
                eventWindow.Gather();
                
                //Debug.Log("HARVEST");
                break;
            case EventType.Challenge:
                //Debug.Log("CHALLENGE");
                //canShowEventWindow = false;
                islandManager.StartBattle();
                break;
            default:
                Debug.Log("What are you doing here?");
                break;
        }
    }
    //NABEHNUTIE HRÁČA NA EVENT
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
    
    private void ClearBtnPrompt()
    {
        btnPrompt.text = "";
    }
    
    
    
    private enum EventType{
        Random = 1,
        Gather = 2,
        Merchant = 3,
        //Sensei = 4,
        
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
        
        
    }
}