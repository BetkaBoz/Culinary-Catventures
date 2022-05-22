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
    [SerializeField] private TextMeshProUGUI  btnPrompt; //FOR NOW
    [SerializeField] private int timeCost = 1;
    
    //EVENT SPRITES
    [SerializeField] private Sprite spriteRandom;
    [SerializeField] private Sprite spriteMerchant;
    [SerializeField] private Sprite spriteHarvest;
    [SerializeField] private Sprite spriteChallenge;
    
    //EVENT WINDOW
    [SerializeField] private GameObject  eventWindow;
    //UILAYER
    [SerializeField] private GameObject  uiLayer;

    private IslandManager islandManager;
    
    private bool isOnEvent;
    private bool isUsed;
    private bool canShowEventWindow = true;
    //private GameObject playerGrabber;

    private const int Minor = 5;
    private const int Moderate = 10;
    private const int Major = 20;

    private void Awake()
    {
        eventWindow = GameObject.FindGameObjectWithTag("EventWindow");
        uiLayer = GameObject.FindGameObjectWithTag("UILayer");
        uiLayer.GetComponent<UILayer>().UpdateUI();

        
        
    }

    void Start()
    {
        
        
        //spriteEventImg = eventWindow.GetComponentInChildren()

        AssignRandomType();
        AssignSprite();
        eventWindow.GetComponent<EventWindowControl>().HideWindow();
        

        islandManager = FindObjectOfType<IslandManager>();
    }

    void Update()
    {

        //Spustenie eventu ak ho spustime
        if (isOnEvent && islandManager.Time > 0 && !isUsed && Input.GetButtonDown("Jump"))
        {
            Time.timeScale = 0;
            isUsed = true;
            ClearBtnPrompt();
            RecognizeAndRunEvent();
            
           
        }
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
    //Zmení Event okno podľa typu random Eventu
    
    private void SwitchEventWindow()
    {   
        EventWindowControl eventWindowControl = eventWindow.GetComponent<EventWindowControl>();
        UILayer uiLayerControl = uiLayer.GetComponent<UILayer>();
        Button firstButtonControl = eventWindowControl.FirstButton.GetComponent<Button>();
        Button secondButtonControl = eventWindowControl.SecondButton.GetComponent<Button>();
        Button thirdButtonControl = eventWindowControl.ThirdButton.GetComponent<Button>();

        
        switch (randomEventType)
        {
            case RandomEventType.HomelessCat:
                Debug.Log("HomelessCat");
                eventWindowControl.SetUpEventWindow("Homeless cat","You found homeless cat on the street.",
                    "HELP","ROB","LEAVE");
                
                firstButtonControl.onClick.AddListener(delegate {
                    uiLayerControl.ChangeMoney(-Moderate);
                    uiLayerControl.ChangeReputation(Major);

                });
                secondButtonControl.onClick.AddListener(delegate {
                    uiLayerControl.ChangeMoney(Moderate);
                    uiLayerControl.ChangeReputation(-Major);
                });
                thirdButtonControl.onClick.AddListener(delegate {
                    Debug.Log("LEAVE");
                });
                break;
            case RandomEventType.DiceCat:
                Debug.Log("DiceCat");
                eventWindowControl.SetUpEventWindow("Dice Cat","You see a cat playing dice.",
                    "PLAY","","DECLINE");
                
                firstButtonControl.onClick.AddListener(delegate {
                    if (RandomState())
                    {
                        //PREHRAL
                        uiLayerControl.ChangeMoney(-Moderate);
                        uiLayerControl.ChangeReputation(Minor);
                    }
                    else
                    {
                        //VYHRAL
                        uiLayerControl.ChangeMoney(Moderate);
                        uiLayerControl.ChangeReputation(Minor);
                    }
                });
                thirdButtonControl.onClick.AddListener(delegate {
                    uiLayerControl.ChangeReputation(-Minor);
                });
                break;
            case RandomEventType.Stumble:
                Debug.Log("Stumble");
                eventWindowControl.SetUpEventWindow("Stumble","You stumbled on a small rock and lost an ingredient."
                ,"ASK FOR HELP","","LEAVE");

                firstButtonControl.onClick.AddListener(delegate {
                    //TODO: MOZE KRICAT O POMOC VIACEJ KRAT
                    uiLayerControl.ChangeReputation(-Minor);
                });
                thirdButtonControl.onClick.AddListener(delegate {
                    //TODO: STRATIT NAHODNU INGREDIENCIU Z DECKU
                    Debug.Log("LEAVE");
                });
                
                break;
            case RandomEventType.PerfectTomatoes:
                Debug.Log("PerfectTomatoes");
                eventWindowControl.SetUpEventWindow("Perfect tomatoes","You see perfect tomatoes behind a fence.",
                    "CLIMB","","LEAVE");
                
                firstButtonControl.onClick.AddListener(delegate {
                    if (RandomState(30))
                    {
                        //PADOL A VSIMLI SI HO
                        uiLayerControl.ChangeReputation(-Major);
                    }
                    else
                    {
                        //TODO: PRESKOCIL A UKRADOL 2 RAJCINY :O
                        Debug.Log("GIMME DAT GRAPES");
                    }
                });
                thirdButtonControl.onClick.AddListener(delegate {
                    Debug.Log("LEAVE");
                });
                break;
            case RandomEventType.Cave:
                Debug.Log("Cave");
                eventWindowControl.SetUpEventWindow("Cave","You see entrance to a cave.",
                    "GO IN","","LEAVE");
                
                firstButtonControl.onClick.AddListener(delegate {
                    if (RandomState(99))
                    {
                        if (RandomState())
                        {
                            //NASIEL PENIAZGY
                            uiLayerControl.ChangeMoney(Moderate);
                        }
                        else
                        {
                            //STRATIL PENIAZGY
                            uiLayerControl.ChangeMoney(-Moderate);
                        }
                    }
                    else
                    {
                        //NASIEL RARE HELPERA: WITCH
                        Debug.Log("YOU FOUND WITCH!");
                    }
                });
                thirdButtonControl.onClick.AddListener(delegate {
                    Debug.Log("LEAVE");
                });
                break;
            case RandomEventType.StuckMerchant:
                Debug.Log("StuckMerchant");
                eventWindowControl.SetUpEventWindow("Stuck Merchant","You see a stuck merchant on the road.",
                    "HELP","","LEAVE");
                
                firstButtonControl.onClick.AddListener(delegate {
                    //20%
                    if (RandomState(100))
                    {                        //AMBUSH
                        eventWindowControl.SetUpEventWindow("It's a trap!","You see robbers coming to you.",
                            "FIGHT","","RUN");
                        Debug.Log("It's a trap!");
                        eventWindowControl.Continue();
                        //SAME AS THIEVES EVENT
                        firstButtonControl.onClick.AddListener(delegate {
                            if (RandomState())
                            {
                                //YOU WON AGAINST THEM
                                uiLayerControl.ChangeMoney(Major);
                            }
                            else
                            {
                                //THEY BEAT YOU UP A STOLE A LOT OF MONEY
                                uiLayerControl.ChangeMoney(-Major);
                            }
                        });
                        thirdButtonControl.onClick.AddListener(delegate {
                            if (RandomState())
                            {
                                //STOLE FROM YOU
                                uiLayerControl.ChangeMoney(-Moderate);
                            }
                            else
                            {
                                Debug.Log("YOU OUTRUN THEM");
                            }
                        });
                        
                    }
                    else
                    {   
                        //HELPED HIM
                        uiLayerControl.ChangeReputation(Moderate);
                    }

                });
                thirdButtonControl.onClick.AddListener(delegate {
                    uiLayerControl.ChangeReputation(-Minor);

                    Debug.Log("LEAVE");
                });
                break;
            case RandomEventType.Thieves:
                Debug.Log("Thieves");
                eventWindowControl.SetUpEventWindow("Thieves","You see thieves trying to rob you.",
                    "FIGHT","","RUN");
                
                firstButtonControl.onClick.AddListener(delegate {
                    if (RandomState())
                    {
                        //YOU WON AGAINST THEM
                        uiLayerControl.ChangeMoney(Major);
                    }
                    else
                    {
                        //THEY BEAT YOU UP A STOLE A LOT OF MONEY
                        uiLayerControl.ChangeMoney(-Major);
                    }
                });
                
                thirdButtonControl.onClick.AddListener(delegate {
                    if (RandomState())
                    {
                        //STOLE FROM YOU
                        uiLayerControl.ChangeMoney(-Moderate);
                    }
                    else
                    {
                        Debug.Log("YOU OUTRUN THEM");
                    }
                });
                
                
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
    

    
    //Spustenie eventu na základe typu
    private void RecognizeAndRunEvent()
    {
        EventWindowControl eventWindowControl = eventWindow.GetComponent<EventWindowControl>();

        switch (eventType)
        {
            case EventType.Merchant:
                //Debug.Log("MERCHANT");
                break;
            case EventType.Random:
                //Debug.Log("RANDOM EVENT");
                SwitchEventWindow();

                if (canShowEventWindow)
                { 
                    
                    eventWindowControl.ShowWindow();
                    eventWindowControl.Init(timeCost);
                }

                break;
            case EventType.Harvest:
                //Debug.Log("HARVEST");
                break;
            case EventType.Challenge:
                //Debug.Log("CHALLENGE");
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

    //AK JE MENEJ/ROVNE AKO PERCENTAGE TAK VRATI TRUE
    private bool RandomState(int percentage = 50)
    {
        Random rnd = new Random();
        int value = rnd.Next(1,101);
        //Debug.Log(value);
        return value <= percentage;
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