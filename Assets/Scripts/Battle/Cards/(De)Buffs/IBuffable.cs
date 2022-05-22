using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffable : MonoBehaviour
{
    [SerializeField] int numOfTurns = 1;
    public bool Finished
    {
        //each buff/debuff should only last one turn
        get
        {
            if (numOfTurns > 0)
            {
                numOfTurns--;
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    [SerializeField] protected Player target;
    public void SetTarget(Player target)
    {
        this.target = target;
    }
    public void EndEffect()
    {
        Destroy(gameObject);
    }
    public virtual void Apply()
    {
    }
}
