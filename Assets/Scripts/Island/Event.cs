using System;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;


public class Event : MonoBehaviour
{
    #region Private Vars

    [SerializeField] public bool isChallenge;
    [SerializeField] public EventType eventType;
    [SerializeField] public RandomEventType randomEventType;
    //[SerializeField] public int timeCost = 1;
    [SerializeField] private Image imageComponent;
    [SerializeField] private Material eventGlowMaterial;

    private bool isUsed;
    private bool isOnEvent;

    //EVENT MANAGER
    [SerializeField] private EventManager eventManager;
    //ISLAND MANAGER
    [SerializeField] private IslandManager islandManager;
    //PLAYER CHARACTER
    [SerializeField] private GameObject playerCharacter;

    #endregion

    private void Awake()
    {
        imageComponent = GetComponentInChildren<Image>();

        //  eventManager = FindObjectOfType<EventManager>();
        //islandManager = FindObjectOfType<IslandManager>();
        //playerCharacter = GameObject.FindGameObjectWithTag("PlayerCharacter");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            ActivateEvent();
        }
        //SPUSTENIE EVENTU AK HO AKTIVUJEME
    }

    //URČI TYP EVENTU, CHALLENGE SA NASTAVUJE Z INŠPEKTORA
    public void AssignRandomType()
    {
        if (isChallenge) eventType = EventType.Challenge;
        //JEDEN MERCHANT
        else if (eventManager.merchantCount < EventManager.MaxMerchantCount)
        {
            eventManager.merchantCount++;
            eventType = EventType.Merchant;
        } //JEDEN SENSEI
        else if (eventManager.senseiCount < EventManager.MaxSenseiCount)
        {
            eventManager.senseiCount++;
            eventType = EventType.Sensei;
        } //DVA GATHERY
        else if (eventManager.gatherCount < EventManager.MaxGatherCount)
        {
            eventManager.gatherCount++;
            eventType = EventType.Gather;
        }
        else
        {
            eventType = EventType.Random;
            /*
                Array values = Enum.GetValues(typeof(EventType));
                Random random = new Random(Guid.NewGuid().GetHashCode());
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
                */
        }
    }
    public void DEVELOPER_AssignRandomEvent()
    {
        eventType = EventType.Random;
        islandManager.time = 69;
    }

    //URČI NÁHODNÝ TYP RANDOM EVENTU
    private void AssignRandomEventRandomType()
    {
        Array rvalues = Enum.GetValues(typeof(RandomEventType));
        Random random = new Random(Guid.NewGuid().GetHashCode());
        int eventNumber = random.Next(rvalues.Length);
        //Debug.Log(eventNumber);
        if (!eventManager.tmpRandomEvents.Contains(eventNumber))
        {
            eventManager.tmpRandomEvents.Add(eventNumber);
            randomEventType = (RandomEventType)rvalues.GetValue(eventNumber);
        }
        else
        {
            AssignRandomEventRandomType();
        }
    }

    //PRIRADÍ SPRITE PODĹA TYPU EVENTU
    public void AssignSprite()
    {

        switch (eventType)
        {
            case EventType.Random:
                imageComponent.sprite = eventManager.spriteRandom;
                AssignRandomEventRandomType();
                break;
            case EventType.Gather:
                imageComponent.sprite = eventManager.spriteHarvest;
                break;
            case EventType.Merchant:
                imageComponent.sprite = eventManager.spriteMerchant;
                break;
            case EventType.Sensei:
                imageComponent.sprite = eventManager.spriteSensei;
                break;
            case EventType.Challenge:
                imageComponent.sprite = eventManager.spriteChallenge;
                break;
            default:
                Debug.Log("What are you doing here CRIMINAL SCUM?");
                break;
        }
    }

    //ZMENENIE VELKOSTI SPRITU EVENTU
    private void ChangeEventScale()
    {
        if (isOnEvent)
        {
            imageComponent.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1);
            imageComponent.material = eventGlowMaterial;
        }
        else
        {
            imageComponent.gameObject.transform.localScale = new Vector3(1, 1, 1);
            imageComponent.material = null;

        }
    }

    //NABEHNUTIE HRÁČA NA EVENT
    private void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log(gameObject.name + " Collided");
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

    private void OnMouseUp()
    {
        if (EventManager.IsInEvent) return;
        ActivateEvent();
    }

    private void OnMouseEnter()
    {
        if (isUsed || EventManager.IsInEvent) return;
        isOnEvent = true;
        ChangeEventScale();
    }

    private void OnMouseExit()
    {
        isOnEvent = false;
        ChangeEventScale();
    }


    private void ActivateEvent()
    {
        if (isOnEvent && !isUsed && (islandManager.time > 0 || eventType == EventType.Challenge))
        {
            EventManager.IsInEvent = true;
            //Time.timeScale = 0;
            eventManager.ClearBtnPrompt();
            eventManager.RecognizeAndRunEvent(this);
            LockEvent();
            MoveCharacterToEvent();
        }
    }
    //ZAMKNUTIE EVENTU / NEMOZE SA UZ SPUSTIT
    public void LockEvent()
    {
        isUsed = true;
        imageComponent.color = new Color32(125, 125, 125, 255);
        imageComponent.gameObject.transform.localScale = new Vector3(1, 1, 1);
        imageComponent.material = null;
    }
    //OBJAVENIE HRACA PRI POUZITOM EVENTE
    private void MoveCharacterToEvent()
    {
        Vector3 position = transform.position;
        Vector3 scale = playerCharacter.transform.localScale;

        //AK JE EVENT NA PRAVEJ STRANE OSTROVA
        if (transform.position.x >= 0)
        {
            position.x -= 0.60f;
            //POZERA SA DO PRAVA
            if (playerCharacter.transform.localScale.x < 0)
            {
                scale.x *= -1;
            }
        }
        else
        {
            position.x += 0.60f;

            if (playerCharacter.transform.localScale.x > 0)
            {
                scale.x *= -1;
            }
        }
        playerCharacter.transform.position = position;
        playerCharacter.transform.localScale = scale;
    }


    public enum EventType
    {
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

        ShrineOfWealth = 8,
        ShrineOfFood = 9,
        FallenNest = 10,
        DrowningCat = 11,
        Maze = 12,
        SlotMachine = 13,
    }


}
