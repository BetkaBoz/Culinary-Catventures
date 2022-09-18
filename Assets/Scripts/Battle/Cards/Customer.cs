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
    List<DebuffTypes> currentDebuffs = new List<DebuffTypes>();

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
    public event Action OnDebuffChanged;

    public float VeggieFoodDefenceBonus = 0; //Used by (de)buffs to adjust Food Defences
    public float MeatFoodDefenceBonus = 0;
    public float GeneralFoodDefenceBonus = 0;
    public float VeggieFoodDefence { get{ return _customerData.VeggieFoodDefence + VeggieFoodDefenceBonus; } } //1.25 == 25% weakness to food, 0.75 == 25% resistance to food
    public float MeatFoodDefence { get { return _customerData.MeatFoodDefence + MeatFoodDefenceBonus; } }
    public float GeneralFoodDefence { get { return _customerData.GeneralFoodDefence + GeneralFoodDefenceBonus; } }
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
    public List<DebuffTypes> CurrentDebuffs
    {
        get
        {
            return currentDebuffs;
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

    public void AddDebuff(DebuffTypes debuff)
    {
        Debug.Log(debuff + " " + (int)debuff);
        currentDebuffs.Add(debuff);
        OnDebuffChanged?.Invoke();
    }

    public void RemoveDebuff(DebuffTypes debuff)
    {
        currentDebuffs.Remove(debuff);
        OnDebuffChanged?.Invoke();
    }

    public void Captivate()
    {
        turnsLeft++;
        Stun();
    }
    public void Stun()
    {
        satisfied = true;
    }
    public void Feed(int amount, CardTypes type, bool doesSatisfy = true)
    {
        if (doesSatisfy) Stun();
        switch (type)
        {
            case CardTypes.Vegetarian:
                TakeDamage((int)(amount * _customerData.VeggieFoodDefence));
                break;
            case CardTypes.Meat:
                TakeDamage((int)(amount * _customerData.MeatFoodDefence));
                break;
            case CardTypes.Mix:
                TakeDamage((int)(amount * _customerData.GeneralFoodDefence));
                break;
            case CardTypes.Neutral:
                TakeDamage((int)(amount * _customerData.GeneralFoodDefence));
                break;
            default:
                TakeDamage(amount);
                break;
        }
    }
    public void TakeDamage(int amount)
    {
        currHunger = currHunger >= amount ? currHunger - amount : 0;

        //if (numTurnsStunned <= 0) numTurnsStunned++;

        if (currHunger ==  0)
        {
            Die(true);
            finalMoney += Data.MaxMoneyEarned;
            finalRep += Data.MaxReputationEarned;
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
