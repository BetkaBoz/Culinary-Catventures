using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardEffect : MonoBehaviour
{
    [SerializeField] protected int discardAmount;
    [SerializeField] protected int amount;
    [SerializeField] protected Card card;
    public Card Card
    {
        get
        {
            return card;
        }
        set
        {
            card = value;
        }
    }
    public int Amount
    {
        set
        {
            amount = value;
        }
    }
    public int DiscardAmount
    {
        set
        {
            discardAmount = value;
        }
    }
    public abstract void Effect(GameManager gm, RaycastHit2D hit);
}
