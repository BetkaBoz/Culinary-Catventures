using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;


public class Event : MonoBehaviour
{
    #region Private Vars
    [SerializeField] public bool isChallenge;
    [SerializeField] public EventType eventType;
    [SerializeField] public RandomEventType randomEventType; //FOR NOW
    [SerializeField] public int timeCost = 1;
    [SerializeField] private Image imageComponent;
    //[SerializeField] private CircleCollider2D circleCollider2D;
    
    public bool isUsed;
    private bool isOnEvent;

    //EVENT MANAGER
    [SerializeField]private EventManager eventManager;
    //ISLAND MANAGER
    [SerializeField]private IslandManager islandManager;
    
    #endregion

    private void Awake()
    {
        imageComponent = GetComponentInChildren<Image>();

        eventManager = FindObjectOfType<EventManager>();
        islandManager = FindObjectOfType<IslandManager>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        //AssignRandomType();
        //AssignSprite();

    }

    // Update is called once per frame
    void Update()
    {
        //SPUSTENIE EVENTU AK HO AKTIVUJEME
        ActivateEvent();
    }
    
    //URČI TYP EVENTU, CHALLENGE SA NASTAVUJE Z INŠPEKTORA
    public void AssignRandomType()
    {
        if (isChallenge) eventType = EventType.Challenge;
            //JEDEN MERCHANT
            else if(eventManager.merchantCount < EventManager.MaxMerchantCount )
            {
                eventManager.merchantCount++;
                eventType = EventType.Merchant;
            }//JEDEN SENSEI
            else if(eventManager.senseiCount < EventManager.MaxSenseiCount )
            {
                eventManager.senseiCount++;
                eventType = EventType.Sensei;
            }
            else {
                Array values = Enum.GetValues(typeof(EventType));
                Random random = new Random(Guid.NewGuid().GetHashCode());
                //TODO: ZMENIT  NA -1 PO TESTOVANI RANDOM EVENTOV
                eventType = (EventType) values.GetValue(random.Next(values.Length - 3));
                //MAX 2 GATHERY
                if (eventType == EventType.Gather)
                {
                    eventManager.gatherCount++;
                    if (eventManager.gatherCount >EventManager.MaxGatherCount)
                    {
                        eventType = EventType.Random;
                    }
                }
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
    public void AssignSprite()
    {
        //Image imageComponent = GetComponent<Image>();

        switch (eventType)
        {
            case EventType.Random:
                imageComponent.sprite =  eventManager.spriteRandom;
                AssignRandomEventRandomType();
                break;
            case EventType.Gather:
                imageComponent.sprite =  eventManager.spriteHarvest;
                break;
            case EventType.Merchant:
                imageComponent.sprite  =  eventManager.spriteMerchant;
                break;
            case EventType.Sensei:
                imageComponent.sprite  =  eventManager.spriteSensei;
                break;
            
            case EventType.Challenge:
                imageComponent.sprite =  eventManager.spriteChallenge;
                break;
            default:
                Debug.Log("What are you doing here CRIMINAL SCUM?");
                break;
        }
    }
    
    //ZMENENIE VELKOSTI SPRITU EVENTU
    private void ChangeEventScale( )
    {
        //if (!isUsed)  return;
        if (isOnEvent)
        {
            imageComponent.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1);

        }
        else
        {
            imageComponent.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        
    }
    
    //NABEHNUTIE HRÁČA NA EVENT
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log(gameObject.name + " Collided");

        //if(!isUsed && islandManager.time > 0) eventManager.btnPrompt.text = "Press SPACE to do stuff";
        if (isUsed) return;
        eventManager.btnPrompt.text = "Press SPACE to do stuff"; 
        isOnEvent = true;
        ChangeEventScale();
    }
   
    //ODÍDENIE HRÁČA Z EVENTU
    private void OnTriggerExit2D(Collider2D col)
    {
        //Debug.Log(gameObject.name + " Leaved");
        eventManager.ClearBtnPrompt();
        isOnEvent = false;
        ChangeEventScale();
    }
    
    private void ActivateEvent()
    {
        if (isOnEvent && !isUsed && Input.GetButtonDown("Jump") && (islandManager.time > 0  || eventType == EventType.Challenge))
        {
            Time.timeScale = 0;
            isUsed = true;
            eventManager.ClearBtnPrompt();
            eventManager.RecognizeAndRunEvent(this);
            imageComponent.color = new Color32(125,125,125,255);
            imageComponent.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    
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
    
    
}
