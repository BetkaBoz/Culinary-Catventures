using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public abstract class Customer : IDamageable
{
    GameManager gm;
    CustomerData _customerData;
    int currHunger;
    byte numTurnsStunned;
    bool satisfied = false;
    bool isDead = false;

    public abstract string Name { get; }
    public int finalMoney = 0;
    public int finalRep = 0;
    public event Action OnDamageTaken;
    public event Action OnDied;
    public event Action OnTurnStarted;
    public int CurrentAction { get; private set; }
    public int Money => finalMoney;
    public int Rep => finalRep;
    public int MaxHunger => _customerData.MaxHunger;
    public int TurnsLeft => _customerData.TurnsUntilAngry;
    public int CurrentHunger => currHunger;
    public bool Satisfied => satisfied;
    

    public void SetUp(GameManager gameManager, CustomerData customerData)
    {
        gm = gameManager;
        _customerData = customerData;
        currHunger = customerData.MaxHunger;
        numTurnsStunned = 0;
    }

    public virtual void StartTurn()
    {
        if (!satisfied) 
        {
            switch (CurrentAction)
            {
                case 1:
                    gm.HurtPlayer(5);
                    break;
                case 2:
                    //GameObject temp = Instantiate(debuffs);
                    //gm.BuffPlayer(temp.GetComponent<IBuffable>());
                    break;
            }
        }
    }

    public abstract bool EndTurn();

    public abstract void ChangeExpressions();
    public virtual void RandomizeDebuffs()
    {
        if (TurnsLeft == 1) CurrentAction = 0;
        else CurrentAction = UnityEngine.Random.Range(1, _customerData.Sprites.Count);
    }
    public void OnDrop(PointerEventData eventData)
    {

    }
    public void Feed(int amount)
    {
        TakeDamage(amount);
    }
    public void TakeDamage(int amount)
    {
        satisfied = true;
        currHunger = currHunger >= amount ? currHunger - amount : 0;

        if (numTurnsStunned <= 0) numTurnsStunned++;

        if (currHunger ==  0)
        {
            Die(true);
            finalMoney += 50;
            finalRep += 50;
            gm.Player.ChangeMoney(finalMoney);
            gm.Player.earnedRep += finalRep;
        }
        OnDamageTaken?.Invoke();    //if(OnDamageTaken != null) OnDamageTaken.Invoke(); -> null conditional operator
    }
    public void Die(bool status)
    {
        isDead = true;
        gm.CustomerListDelete(this);
        OnDied?.Invoke();
    }
}
