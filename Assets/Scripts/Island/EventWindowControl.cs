using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class EventWindowControl : WindowControl
{
    #region Variables and getters

    [SerializeField] private Image windowImage;
    [SerializeField] private TextMeshProUGUI windowName;
    [SerializeField] private TextMeshProUGUI windowText;
    [SerializeField] private Button firstButton;
    [SerializeField] private Button secondButton;
    [SerializeField] private Button thirdButton;
    [SerializeField] private GameObject coinUI;
    [SerializeField] private GameObject repUI;
    [SerializeField] private List<GameObject> eventCards;

    /*
        [SerializeField] private GameObject  windowName;
        [SerializeField] private GameObject  windowText;
        [SerializeField] private GameObject  windowImage;
        [SerializeField] private GameObject  firstButton;
        [SerializeField] private GameObject  secondButton;
        [SerializeField] private GameObject  thirdButton;
    */


    private List<CardBaseInfo> tmpCards = new List<CardBaseInfo>();

    //EVENT WINDOW SPRITES
    [SerializeField] private Sprite homelessCatSprite;
    [SerializeField] private Sprite diceCatSprite;
    [SerializeField] private Sprite stumbleSprite;
    [SerializeField] private Sprite perfectTomatoesSprite;
    [SerializeField] private Sprite caveSprite;
    [SerializeField] private Sprite stuckMerchantSprite;
    [SerializeField] private Sprite thievesSprite;

    [SerializeField] private Sprite shrineOfWealthSprite;
    [SerializeField] private Sprite shrineOfFoodSprite;
    [SerializeField] private Sprite fallenNestSprite;
    [SerializeField] private Sprite drowningCatSprite;
    [SerializeField] private Sprite mazeSprite;
    [SerializeField] private Sprite slotMachineSprite;

    [SerializeField] private Sprite gatherSprite;

    //Constants
    private const int Minor = 5;
    private const int Moderate = 10;
    private const int Major = 20;

    #endregion


    //ZMAZE VSETKY AKCIE NA TLACIDLACH OKREM ZATVORENIE OKNA
    //(LEBO TO JE NASTAVENE V INSPEKTOROVI LEN NA TRETIE TLACIDLO)
    private void RemoveAllListeners()
    {
        firstButton.onClick.RemoveAllListeners();
        secondButton.onClick.RemoveAllListeners();
        thirdButton.onClick.RemoveAllListeners();
    }

    #region EventWindowSetters

    private void SetWindowName(string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            windowName.text = text;
        }
    }
    private void SetWindowText(string text)
    {
        windowText.text = text;
    }
    private void SetFirstButtonText(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            firstButton.gameObject.SetActive(false);
        }
        else
        {
            firstButton.gameObject.SetActive(true);
            firstButton.GetComponentInChildren<TMP_Text>().text = text;
        }
    }
    private void SetSecondButtonText(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            secondButton.gameObject.SetActive(false);
        }
        else
        {
            secondButton.gameObject.SetActive(true);
            secondButton.GetComponentInChildren<TMP_Text>().text = text;
        }
    }

    private void SetThirdButtonText(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            thirdButton.gameObject.SetActive(false);
            enabled = false;

        }
        else
        {
            thirdButton.gameObject.SetActive(true);
            thirdButton.GetComponentInChildren<TMP_Text>().text = text;
            enabled = true;

        }
    }
    //CAKA SA NA SPRITES
    private void AssignEventWindowSprite(String eventName)
    {
        if (string.IsNullOrEmpty(eventName)) return;
        //Image imageComponent = windowImage.GetComponent<Image>();
        switch (eventName)
        {
            case "Homeless cat":
                windowImage.sprite = homelessCatSprite;
                break;
            case "Dice cat":
                windowImage.sprite = diceCatSprite;
                break;
            case "Stumble":
                windowImage.sprite = stumbleSprite;
                break;
            case "Perfect tomatoes":
                windowImage.sprite = perfectTomatoesSprite;
                break;
            case "Cave":
                windowImage.sprite = caveSprite;
                break;
            case "Stuck merchant":
                windowImage.sprite = stuckMerchantSprite;
                break;
            case "Thieves":
                windowImage.sprite = thievesSprite;
                break;
            //TO ISTE AKO THIEVES EVENT LEN INY OBRAZOK?
            case "Robbers":
                windowImage.sprite = thievesSprite;
                break;

            case "Shrine of wealth":
                windowImage.sprite = shrineOfWealthSprite;
                break;
            case "Shrine of food":
                windowImage.sprite = shrineOfFoodSprite;
                break;
            case "Fallen nest":
                windowImage.sprite = fallenNestSprite;
                break;
            case "Drowning cat":
                windowImage.sprite = drowningCatSprite;
                break;
            case "Maze":
                windowImage.sprite = mazeSprite;
                break;
            case "Slot machine":
                windowImage.sprite = slotMachineSprite;
                break;

            case "Gather":
                windowImage.sprite = gatherSprite;
                break;

            default:
                Debug.Log("What are you doing here CRIMINAL SCUM?");
                break;
        }
    }

    #endregion

    //DONT USE AWAKE CAUSE IT WILL OVERRIDE FUNCTION FROM PARENT CLASS

    private void SetUpEventWindow(string title, string text, string firstBtn = "", string secondBtn = "", string thirdBtn = "LEAVE")
    {
        SetWindowName(title);
        SetWindowText(text);
        SetFirstButtonText(firstBtn);
        SetSecondButtonText(secondBtn);
        SetThirdButtonText(thirdBtn);
        AssignEventWindowSprite(title);
        RemoveAllListeners();
        HideEventCardsAndCurrencies();
        ShowWindow();
    }

    //ZMENÍ EVENT OKNO PODĽA TYPU RANDOM EVENTU
    public void StartWindow(Event.RandomEventType randomEventType)
    {
        switch (randomEventType)
        {
            case Event.RandomEventType.HomelessCat:
                //HOMELESS_CAT
                SetUpEventWindow("Homeless cat", "You found homeless cat on the street.",
                    "HELP", "ROB", "");

                firstButton.onClick.AddListener(delegate {
                    SetUpEventWindow("", "You helped the homeless cat by giving them some of your coins.");
                    ChangeMoney(-Moderate);
                    ChangeReputation(Major);
                });
                secondButton.onClick.AddListener(delegate {
                    SetUpEventWindow("", "You robbed the homeless cat... That wasn't very nice of you and everyone will remember that.");
                    ChangeMoney(Moderate);
                    ChangeReputation(-Major);
                });
                break;
            case Event.RandomEventType.DiceCat:
                //DICE_CAT
                SetUpEventWindow("Dice cat", "You see a kitten playing dice. You locked eyes and looks like he's eager to play with you.",
                    "PLAY", "DECLINE", "");

                firstButton.onClick.AddListener(delegate {
                    if (RandomState())
                    {
                        //PREHRAL
                        SetUpEventWindow("", "You decided to play so you roll the dice and....You lost! You lost some of your money but at least the kittne is happy...");
                        ChangeMoney(-Moderate);
                        ChangeReputation(Minor);
                    }
                    else
                    {
                        //VYHRAL
                        SetUpEventWindow("", "You decided to play so you roll the dice and... You won! You took kitten's money he bet with but he looks happy that someone played with him.");
                        ChangeMoney(Moderate);
                        ChangeReputation(Minor);
                    }
                });
                secondButton.onClick.AddListener(delegate {
                    SetUpEventWindow("", "You quickly broke the eye contact and went on. The cat is sad because you didn't play with him.");
                    ChangeReputation(-Minor);
                });
                break;
            case Event.RandomEventType.Stumble:
                //STUMBLE
                player = FindObjectOfType<Player>();
                if (!player.CheckIfDeckHasIngredient())
                {
                    SetUpEventWindow("Stumble", "On your walk getting groceries you tripped over a small rock and fell down. If you had an ingredient you would surely lose it... but now everyone is laughing at you!");
                    thirdButton.onClick.AddListener(delegate {
                        ChangeReputation(-Minor);
                    });
                }
                else
                {
                    SetUpEventWindow("Stumble", "On your walk getting groceries you tripped over a small rock and fell down. Some ingredients you had in a bag rolled out on dusty road. You lost an ingredient!"
                        , "ASK FOR HELP", "SEARCH");
                    CardBaseInfo randomCard = player.FindCardFromDeck();

                    Stumble(randomCard);
                }

                break;
            case Event.RandomEventType.PerfectTomatoes:
                //PERFECT_TOMATOES
                SetUpEventWindow("Perfect tomatoes", "On your wanders you came across a perfect tomatoes behind someone's fence.",
                    "CLIMB", "DON'T TEMPT", "");
                firstButton.onClick.AddListener(delegate {
                    //30%
                    if (RandomState(30))
                    {
                        //PADOL A VSIMLI SI HO
                        SetUpEventWindow("", "The tomatoes looked so tasty! While awkwardly climbing on the fence your tail got stuck and you fell over the fence with big *THUD*. Owner of the tomatoes heard you and chased you away!");
                        ChangeReputation(-Major);
                    }
                    else
                    {
                        // PRESKOCIL A UKRADOL 2 RAJCINY :O
                        SetUpEventWindow("", "The tomatoes looked so tasty! You looked around and decided to climb the fence and it was... Successfull! You *borrowed* some tomatoes and ran away.");

                        for (int i = 0; i < 2; i++)
                        {
                            CardBaseInfo randomCard = GetIngredient("Tomatoes");
                            player.Deck.Add(randomCard);
                            tmpCards.Add(randomCard);
                        }
                        ShowCards(tmpCards, 2);
                        Debug.Log("GIMME DAT GRAPES");
                    }
                }); //RESIST THE DARK SIDE!
                secondButton.onClick.AddListener(delegate {
                    SetUpEventWindow("", "You shook your head and resisted the temptation. Cat God saw your determination and gave you some reputation.");
                    ChangeReputation(Minor);
                });
                break;
            case Event.RandomEventType.Cave:
                //CAVE
                SetUpEventWindow("Cave", "You see a cave with some ingredients to gather nearby the enterance.",
                    "GO IN", "GATHER", "");

                firstButton.onClick.AddListener(delegate {
                    //99%
                    if (RandomState(99))
                    {
                        if (RandomState())
                        {
                            //NASIEL PENIAZGY
                            SetUpEventWindow("", "You went in but you didn't wander too far. In slight shine of the sun entering the cave you noticed someting shiny on the ground. You found some coins!");
                            ChangeMoney(Moderate);
                        }
                        else
                        {
                            //STRATIL PENIAZGY
                            SetUpEventWindow("", "You went in and went deep into the cave. In pitch darkness you fell over a trap and someone or something took your coins!");
                            ChangeMoney(-Moderate);
                        }
                    }
                    else
                    {
                        //TODO: DOKONCIT EVENT CHAIN, CURSE, POTREBOVANIE INGREDIENCII NA SECRET BREW
                        //NASIEL RARE HELPERA: WITCH
                        SetUpEventWindow("", "You found a witch cooking a special brew. She is willing to join your team , after you pay her with some ingredients",
                            "PAY", "DON'T PAY", "");
                        Debug.Log("YOU FOUND WITCH!");
                        firstButton.onClick.AddListener(delegate {
                            SetUpEventWindow("", "After finishing her suspicious brew the witch joined your team for some... coins.");
                            ChangeMoney(-Moderate);

                        });
                        secondButton.onClick.AddListener(delegate {
                            SetUpEventWindow("", "The witch got angry and cursed you!");
                            ChangeReputation(-Minor);
                            //CURSE
                        });
                    }
                });
                //GATHER
                secondButton.onClick.AddListener(Gather);
                break;
            case Event.RandomEventType.StuckMerchant:
                //STUCK_MERCHANT
                SetUpEventWindow("Stuck merchant", "You see a stuck merchant under crushd carriage on the road.",
                    "HELP", "IGNORE", "");

                firstButton.onClick.AddListener(delegate {
                    //30%
                    if (RandomState(30))
                    {
                        //AMBUSH
                        SetUpEventWindow("Robbers", "It's a trap! You see robbers waiting for you to rob you as well.",
                            "FIGHT", "RUN", "");
                        Debug.Log("It's a trap!");
                        //SAME AS THIEVES EVENT BUT DIFFERENT
                        Thieves("robbers");
                    }
                    else
                    {
                        //HELPED HIM
                        SetUpEventWindow("", "You helped the merchant to get out. He thanked you and will spread a word of your actions.");
                        ChangeReputation(Major);
                    }
                });
                //IGNORE
                secondButton.onClick.AddListener(delegate {
                    SetUpEventWindow("", "You went the other way and ignored him.");
                    ChangeReputation(-Minor);
                });
                break;
            case Event.RandomEventType.Thieves:
                //THIEVES
                SetUpEventWindow("Thieves", "You see thieves trying to rob you.",
                    "FIGHT", "RUN", "");

                Thieves("thieves");
                break;
            case Event.RandomEventType.ShrineOfWealth:
                //SHRINE
                SetUpEventWindow("Shrine of wealth", "You encounter a sacred shrine dedicated to god of wealth and riches. Everyone should pray here ...",
                    "PRAY", "DESECRATE", "");

                firstButton.onClick.AddListener(delegate {
                    SetUpEventWindow("", "You prayed to god of wealth, he rewarded you with good luck.");
                    ChangeReputation(Moderate);
                });
                secondButton.onClick.AddListener(delegate {
                    SetUpEventWindow("", "You desecrated this holy place. The god is angry at you!");
                    ChangeMoney(Major);
                    ChangeReputation(-Major);
                });
                break;
            case Event.RandomEventType.ShrineOfFood:
                //SHRINE
                SetUpEventWindow("Shrine of food", "You encounter a sacred shrine dedicated to god of food and cooking. Everyone should pray here ...",
                    "PRAY", "DESECRATE", "");

                firstButton.onClick.AddListener(delegate {
                    SetUpEventWindow("", "You prayed to god of food, he rewarded you with good luck.");
                    ChangeReputation(Moderate);
                });
                secondButton.onClick.AddListener(delegate {
                    SetUpEventWindow("", "You desecrated this holy place. The god is angry at you!");
                    for (int i = 0; i < 2; i++)
                    {
                        CardBaseInfo randomFood = GetRandomFood();
                        player.Deck.Add(randomFood);
                        tmpCards.Add(randomFood);
                    }
                    ShowCards(tmpCards, 2);
                    ChangeReputation(-Major);
                });
                break;
            case Event.RandomEventType.FallenNest:
                //FALLEN NEST
                SetUpEventWindow("Fallen nest", "In the woods you find a fallen bird nest with eggs inside. You can save the eggs or...",
                    "SAVE", "TAKE", "");

                firstButton.onClick.AddListener(delegate {
                    SetUpEventWindow("", "You put the nest back in the tree.");
                    ChangeReputation(Moderate);
                });
                secondButton.onClick.AddListener(delegate {
                    SetUpEventWindow("", "You take the eggs into your bag without thinking...");
                    for (int i = 0; i < 2; i++)
                    {
                        CardBaseInfo randomFood = GetIngredient("Egg");
                        player.Deck.Add(randomFood);
                        tmpCards.Add(randomFood);
                    }
                    ShowCards(tmpCards, 2);
                    ChangeReputation(-Major);
                });
                break;
            case Event.RandomEventType.DrowningCat:
                //DROWNING CAT
                SetUpEventWindow("Drowning cat", "You see a drowning cat in the nearby lake!",
                    "SAVE", "IGNORE", "");
                firstButton.onClick.AddListener(delegate {
                    if (RandomState())
                    {
                        SetUpEventWindow("", "You saved the cat from certain death! The cat thanked you...");
                        ChangeReputation(Minor);
                    }
                    else
                    {
                        SetUpEventWindow("", "You saved the cat from certain death! You lost some coins in the water but the cat is alive thanks to you...");
                        ChangeMoney(-Minor);
                        ChangeReputation(Minor);
                    }
                });
                secondButton.onClick.AddListener(delegate {
                    SetUpEventWindow("", "You are scared of deep water, you looked the other way...");
                    ChangeReputation(-Moderate);
                });

                break;
            case Event.RandomEventType.Maze:
                SetUpEventWindow("Maze", "You find a maze in the forest, maybe there is a treasure in the end. You can go left, right or just leave."
                    , "LEFT", "RIGHT");

                Maze();
                break;
            case Event.RandomEventType.SlotMachine:
                SetUpEventWindow("Slot machine", "You find a abandoned slot machine, you need to pay in order to play."
                    , "PLAY");

                SlotMachine();
                break;
            default:
                Debug.Log("What are you doing here CRIMINAL SCUM?");
                break;
        }
    }
    private void Stumble(CardBaseInfo card)
    {
        ShowOneCard(card, 2);

        //STUMBLE
        firstButton.onClick.AddListener(delegate {
            //35%
            if (RandomState(35))
            {
                SetUpEventWindow("", "Some cats heard you and decided to help you. You found your lost ingredient");
            }
            else
            {
                SetUpEventWindow("", "No one is willing to help you. You can ask again..."
                    , "ASK FOR HELP", "SEARCH");

                ChangeReputation(-Minor);
                Stumble(card);
            }
        });
        secondButton.onClick.AddListener(delegate {
            //TODO: 25% + 5% * HELPERS, VIACEJ PERCENT KED MAS HELPEROV
            if (RandomState())
            {
                if (player.helpers.Count > 0)
                {
                    SetUpEventWindow("", "Your friends found your lost ingredient.");
                }
                else
                {
                    SetUpEventWindow("", "You found your lost ingredient by yourself.");
                }
            }
            else
            {
                if (player.helpers.Count > 0)
                {
                    SetUpEventWindow("", "You are looking for the lost ingredient with your friends, but you cant find it. Other cats are looking at you..."
                        , "ASK FOR HELP", "SEARCH");
                }
                else
                {
                    SetUpEventWindow("", "You are helplessly looking for the lost ingredient, but you cant find it. Other cats are looking at you..."
                        , "ASK FOR HELP", "SEARCH");
                }

                ChangeReputation(-Minor);
                Stumble(card);
            }
        });

        thirdButton.onClick.AddListener(delegate {
            player.RemoveCardFromDeck(card.CardName);
        });
    }
    private void Thieves(string who)
    {
        //THIEVES
        firstButton.onClick.AddListener(delegate {
            //PEACE WAS NEVER AN OPTION
            //40% + HELPERS * 20%
            if (RandomState(40 + player.helpers.Count * 20))
            {
                //YOU WON AGAINST THEM
                SetUpEventWindow("", $"You managed to beat up the {who}. You take their coins.");
                ChangeMoney(Moderate);
                ChangeReputation(Moderate);
            }
            else
            {
                SetUpEventWindow("", $"The {who} beat you up and stole your money.");
                //THEY BEAT YOU UP A STOLE A LOT OF MONEY
                ChangeMoney(-Major);
            }
        });
        secondButton.onClick.AddListener(delegate {
            //90% - HELPERS * 20%
            if (RandomState(90 - player.helpers.Count * 20))
            {
                SetUpEventWindow("", $"You are as fast as lighting! You don't see the {who} anymore. You tell the police cats what happened and they thanked you...");
                ChangeReputation(Minor);
            }
            else
            {
                //STOLE FROM YOU
                SetUpEventWindow("", $"The {who} catch you up and stole your money.");
                ChangeMoney(-Major);
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
            tmpCards.Add(randomCard);
        }
        ShowCards(tmpCards, 2);
    }
    private void Maze()
    {
        //MAZE
        firstButton.onClick.AddListener(delegate {
            //30%
            if (RandomState(30))
            {
                SetUpEventWindow("", "You fell in mud...", "LEFT", "RIGHT");
                Maze();
                ChangeReputation(-Moderate);
            }
            else
            {
                //33%
                if (RandomState(33))
                {
                    //GRAND PRIZE
                    SetUpEventWindow("", "You find treasure in the middle of the maze!");
                    Maze();
                    ChangeMoney(Major * 5);

                }
                else
                {
                    SetUpEventWindow("", "You did not find anything.", "LEFT", "RIGHT");
                    Maze();
                }
            }
        });
        secondButton.onClick.AddListener(delegate {
            //30%
            if (RandomState(30))
            {
                SetUpEventWindow("", "You fell in the mud...", "LEFT", "RIGHT");
                Maze();
                ChangeReputation(-Moderate);
            }
            else
            {
                //33%
                if (RandomState(33))
                {
                    //GRAND PRIZE
                    SetUpEventWindow("", "You find treasure in the middle of the maze!");
                    Maze();
                    ChangeMoney(Major * 5);
                }
                else
                {
                    SetUpEventWindow("", "You did not find anything.", "LEFT", "RIGHT");
                    Maze();

                }
            }
        });
    }
    private void SlotMachine()
    {
        //SLOT MACHINE
        firstButton.onClick.AddListener(delegate {
            //5%
            if (player.Money < 5)
            {
                SetUpEventWindow("", "You don't have enough coins to play!");
            }
            else if (RandomState(5))
            {
                SetUpEventWindow("", "You break the machine! Some coins fall on the ground but you can't play anymore.");
                ChangeMoney(Minor);
            }
            else
            {
                //25%
                if (RandomState(20))
                {
                    SetUpEventWindow("", "You won! You can play again...", "PLAY");
                    SlotMachine();
                    ChangeMoney(Major);
                }
                else
                {
                    SetUpEventWindow("", "You lost! You can play again...", "PLAY");
                    ChangeMoney(-Minor);
                    SlotMachine();
                }
            }
        });
    }


    private void ShowCards(List<CardBaseInfo> cards, int color)
    {
        Color32 imageColor = ChangeColor(color);
        int i = 0;
        foreach (CardBaseInfo card in cards)
        {
            eventCards[i].SetActive(true);
            eventCards[i].transform.Find("Energy").gameObject.GetComponentInChildren<TextMeshProUGUI>().text = $"{card.EnergyCost}";
            eventCards[i].GetComponent<Image>().sprite = card.Artwork;
            eventCards[i].GetComponentInChildren<Text>().text = $"{card.NutritionPoints}";
            eventCards[i].GetComponent<Image>().color = imageColor;
            i++;
        }
    }
    private void ShowOneCard(CardBaseInfo card, int color)
    {
        eventCards.First().SetActive(true);
        eventCards.First().transform.Find("Energy").gameObject.GetComponentInChildren<TextMeshProUGUI>().text = $"{card.EnergyCost}";
        eventCards.First().GetComponent<Image>().sprite = card.Artwork;
        eventCards.First().GetComponentInChildren<Text>().text = $"{card.NutritionPoints}";
        Color32 imageColor = ChangeColor(color);
        eventCards.First().GetComponent<Image>().color = imageColor;


    }
    private Color32 ChangeColor(int color)
    {
        switch (color)
        {
            case 0:
                //RED
                return new Color32(255, 200, 200, 255);
            case 1:
                //GREEN
                return new Color32(200, 255, 200, 255);
            case 2:
                //TRANSPARENT
                return new Color32(255, 255, 255, 255);
            default:
                //WHAT IS THIS COLOR?
                Debug.Log("WHAT IS THIS COLOR?");
                return new Color32(200, 50, 255, 255);

        }
    }
    //AK JE MENEJ/ROVNE AKO PERCENTAGE TAK VRATI TRUE
    private bool RandomState(int percentage = 50)
    {
        Random rnd = new Random();
        int value = rnd.Next(1, 101);
        //Debug.Log(value);
        return value <= percentage;
    }

    private void HideEventCardsAndCurrencies()
    {
        foreach (GameObject card in eventCards)
        {
            card.GetComponent<Image>().color = Color.clear;
            card.SetActive(false);
        }
        tmpCards?.Clear();
        repUI.SetActive(false);
        coinUI.SetActive(false);
    }

    private void ChangeMoney(int coin)
    {
        coinUI.SetActive(true);
        TextMeshProUGUI text = coinUI.GetComponentInChildren<TextMeshProUGUI>();
        if (coin > 0)
        {
            text.text = $"+{coin}";
            text.color = Color.green;

        }
        else
        {
            text.text = $"{coin}";
            text.color = Color.red;

        }
        //Debug.Log("COIN: " + coin);
        uiLayer.ChangeMoney(coin);
    }
    private void ChangeReputation(int rep)
    {
        repUI.SetActive(true);
        TextMeshProUGUI text = repUI.GetComponentInChildren<TextMeshProUGUI>();
        if (rep > 0)
        {
            text.text = $"+{rep}";
            text.color = Color.green;

        }
        else
        {
            text.text = $"{rep}";
            text.color = Color.red;

        }
        //Debug.Log("REP: " + rep);
        uiLayer.ChangeReputation(rep);
    }
}
