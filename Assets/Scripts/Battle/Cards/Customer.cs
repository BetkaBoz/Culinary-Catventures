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
    //[SerializeField] GameObject debuffs;
    CustomerData _customerData;
    int currentAction;

    protected GameManager gm;
    protected int turnsLeft;
    protected int numTurnsStunned;
    protected int currHunger;
    protected bool satisfied = false;

    public int finalMoney = 0;
    public int finalRep = 0;
    public event Action OnDamageTaken;
    public event Action OnDied;
    public event Action OnTurnStarted;
    public event Action OnTurnEnded;
    public event Action OnActionChanged;
    public int CurrentAction 
    {   
        get
        {
            return currentAction;
        } 
        private set
        {
            currentAction = value;
            OnActionChanged?.Invoke();
        } 
    }
    public int Money => finalMoney;
    public int Rep => finalRep;
    public int CurrentHunger => currHunger;
    public bool Satisfied => satisfied;
    public int TurnsLeft => turnsLeft;
    public CustomerData Data => _customerData;

    public void SetUp(GameManager gameManager, CustomerData customerData)
    {
        gm = gameManager;
        _customerData = customerData;
        currHunger = customerData.MaxHunger;
        turnsLeft = customerData.Turns;
        numTurnsStunned = 0;
        gm.CustomerListAdd(this);
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
                    gm.Player.Energy -= 1;
                    //GameObject temp = UnityEngine.Object.Instantiate(debuffs);
                    //gm.BuffPlayer(temp.GetComponent<IBuffable>());
                    break;
            }
        }
        OnTurnStarted?.Invoke();
    }

    public virtual void EndTurn()
    {
        OnTurnEnded?.Invoke();
    }

    public virtual void RandomizeDebuffs()
    {
        if (turnsLeft == 1) CurrentAction = 0;
        else CurrentAction = UnityEngine.Random.Range(1, _customerData.ActionSprites.Count);
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
        gm.CustomerListDelete(this);
        OnDied?.Invoke();
    }
}
