using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class EventWindowControl : WindowControl
{
    #region Variables and getters
    /*
        [SerializeField] private Sprite spriteEventImg;
        [SerializeField] private TextMeshProUGUI  eventWindowName;
        [SerializeField] private TextMeshProUGUI  eventWindowText;
        [SerializeField] private TextMeshProUGUI eventFirstButtonText;
        [SerializeField] private TextMeshProUGUI eventSecondButtonText;
        [SerializeField] private TextMeshProUGUI eventThirdButtonText;
*/
        [SerializeField] private GameObject  windowName;
        [SerializeField] private GameObject  windowText;
        [SerializeField] private GameObject  windowImage;
        [SerializeField] private GameObject  firstButton;
        [SerializeField] private GameObject  secondButton;
        [SerializeField] private GameObject  thirdButton;
        [SerializeField] private List<GameObject> eventCards;

        //EVENT WINDOW SPRITES
        [SerializeField] private Sprite  homelessCatSprite;
        [SerializeField] private Sprite  diceCatSprite;
        [SerializeField] private Sprite  stumbleSprite;
        [SerializeField] private Sprite  perfectTomatoesSprite;
        [SerializeField] private Sprite  caveSprite;
        [SerializeField] private Sprite  stuckMerchantSprite;
        [SerializeField] private Sprite  thievesSprite;
        [SerializeField] private Sprite  gatherSprite;
        
        //Constants
        private const int Minor = 5;
        private const int Moderate = 10;
        private const int Major = 20;
        #endregion
        
    
    //ZMAZE VSETKY AKCIE NA TLACIDLACH OKREM ZATVORENIE OKNA
    //(LEBO TO JE NASTAVENE V INSPEKTOROVI LEN NA TRETIE TLACIDLO)
    private void RemoveAllListeners()
    {
        firstButton.GetComponent<Button>().onClick.RemoveAllListeners();
        secondButton.GetComponent<Button>().onClick.RemoveAllListeners();
        thirdButton.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    #region EventWindowSetters
    private void SetWindowName(string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            windowName.GetComponent<TMP_Text>().text = text;
        }
    }
    private void SetWindowText(string text)
    {
        windowText.GetComponent<TMP_Text>().text = text;
    }
    private void SetFirstButtonText(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            firstButton.SetActive(false);
        }
        else
        {
            firstButton.SetActive(true);
            firstButton.GetComponentInChildren<TMP_Text>().text = text;
        }
    }
    private void SetSecondButtonText(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            secondButton.SetActive(false);
        }
        else
        {
            secondButton.SetActive(true);
            secondButton.GetComponentInChildren<TMP_Text>().text = text;
        }
    }
    
    private void SetThirdButtonText(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            thirdButton.SetActive(false);
        }
        else
        {
            thirdButton.SetActive(true);
            thirdButton.GetComponentInChildren<TMP_Text>().text = text;
        }
    }
    //CAKA SA NA SPRITES
    private void AssignEventWindowSprite(String eventName)
    {
        if (string.IsNullOrEmpty(eventName)) return;
        Image imageComponent = windowImage.GetComponent<Image>();
        switch (eventName)
        {
            case "Homeless cat":
                imageComponent.sprite =  homelessCatSprite;
                break;
            case "Dice cat":
                imageComponent.sprite =  diceCatSprite;
                break;
            case "Stumble":
                imageComponent.sprite =  stumbleSprite;
                break;
            case "Perfect tomatoes":
                imageComponent.sprite =  perfectTomatoesSprite;
                break;
            case "Cave":
                imageComponent.sprite =  caveSprite;
                break;
            case "Stuck merchant":
                imageComponent.sprite =  stuckMerchantSprite;
                break;
            case "Thieves":
                imageComponent.sprite =  thievesSprite;
                break;
            //TO ISTE AKO THIEVES EVENT LEN INY OBRAZOK?
            case "Robbers":
                imageComponent.sprite =  thievesSprite;
                break;
            case "Gather":
                imageComponent.sprite =  gatherSprite;
                break;
            
            default:
                Debug.Log("What are you doing here CRIMINAL SCUM?");
                break;
        }
    }

    
    #endregion

    //DONT USE AWAKE CAUSE IT WILL OVERRIDE FUNCTION FROM PARENT CLASS

    private void SetUpEventWindow(string title,string text,string firstBtn = "",string secondBtn = "",string thirdBtn = "LEAVE")
    {
        SetWindowName(title);
        SetWindowText(text);
        SetFirstButtonText(firstBtn);
        SetSecondButtonText(secondBtn);
        SetThirdButtonText(thirdBtn);
        AssignEventWindowSprite(title);
        RemoveAllListeners();
        HideEventCards();
        ShowWindow();
    }
    
    //ZMENÍ EVENT OKNO PODĽA TYPU RANDOM EVENTU
    public void StartWindow(Event.RandomEventType randomEventType )
    {   
        //TODO: MOZNO BUDE DOBRE AK PREMENNE BUDU INDE A NECH SA NEONDIA STALE
        Button firstButtonControl = firstButton.GetComponent<Button>();
        Button secondButtonControl = secondButton.GetComponent<Button>();
        Button thirdButtonControl = thirdButton.GetComponent<Button>();
        
        switch (randomEventType)
        {
            case Event.RandomEventType.HomelessCat:
                //HOMELESS_CAT
                SetUpEventWindow("Homeless cat","You found homeless cat on the street.",
                    "HELP","ROB","");
                
                firstButtonControl.onClick.AddListener(delegate {
                    SetUpEventWindow("","You helped the homeless cat.");
                    uiLayer.ChangeMoney(-Moderate);
                    uiLayer.ChangeReputation(Major);
                });
                secondButtonControl.onClick.AddListener(delegate {
                    SetUpEventWindow("","You robbed the homeless cat.");
                    uiLayer.ChangeMoney(Moderate);
                    uiLayer.ChangeReputation(-Major);
                });
                break;
            case Event.RandomEventType.DiceCat:
                //DICE_CAT
                    SetUpEventWindow("Dice cat","You see a cat playing dice. He wants to play with you.",
                    "PLAY","DECLINE","");
                
                firstButtonControl.onClick.AddListener(delegate {
                    if (RandomState())
                    {
                        //PREHRAL
                        SetUpEventWindow("","You lost! But at least the cat is happy.");
                        uiLayer.ChangeMoney(-Moderate);
                        uiLayer.ChangeReputation(Minor);
                    }
                    else
                    {
                        //VYHRAL
                        SetUpEventWindow("","You won! The cat is happy that someone played with him.");
                        uiLayer.ChangeMoney(Moderate);
                        uiLayer.ChangeReputation(Minor);
                    }
                });
                secondButtonControl.onClick.AddListener(delegate {
                    SetUpEventWindow("","The cat is unhappy because you didn't play with him.");
                    uiLayer.ChangeReputation(-Minor);
                });
                break;
            case Event.RandomEventType.Stumble:
                //STUMBLE
                if (!player.CheckIfDeckHasIngredient())
                {
                    SetUpEventWindow("Stumble","You stumbled on a small rock. If you had an ingredient you would surely lose it, but now everyone is laughing at you!");
                    thirdButtonControl.onClick.AddListener(delegate {
                        uiLayer.ChangeReputation(-Minor);
                        //eventWindowControl.Continue();
                        //eventWindowControl.SetUpEventWindow("","You left the place and your ingredient too.");
                    });
                }
                else
                {
                    SetUpEventWindow("Stumble","You stumbled on a small rock and lost an ingredient."
                        ,"ASK FOR HELP","SEARCH");
                    CardBaseInfo randomCard = player.FindCardFromDeck();

                    Stumble(firstButtonControl,secondButtonControl ,thirdButtonControl,randomCard);
                }
                
                break;
            case Event.RandomEventType.PerfectTomatoes:
                //PERFECT_TOMATOES
                SetUpEventWindow("Perfect tomatoes","You see perfect tomatoes behind a fence.",
                    "CLIMB","DON'T TEMPT","");
                firstButtonControl.onClick.AddListener(delegate {
                    //eventWindowControl.RemoveAllListeners();
                    //30%
                    if (RandomState(30))
                    {
                        //PADOL A VSIMLI SI HO
                        SetUpEventWindow("","While climbing on the fence your tail got stuck and you fell down. Owner of the tomatoes heard you!");
                        uiLayer.ChangeReputation(-Major);
                    }
                    else
                    {
                        // PRESKOCIL A UKRADOL 2 RAJCINY :O
                        SetUpEventWindow("","You successfully climbed the fence and stole some tomatoes.");
                        
                        for (int i = 0; i < 2; i++)
                        {
                            CardBaseInfo randomCard = GetIngredient("Tomatoes");
                            player.Deck.Add(randomCard); 
                            eventCards[i].SetActive(true);
                            eventCards[i].GetComponent<Image>().sprite = randomCard.Artwork;    
                            eventCards[i].GetComponentInChildren<TextMeshProUGUI>().text = $"{randomCard.NutritionPoints}";

                        }

                        Debug.Log("GIMME DAT GRAPES");
                    }
                });//RESIST THE DARK SIDE!
                    secondButtonControl.onClick.AddListener(delegate {
                        SetUpEventWindow("","You did not fall into your temptation. God gave you some reputation.");
                        uiLayer.ChangeReputation(Minor);
                    });
                break;
            case Event.RandomEventType.Cave:
                //CAVE
                SetUpEventWindow("Cave","You see entrance to a cave and some ingredients to gather nearby.",
                    "GO IN" ,"GATHER","");
                
                firstButtonControl.onClick.AddListener(delegate {
                    //99%
                    if (RandomState(99))
                    {
                        if (RandomState())
                        {
                            //NASIEL PENIAZGY
                            uiLayer.ChangeMoney(Moderate);
                            SetUpEventWindow("","You went in and found some coins on the ground.");
                        }
                        else
                        {
                            //STRATIL PENIAZGY
                            uiLayer.ChangeMoney(-Moderate);
                            SetUpEventWindow("","You went in and in the pitch darkness someone or something took your coins.");
                        }
                    }
                    else
                    {
                        //TODO: DOKONCIT EVENT CHAIN, CURSE
                        //NASIEL RARE HELPERA: WITCH
                        SetUpEventWindow("","You found a witch cooking a special brew. She is willing to join your team , after you pay her with some ingredients",
                            "PAY","DON'T PAY","");
                        Debug.Log("YOU FOUND WITCH!");
                        firstButtonControl.onClick.AddListener(delegate {
                            uiLayer.ChangeMoney(-Moderate);
                            SetUpEventWindow("","After finishing her brew the witch joined your team.");

                        });
                        secondButtonControl.onClick.AddListener(delegate {
                            uiLayer.ChangeMoney(-Moderate);
                            //CURSE
                            SetUpEventWindow("","The witch got angry and cursed you!");
                        });
                    }
                });
                //GATHER
                secondButtonControl.onClick.AddListener(Gather);
                break;
            case Event.RandomEventType.StuckMerchant:
                //STUCK_MERCHANT
                    SetUpEventWindow("Stuck merchant","You see a stuck merchant on the road.",
                    "HELP","IGNORE","");
                
                firstButtonControl.onClick.AddListener(delegate {
                    //30%
                    if (RandomState(30))
                    {   //AMBUSH
                        SetUpEventWindow("Robbers","It's a trap! You see robbers coming to you.",
                            "FIGHT","RUN","");
                        Debug.Log("It's a trap!");
                        //SAME AS THIEVES EVENT BUT DIFFERENT
                        Thieves(firstButtonControl,secondButtonControl,"robbers");
                    }
                    else
                    {   
                        //HELPED HIM
                        SetUpEventWindow("","You helped the merchant and he thanked you.");
                        uiLayer.ChangeReputation(Major);
                    }
                });
                //IGNORE
                secondButtonControl.onClick.AddListener(delegate {
                    SetUpEventWindow("","You went the other way and ignored him.");
                    uiLayer.ChangeReputation(-Minor);

                    Debug.Log("LEAVE");
                });
                break;
            case Event.RandomEventType.Thieves:
                //THIEVES
                SetUpEventWindow("Thieves","You see thieves trying to rob you.",
                    "FIGHT","RUN","");
                
                Thieves(firstButtonControl,secondButtonControl,"thieves");
                break;
            
            default:
                Debug.Log("What are you doing here CRIMINAL SCUM?");
                break;
        }
    }
    private void Stumble(Button firstButtonControl,Button secondButtonControl,Button thirdButtonControl,CardBaseInfo card)
    {
        eventCards[0].SetActive(true);
        eventCards[0].GetComponent<Image>().sprite = card.Artwork;
        eventCards[0].GetComponentInChildren<TextMeshProUGUI>().text = $"{card.NutritionPoints}";


        firstButtonControl.onClick.AddListener(delegate {
            //35%
            if (RandomState(35))
            {
                SetUpEventWindow("","Some cats heard you and decided to help you. You found your lost ingredient");
            }
            else
            {
                SetUpEventWindow("","No one is willing to help you. You can ask again..."
                    ,"ASK FOR HELP","SEARCH");
                
                uiLayer.ChangeReputation(-Minor);
                Stumble( firstButtonControl, secondButtonControl,thirdButtonControl,card);
            }
        });
        secondButtonControl.onClick.AddListener(delegate {
            //TODO: 25% + 5% * HELPERS, VIACEJ PERCENT KED MAS HELPEROV
            if (RandomState())
            {
                if (player.helpers.Count > 0)
                {
                    SetUpEventWindow("","Your friends found your lost ingredient.");
                }
                else
                {
                    SetUpEventWindow("","You found your lost ingredient by myself.");
                }
            }
            else
            {
                if (player.helpers.Count > 0)
                {
                    SetUpEventWindow("","You are looking for the lost ingredient with your friends, but you cant find it. Other cats are looking at you..."
                        ,"ASK FOR HELP","SEARCH");
                }
                else
                {
                    SetUpEventWindow("","You are helplessly looking for the lost ingredient, but you cant find it. Other cats are looking at you..."
                        ,"ASK FOR HELP","SEARCH");
                }
                
                uiLayer.ChangeReputation(-Minor);
                Stumble( firstButtonControl,secondButtonControl,thirdButtonControl,card);
            }
        });
        
        thirdButtonControl.onClick.AddListener(delegate {
            //player.Deck.Remove(player.Deck.Find(x => x == card));
            player.RemoveCardFromDeck(card.CardName);
            //eventWindowControl.Continue();
            //eventWindowControl.SetUpEventWindow("","You left the place and your ingredient too.");
        });
    }
    private void Thieves(Button firstButtonControl,Button secondButtonControl , string who)
    {
        firstButtonControl.onClick.AddListener(delegate {
            if (RandomState())
            {
                //YOU WON AGAINST THEM
                SetUpEventWindow("",$"You managed to beat up the {who}. You take their coins.");
                uiLayer.ChangeMoney(Moderate);
            }
            else
            {
                SetUpEventWindow("",$"The {who} beat you up and stole more money than usual.");
                //THEY BEAT YOU UP A STOLE A LOT OF MONEY
                uiLayer.ChangeMoney(-Major);
            }
        });
        secondButtonControl.onClick.AddListener(delegate {
            if (RandomState())
            {
                //STOLE FROM YOU
                SetUpEventWindow("",$"The {who} catch you up and stole your money.");
                uiLayer.ChangeMoney(-Moderate);
            }
            else
            {
                SetUpEventWindow("",$"You are as fast as lighting! You don't see the {who} anymore.");
            }
        });
    }
    public void Gather()
    {
        Random rnd = new Random();
        //MIN INCLUSIVE MAX EXCLUSIVE
        int value = rnd.Next(3, 6);
        SetUpEventWindow("Gather", $"You went to gather some ingredients. You found {value} ingredients.");
        for (int i = 0; i < value; i++)
        {
            CardBaseInfo randomCard = GetRandomIngredient();
            player.Deck.Add(randomCard); 
            eventCards[i].SetActive(true);
            eventCards[i].GetComponent<Image>().sprite = randomCard.Artwork;    
            eventCards[i].GetComponentInChildren<TextMeshProUGUI>().text = $"{randomCard.NutritionPoints}";

        }
    }
    
    //AK JE MENEJ/ROVNE AKO PERCENTAGE TAK VRATI TRUE
    private bool RandomState(int percentage = 50)
    {
        Random rnd = new Random();
        int value = rnd.Next(1,101);
        //Debug.Log(value);
        return value <= percentage;
    }

    public void HideEventCards()
    {
        foreach (GameObject card in eventCards)
        {
            card.SetActive(false);
        }
    }


}
