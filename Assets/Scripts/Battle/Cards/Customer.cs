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
    [SerializeField] Image Action;
    [SerializeField] GameObject debuffs;
    
    private int currHunger;
    private byte numTurnsStunned;
    private bool satisfied = false;
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

    public void RandomizeDebuffs()
    {
        ac.Suffle();
    }

    public void StartTurn()
    {
        Action.DOFade(1f, 1f);

        if (!satisfied) 
        {
            switch (ac.CurrentIndex)
            {
                case 0:
                    gm.HurtPlayer(5);
                    
                    break;
                case 1:
                    GameObject temp = Instantiate(debuffs);
                    gm.BuffPlayer(temp.GetComponent<IBuffable>());
                    break;
            }
        }
    }

    public bool EndTurn()
    {
        turnsUntilAngry--;
        if (numTurnsStunned > 0)
            numTurnsStunned--;
        if(turnsUntilAngry >= 8)
        {
            //Debug.Log("I sleep");
        }
        else if (turnsUntilAngry >= 5)
        {
            //Debug.Log("This is fine");
        }
        else if(turnsUntilAngry >= 3)
        {
            //Debug.Log("Yo what up?!");
        }
        else if(turnsUntilAngry >= 1)
        {
            //Debug.Log("I'm angeri");
        }
        else
        {
            Die(true);
            gm.Player.TakeDamage(125);
            return true;
        }

        return false;
    }

    public void OnDrop(PointerEventData eventData)
    {

    }

    public void TakeDamage(int amount)
    {
        satisfied = true;

        currHunger = currHunger >= amount ? currHunger - amount : 0;
        if (numTurnsStunned <= 0) numTurnsStunned++;
        hunger.text = $"{currHunger}";

        Action.DOFade(0f, 1f);

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
            Action.DOColor(new Color(0, 0, 0, 0), 2f);
            GetComponent<Image>().DOColor(new Color(0, 0, 0, 0), 2f).OnComplete(() => { Destroy(gameObject); });
        }
    }

    private void OnDestroy()
    {
        gm.customerListDelete(this);
    }
}
