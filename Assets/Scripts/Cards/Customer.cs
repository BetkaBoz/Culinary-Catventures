using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Customer : MonoBehaviour, IDropHandler
{
    [SerializeField] private int maxHunger = 0;
    [SerializeField] private byte turnsUntilAngry = 0;
    [SerializeField] private Text hunger;
    private int currHunger;
    private byte numTurnsStunned;

    private void Awake()
    {
        currHunger = maxHunger;
        numTurnsStunned = 0;
        hunger.text = $"{currHunger}";
    }

    public void Feed(int amount)
    {
        currHunger = currHunger >= amount ? currHunger - amount : 0;
        if (numTurnsStunned <= 0) numTurnsStunned++; 
        Debug.Log("Yum yum " + currHunger.ToString() + " " + amount.ToString());
        hunger.text = $"{currHunger}";
    }

    public void EndTurn()
    {
        turnsUntilAngry--;
        numTurnsStunned--;
        Debug.Log("hunger: " + currHunger.ToString());
        if(turnsUntilAngry >= 8)
        {
            Debug.Log("I sleep");
        }
        else if (turnsUntilAngry >= 5)
        {
            Debug.Log("This is fine");
        }
        else if(turnsUntilAngry >= 3)
        {
            Debug.Log("Yo what up?!");
        }
        else if(turnsUntilAngry >= 1)
        {
            Debug.Log("I'm angeri");
        }
        else
        {
            Debug.Log("Fok dis shid I'm out");
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("ide to");
    }
}
