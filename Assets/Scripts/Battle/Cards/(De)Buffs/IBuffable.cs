using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfBuffStacking
{
    AddTurns,
    Stack
}


public enum DebuffTypes:int
{
    Stun = 0,
    Captivate = 1,
    Flavourful = 2,
    WeaknessMeat = 3,
    WeaknessVegg = 4,
    WeaknessMix = 5
}

public class IBuffable : MonoBehaviour
{
    [SerializeField] TypeOfBuffStacking _typeOfStacking;
    [SerializeField] protected DebuffTypes _typeOfDebuff;
    [SerializeField] protected int numOfTurns = 0;
    protected bool finished = false;
    public bool Finished
    {
        //each buff/debuff should only last one turn
        get
        {
            return finished;
        }
    }
    public TypeOfBuffStacking TypeOfStacking => _typeOfStacking;
    public DebuffTypes TypeOfDebuff => _typeOfDebuff;
    [SerializeField] protected Player targetPlayer = null;
    [SerializeField] protected Customer targetCustomer = null;
    public Player TargetPlayer => targetPlayer;
    public Customer TargetCustomer => targetCustomer;
    public int NumOfTurns => numOfTurns;
    public virtual void CheckFinished()
    {
        if(numOfTurns <= 0)
        {
            finished = true;
        }
        else
        {
            finished = false;
        }
    }
    public void increaseTimer(int amount)
    {
        numOfTurns += amount;
    }
    public virtual void SetTarget(Player target)
    {
        targetPlayer = target;
    }
    public virtual void SetTarget(Customer target)
    {
        Debug.Log("Customer set"+target.ToString());
        targetCustomer = target;
    }
    public virtual void EndEffect()
    {
        if (targetCustomer != null)
        {
            targetCustomer.RemoveDebuff(_typeOfDebuff);
        }
        Destroy(gameObject);
    }
    public virtual void Apply()
    {
        numOfTurns--;
        CheckFinished();
    }
}
