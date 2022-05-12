using System;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class EventManager : MonoBehaviour
{
    [SerializeField] bool isChallenge = false;
    [SerializeField] EventType eventType;
    [SerializeField] TextMeshProUGUI  BtnPrompt;
    [SerializeField] private int timeCost = 1;

    private IslandManager islandManager;
    private bool isOnEvent = false;
    private bool isUsed = false;
    void Start()
    {
        if (isChallenge) this.eventType = EventType.Challenge;
        else assignRandomType();
        
        islandManager = FindObjectOfType<IslandManager>();
    }

    public void assignRandomType()
    {
        Array values = Enum.GetValues(typeof(EventType));
        Random random = new Random(Guid.NewGuid().GetHashCode());
        this.eventType = (EventType) values.GetValue(random.Next(values.Length - 1));
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
                Debug.Log("Buy shit!");
                break;
            case EventType.Academy:
                Debug.Log("Learn shit!");
                break;
            case EventType.Forest:
                Debug.Log("Lose shit!");
                break;
            case EventType.Lava:
                Debug.Log("Shit yourself!");
                break;
            case EventType.Challenge:
                Debug.Log("Betka ma bije!");
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
        Academy = 2,
        Forest = 3,
        Lava = 4,

        Challenge = 10
    }
}