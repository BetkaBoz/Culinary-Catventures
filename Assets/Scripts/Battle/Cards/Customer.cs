using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Customer : MonoBehaviour, IDropHandler, IDamageable
{
    [SerializeField] int maxHunger = 0;
    [SerializeField] byte turnsUntilAngry = 0;
    [SerializeField] Text hunger;
    [SerializeField] GameManager gm;
    [SerializeField] ActionManager ac;
    [SerializeField] Player player;
    [SerializeField] Image bubbleAwait;
    [SerializeField] Image bubbleAction;

    private int currHunger;
    private byte numTurnsStunned;
    private bool satisfied;
    private bool isDead = false;

 
    private void Awake()
    {
        bubbleAwait.gameObject.SetActive(false);
        bubbleAction.gameObject.SetActive(true);
        bubbleAction.DOColor(new Color(0, 0, 0, 0), 5f).OnComplete(() => 
        {
            bubbleAwait.gameObject.SetActive(true);
            bubbleAction.gameObject.SetActive(false); 
        });

        currHunger = maxHunger;
        numTurnsStunned = 0;
        hunger.text = $"{currHunger}";
    }

    public void Feed(int amount) 
    {
        TakeDamage(amount);
    }

    public bool StartTurn()
    {
        bubbleAwait.gameObject.SetActive(false);
        if (!satisfied) 
        {
            switch (ac.CurrentIndex)
            {
                case 0:
                    player.TakeDamage(-5);
                    satisfied = true;
                    break;
                case 1:
                    gm.AddEnergy(-1);
                    satisfied = true;
                    break;
            }
            gm.EndEnemyTurn();
        }
        return false;
    }

    public bool EndTurn()
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
            Die(true);

            player.TakeDamage(-25);

            Debug.Log("Fok dis shid I'm out");
            return true;
        }

        return false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("ide to");
    }

    public void TakeDamage(int amount)
    {
        satisfied = true;

        currHunger = currHunger >= amount ? currHunger - amount : 0;
        if (numTurnsStunned <= 0) numTurnsStunned++;
        Debug.Log("Yum yum " + currHunger.ToString() + " " + amount.ToString());
        hunger.text = $"{currHunger}";

        if (currHunger ==  0)
        {
            Die(true);
            //++money and rep
        }
        
    }

    public void Die(bool status)
    {
        if (!isDead)
        {
            isDead = true;
            bubbleAwait.DOColor(new Color(0, 0, 0, 0), 2f);
            GetComponent<Image>().DOColor(new Color(0, 0, 0, 0), 2f).OnComplete(() => { Destroy(gameObject); });
        }
    }

    private void OnDestroy()
    {
        gm.customerListDelete(this);
    }
}
