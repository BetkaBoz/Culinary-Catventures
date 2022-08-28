using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


public class EventManager : MonoBehaviour
{
    #region Vars

    [SerializeField] public TextMeshProUGUI btnPrompt; //FOR NOW
    //[SerializeField] private int timeCost = 1;

    //EVENT SPRITES
    [SerializeField] public Sprite spriteRandom;
    [SerializeField] public Sprite spriteMerchant;
    [SerializeField] public Sprite spriteHarvest;
    [SerializeField] public Sprite spriteChallenge;
    [SerializeField] public Sprite spriteSensei;

    [SerializeField] public List<Event> allEvents;
    //EVENT WINDOW
    [SerializeField] private EventWindowControl eventWindow;
    // MERCHARNT WINDOW
    [SerializeField] private MerchantWindowControl merchantWindow;
    // SENSEI WINDOW
    [SerializeField] private SenseiWindowControl senseiWindow;
    //ISLAND MANAGER
    private IslandManager islandManager;


    public List<int> tmpRandomEvents = new List<int>();

    public static bool IsInEvent;

    [HideInInspector] public int merchantCount;
    [HideInInspector] public int senseiCount;
    [HideInInspector] public int gatherCount;

    //CONSTANTS
    public const int MaxMerchantCount = 1;
    public const int MaxSenseiCount = 1;
    public const int MaxGatherCount = 2;

    #endregion

    private void Awake()
    {
        islandManager = FindObjectOfType<IslandManager>();
        ResetScene();
    }

    private void ResetScene()
    {
        Time.timeScale = 1;
        IsInEvent = false;
    }

    void Start()
    {
        AssignRandomTypes();
        ClearBtnPrompt();
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
    //VYBERA NAHODNY EVENT NA MAPE A PRIDAVA MU TYP A SPRITE
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
            //@event.DEVELOPER_AssignRandomEvent();
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
                //merchantWindow.Init(eventt.timeCost);
                merchantWindow.StartWindow();

                break;
            case Event.EventType.Random:
                //Debug.Log("RANDOM EVENT");
                //eventWindow.Init(eventt.timeCost);
                eventWindow.StartWindow(eventt.randomEventType);

                break;
            case Event.EventType.Gather:
                //eventWindow.Init(eventt.timeCost);
                eventWindow.Gather();

                break;
            case Event.EventType.Sensei:
                //senseiWindow.Init(eventt.timeCost);
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

}
