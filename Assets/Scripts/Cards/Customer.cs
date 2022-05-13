using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Customer : MonoBehaviour, IDropHandler, IDamageable
{
    [SerializeField] private int maxHunger = 0;
    [SerializeField] private byte turnsUntilAngry = 0;
    [SerializeField] private Text hunger;
    [SerializeField] private GameManager gm;
    private int currHunger;
    private byte numTurnsStunned;

    private bool isDead = false;

    private void Awake()
    {
        currHunger = maxHunger;
        numTurnsStunned = 0;
        hunger.text = $"{currHunger}";
    }

    public void Feed(int amount)
    {
        TakeDamage(amount);
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
            //GetComponentsInChildren<Image>().DOFade(new Color(0, 0, 0, 0), 1f, 2f).OnComplete(() => { Destroy(gameObject); });
            isDead = true;
            GetComponent<Image>().DOColor(new Color(0, 0, 0, 0), 5f).OnComplete(() => { Destroy(gameObject); });
        }
    }

    private void OnDestroy()
    {
        gm.customerListDelete(this);
    }
}
