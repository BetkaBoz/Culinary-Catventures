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
    //poob
    private int currHunger;
    private byte numTurnsStunned;
    private bool satisfied;
    private bool isDead = false;

    private void Start()
    {
        Action.DOFade(0f, 0f).OnComplete(() => {
            Action.DOFade(1f, 1f); 
        });

        //switch (ac.CurrentIndex)
        //{
        //    case 0:
        //        //message = "If customer won't be fed this round, they will cause less EP in next round";
        //        //header = "Energy Debuff";
        //        TooltipManager.Show("If customer won't be fed this round, they will cause less EP in next round", "Energy Debuff");
        //        break;
        //    case 1:
        //        //message = "If customer won't be fed this round, they will take your reputation points";
        //        //header = "Reputation Debuff";
        //        TooltipManager.Show("If customer won't be fed this round, they will take your reputation points ", "Reputation Debuff");
        //        break;
        //}
    }

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

    public void StartTurn()
    {
        if (!satisfied) 
        {
            switch (ac.CurrentIndex)
            {
                case 0:
                    GameObject temp = Instantiate(debuffs);
                    gm.BuffPlayer(temp.GetComponent<IBuffable>());
                    satisfied = true;
                    break;
                case 1:
                    gm.HurtPlayer(5);
                    satisfied = true;
                    break;
            }
        }
    }

    public bool EndTurn()
    {
        turnsUntilAngry--;
        if (numTurnsStunned > 0)
            numTurnsStunned--;
        //Debug.Log("hunger: " + currHunger.ToString());
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

            gm.Player.TakeDamage(-25);

            //Debug.Log("Fok dis shid I'm out");
            return true;
        }

        return false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("ide to");
    }

    public void TakeDamage(int amount)
    {
        satisfied = true;

        currHunger = currHunger >= amount ? currHunger - amount : 0;
        if (numTurnsStunned <= 0) numTurnsStunned++;
        //Debug.Log("Yum yum " + currHunger.ToString() + " " + amount.ToString());
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
            Action.DOColor(new Color(0, 0, 0, 0), 2f);
            GetComponent<Image>().DOColor(new Color(0, 0, 0, 0), 2f).OnComplete(() => { Destroy(gameObject); });
        }
    }

    private void OnDestroy()
    {
        gm.customerListDelete(this);
    }
}
