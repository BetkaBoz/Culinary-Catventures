using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using Random = System.Random;

public class EventManager : MonoBehaviour
{
    [SerializeField] bool isChallenge = false;
    [SerializeField] EventType eventType;
    [SerializeField] private int timeCost = 1;

    void Start()
    {
        if (isChallenge) this.eventType = EventType.Challenge;
        else assignRandomType();
    }

    public void assignRandomType()
    {
        Array values = Enum.GetValues(typeof(EventType));
        Random random = new Random(Guid.NewGuid().GetHashCode());
        this.eventType = (EventType) values.GetValue(random.Next(values.Length - 1));
    }

    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        IslandManager islandManager = FindObjectOfType<IslandManager>();
        islandManager.lowerTime(timeCost);
        
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

    enum EventType : int
    {
        Merchant = 1,
        Academy = 2,
        Forest = 3,
        Lava = 4,

        Challenge = 10
    }
}