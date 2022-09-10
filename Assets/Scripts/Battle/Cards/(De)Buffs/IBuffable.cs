using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IBuffable : MonoBehaviour
{
    [SerializeField] protected int numOfTurns = 1;
    protected bool finished = false;
    public bool Finished
    {
        //each buff/debuff should only last one turn
        get
        {
            return finished;
        }
    }
    [SerializeField] protected Player targetPlayer = null;
    [SerializeField] protected Customer targetCustomer = null;
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
    public virtual void SetTarget(Player target)
    {
        targetPlayer = target;
    }
    public virtual void SetTarget(Customer target)
    {
        targetCustomer = target;
    }
    public virtual void EndEffect()
    {
        Destroy(gameObject);
    }
    public virtual void Apply()
    {
        numOfTurns--;
        CheckFinished();
    }
}
