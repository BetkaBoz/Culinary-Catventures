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
    private GameObject  eventWindow;
    //UILAYER
    private GameObject  uiLayer;

    private IslandManager islandManager;
    
    private bool isOnEvent;
    private bool isUsed;
    //private bool canShowEventWindow = true;
    //private GameObject playerGrabber;

    private const int Minor = 5;
    private const int Moderate = 10;
    private const int Major = 20;
    
    #endregion
    
    private void Awake()
    {
        eventWindow = GameObject.FindGameObjectWithTag("EventWindow");
        uiLayer = GameObject.FindGameObjectWithTag("UILayer");
        uiLayer.GetComponent<UILayer>().UpdateUI();
        islandManager = FindObjectOfType<IslandManager>();
    }

    void Start()
    {
        //spriteEventImg = eventWindow.GetComponentInChildren()

        AssignRandomType();
        AssignSprite();
        eventWindow.GetComponent<EventWindowControl>().HideWindow();

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
    //ZMENÍ EVENT OKNO PODĽA TYPU RANDOM EVENTU
    private void SwitchRandomEventWindow(EventWindowControl eventWindowControl )
    {   
        //TODO: MOZNO BUDE DOBRE AK PREMENNE BUDU INDE A NECH SA NEONDIA STALE
        //EventWindowControl eventWindowControl = eventWindow.GetComponent<EventWindowControl>();
        UILayer uiLayerControl = uiLayer.GetComponent<UILayer>();
        Button firstButtonControl = eventWindowControl.FirstButton.GetComponent<Button>();
        Button secondButtonControl = eventWindowControl.SecondButton.GetComponent<Button>();
        Button thirdButtonControl = eventWindowControl.ThirdButton.GetComponent<Button>();

        
        switch (randomEventType)
        {
            case RandomEventType.HomelessCat:
                //HOMELESS_CAT
                eventWindowControl.SetUpEventWindow("Homeless cat","You found homeless cat on the street.",
                    "HELP","ROB","");
                
                firstButtonControl.onClick.AddListener(delegate {
                    eventWindowControl.SetUpEventWindow("","You helped the homeless cat.");
                    uiLayerControl.ChangeMoney(-Moderate);
                    uiLayerControl.ChangeReputation(Major);
                });
                secondButtonControl.onClick.AddListener(delegate {
                    eventWindowControl.SetUpEventWindow("","You robbed the homeless cat.");
                    uiLayerControl.ChangeMoney(Moderate);
                    uiLayerControl.ChangeReputation(-Major);
                });
                break;
            case RandomEventType.DiceCat:
                //DICE_CAT
                eventWindowControl.SetUpEventWindow("Dice cat","You see a cat playing dice. He wants to play with you.",
                    "PLAY","DECLINE","");
                
                firstButtonControl.onClick.AddListener(delegate {
                    if (RandomState())
                    {
                        //PREHRAL
                        eventWindowControl.SetUpEventWindow("","You lost! But at least the cat is happy.");
                        uiLayerControl.ChangeMoney(-Moderate);
                        uiLayerControl.ChangeReputation(Minor);
                    }
                    else
                    {
                        //VYHRAL
                        eventWindowControl.SetUpEventWindow("","You won! The cat is happy that someone played with him.");
                        uiLayerControl.ChangeMoney(Moderate);
                        uiLayerControl.ChangeReputation(Minor);
                    }
                });
                secondButtonControl.onClick.AddListener(delegate {
                    eventWindowControl.SetUpEventWindow("","The cat is unhappy because you didn't play with him.");
                    uiLayerControl.ChangeReputation(-Minor);
                });
                break;
            case RandomEventType.Stumble:
                //STUMBLE
                eventWindowControl.SetUpEventWindow("Stumble","You stumbled on a small rock and lost an ingredient."
                ,"ASK FOR HELP","SEARCH");
                
                Stumble(firstButtonControl,secondButtonControl ,thirdButtonControl, eventWindowControl, uiLayerControl);
                break;
            case RandomEventType.PerfectTomatoes:
                //PERFECT_TOMATOES
                eventWindowControl.SetUpEventWindow("Perfect tomatoes","You see perfect tomatoes behind a fence.",
                    "CLIMB","DON'T TEMPT","");
                firstButtonControl.onClick.AddListener(delegate {
                    //eventWindowControl.RemoveAllListeners();
                    //30%
                    if (RandomState(30))
                    {
                        //PADOL A VSIMLI SI HO
                        eventWindowControl.SetUpEventWindow("","While climbing on the fence your tail got stuck and you fell down. Owner of the tomatoes heard you!");
                        uiLayerControl.ChangeReputation(-Major);
                    }
                    else
                    {
                        //TODO: PRESKOCIL A UKRADOL 2 RAJCINY :O
                        eventWindowControl.SetUpEventWindow("","You successfully climbed the fence and stole some tomatoes.");
                        Debug.Log("GIMME DAT GRAPES");
                    }
                });
                    secondButtonControl.onClick.AddListener(delegate {
                        eventWindowControl.SetUpEventWindow("","You did not fall into your temptation. God gave you some reputation.");
                        uiLayerControl.ChangeReputation(Minor);
                    });
                break;
            case RandomEventType.Cave:
                //CAVE
                eventWindowControl.SetUpEventWindow("Cave","You see entrance to a cave and some ingredients to gather nearby.",
                    "GO IN" ,"GATHER","");
                
                firstButtonControl.onClick.AddListener(delegate {
                    //99%
                    if (RandomState(99))
                    {
                        if (RandomState())
                        {
                            //NASIEL PENIAZGY
                            uiLayerControl.ChangeMoney(Moderate);
                            eventWindowControl.SetUpEventWindow("","You went in and found some coins on the ground.");
                        }
                        else
                        {
                            //STRATIL PENIAZGY
                            uiLayerControl.ChangeMoney(-Moderate);
                            eventWindowControl.SetUpEventWindow("","You went in and in the pitch darkness someone or something took your coins.");
                        }
                    }
                    else
                    {
                        //TODO: DOKONCIT EVENT CHAIN, CURSE
                        //NASIEL RARE HELPERA: WITCH
                        eventWindowControl.SetUpEventWindow("","You found a witch cooking a special brew. She is willing to join your team , after you pay her with some ingredients",
                            "PAY","DON'T PAY","");
                        Debug.Log("YOU FOUND WITCH!");
                        firstButtonControl.onClick.AddListener(delegate {
                            uiLayerControl.ChangeMoney(-Moderate);
                            eventWindowControl.SetUpEventWindow("","After finishing her brew the witch joined your team.");

                        });
                        secondButtonControl.onClick.AddListener(delegate {
                            uiLayerControl.ChangeMoney(-Moderate);
                            //CURSE
                            eventWindowControl.SetUpEventWindow("","The witch got angry and cursed you!");
                        });
                    }
                });
                //GATHER
                //TODO: GET SOM INGREDIENTS
                secondButtonControl.onClick.AddListener(delegate {
                    Gather(eventWindowControl);
                });
                break;
            case RandomEventType.StuckMerchant:
                //STUCK_MERCHANT
                eventWindowControl.SetUpEventWindow("Stuck merchant","You see a stuck merchant on the road.",
                    "HELP","IGNORE","");
                
                firstButtonControl.onClick.AddListener(delegate {
                    //30%
                    if (RandomState(30))
                    {   //AMBUSH
                        eventWindowControl.SetUpEventWindow("Robbers","It's a trap! You see robbers coming to you.",
                            "FIGHT","RUN","");
                        Debug.Log("It's a trap!");
                        //SAME AS THIEVES EVENT BUT DIFFERENT
                        Thieves(firstButtonControl,secondButtonControl , eventWindowControl, uiLayerControl,"robbers");
                    }
                    else
                    {   
                        //HELPED HIM
                        eventWindowControl.SetUpEventWindow("","You helped the merchant and he thanked you.");
                        uiLayerControl.ChangeReputation(Major);
                    }
                });
                //IGNORE
                secondButtonControl.onClick.AddListener(delegate {
                    eventWindowControl.SetUpEventWindow("","You went the other way and ignored him.");
                    uiLayerControl.ChangeReputation(-Minor);

                    Debug.Log("LEAVE");
                });
                break;
            case RandomEventType.Thieves:
                //THIEVES
                eventWindowControl.SetUpEventWindow("Thieves","You see thieves trying to rob you.",
                    "FIGHT","RUN","");
                
                Thieves(firstButtonControl,secondButtonControl, eventWindowControl, uiLayerControl,"thieves");
                break;
            
            default:
                Debug.Log("What are you doing here CRIMINAL SCUM?");
                break;
        }
    }
    private void Stumble(Button firstButtonControl,Button secondButtonControl,Button thirdButtonControl,EventWindowControl eventWindowControl,UILayer uiLayerControl)
    {
        //TODO: STRATIT NAHODNU INGREDIENCIU Z DECKU
        firstButtonControl.onClick.AddListener(delegate {
            //TODO: 35%
            if (RandomState(35))
            {
                eventWindowControl.SetUpEventWindow("","Some cats heard you and decided to help you. You found your lost ingredient");
            }
            else
            {
                eventWindowControl.SetUpEventWindow("","No one is willing to help you. You can ask again."
                    ,"ASK FOR HELP","SEARCH");
                
                uiLayerControl.ChangeReputation(-Minor);
                Stumble( firstButtonControl, secondButtonControl,thirdButtonControl, eventWindowControl, uiLayerControl);
            }
        });
        secondButtonControl.onClick.AddListener(delegate {
            //TODO: 50%, VIACEJ PERCENT KED MAS HELPEROV
            if (RandomState())
            {               
                eventWindowControl.SetUpEventWindow("","Some cats heard you and decided to help you. You found your lost ingredient");
            }
            else
            {
                eventWindowControl.SetUpEventWindow("","You are looking for the lost ingredient but cant find it. Other cats are looking at you..."
                    ,"ASK FOR HELP","SEARCH");
                uiLayerControl.ChangeReputation(-Minor);
                Stumble( firstButtonControl,secondButtonControl,thirdButtonControl, eventWindowControl, uiLayerControl);
            }
        });
        
        thirdButtonControl.onClick.AddListener(delegate {
            //eventWindowControl.Continue();
            //eventWindowControl.SetUpEventWindow("","You left the place and your ingredient too.");
        });
    }
    private void Thieves(Button firstButtonControl,Button secondButtonControl,EventWindowControl eventWindowControl,UILayer uiLayerControl, string who)
    {
        firstButtonControl.onClick.AddListener(delegate {
            if (RandomState())
            {
                //YOU WON AGAINST THEM
                eventWindowControl.SetUpEventWindow("",$"You managed to beat up the {who}. You take their coins.");
                uiLayerControl.ChangeMoney(Moderate);
            }
            else
            {
                eventWindowControl.SetUpEventWindow("",$"The {who} beat you up and stole more money than usual.");
                //THEY BEAT YOU UP A STOLE A LOT OF MONEY
                uiLayerControl.ChangeMoney(-Major);
            }
        });
        secondButtonControl.onClick.AddListener(delegate {
            if (RandomState())
            {
                //STOLE FROM YOU
                eventWindowControl.SetUpEventWindow("",$"The {who} catch you up and stole your money.");
                uiLayerControl.ChangeMoney(-Moderate);
            }
            else
            {
                eventWindowControl.SetUpEventWindow("",$"You are as fast as lighting! You don't see the {who} anymore.");
            }
        });
    }
    private void Gather(EventWindowControl eventWindowControl)
    {
        //TODO: PRIDAT INGREDIENCIE DO DECKU,ZMENIT VSTUPNE PARAMETRE DO FUNKCIE
        Random rnd = new Random();
        int value = rnd.Next(3, 6);
        eventWindowControl.SetUpEventWindow("Gather", $"You went to gather some ingredients. You found {value} ingredients.");
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
        EventWindowControl eventWindowControl = eventWindow.GetComponent<EventWindowControl>();

        switch (eventType)
        {
            case EventType.Merchant:
                //Debug.Log("MERCHANT");
                break;
            case EventType.Random:
                //Debug.Log("RANDOM EVENT");
                SwitchRandomEventWindow(eventWindowControl);
                /*
                if (canShowEventWindow)
                {
                    eventWindowControl.ShowWindow();
                    eventWindowControl.Init(timeCost);
                }
*/
                break;
            case EventType.Gather:
                Gather(eventWindowControl);
                
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
        Gather = 2,
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