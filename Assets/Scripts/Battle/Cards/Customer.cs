using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

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

    public int money, rep;

    private void Awake()
    {
        currHunger = maxHunger;
        numTurnsStunned = 0;
        hunger.text = $"{currHunger}";
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
        if (numTurnsStunned > 0) numTurnsStunned--;
        //if(turnsUntilAngry >= 8)
        //{
        //    //Debug.Log("I sleep");
        //}
        //else if (turnsUntilAngry >= 5)
        //{
        //    //Debug.Log("This is fine");
        //}
        //else if(turnsUntilAngry >= 3)
        //{
        //    //Debug.Log("Yo what up?!");
        //}
        //else if(turnsUntilAngry >= 1)
        //{
        //    //Debug.Log("I'm angeri");
        //}
        if (turnsUntilAngry == 0)
        {
            if (currHunger >= maxHunger / 2)
            {
                money += 5;
                rep += 5;
            }
            else if (currHunger >= maxHunger / 3)
            {
                money += 10;
                rep += 10;
            }
            else if (currHunger >= maxHunger / 4)
            {
                money += 25;
                rep += 25;
            }
            Die(true);
            gm.Player.TakeDamage(25);
            return true;
        }
        return false;
    }

    public void OnDrop(PointerEventData eventData)
    {

    }
    public void Feed(int amount)
    {
        TakeDamage(amount);
    }

    public void RandomizeDebuffs()
    {
        ac.Suffle();
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
            money += 50;
            rep += 50;
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
        gm.CustomerListDelete(this);
    }
}
