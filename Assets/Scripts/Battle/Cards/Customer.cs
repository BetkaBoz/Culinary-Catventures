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
    [SerializeField] Image States;
    [SerializeField] List<Sprite> sprites;
    [SerializeField] GameObject debuffs;
    private int currHunger;
    private byte numTurnsStunned;
    private bool satisfied = false;
    private bool isDead = false;
    public int money = 0;
    public int rep = 0;
    public int Money => money;
    public int Rep => rep;
    public int MaxHunger => maxHunger;

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

        //change customers expressions based on how many turns are left till end of the battle
        if (turnsUntilAngry >= 8)
            States.DOFade(1, 0.2f).OnPlay(() => { States.sprite = sprites[0]; });
        else if (turnsUntilAngry >= 5)
            States.DOFade(1, 0.2f).OnPlay(() => { States.sprite = sprites[1]; });
        else if (turnsUntilAngry >= 3)
            States.DOFade(1, 0.2f).OnPlay(() => { States.sprite = sprites[2]; });
        else if (turnsUntilAngry >= 1)
            States.DOFade(1, 0.2f).OnPlay(() => { States.sprite = sprites[3]; });

        //when there are 0 turns left, check how much was customer satisfied
        //and based on this info add money and reputation to the player
        //or cause reputation demage on player
        if (turnsUntilAngry == 0)
        {
            if (currHunger >= maxHunger / 1.2f) gm.Player.TakeDamage(25); 
            else if (currHunger >= maxHunger / 2)
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
            gm.Player.ChangeMoney(money);
            gm.Player.ChangeReputation(rep);
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
            gm.Player.ChangeMoney(money);
            gm.Player.ChangeReputation(rep);
        }
    }
    public void Die(bool status)
    {
        if (!isDead)
        {
            isDead = true;
            Action.DOFade(0, 2f);
            States.DOFade(0, 2f);
            GetComponent<Image>().DOFade(0, 2f).OnComplete(() => { Destroy(gameObject); });
        }
    }
    private void OnDestroy()
    {
        gm.CustomerListDelete(this);
    }
}
