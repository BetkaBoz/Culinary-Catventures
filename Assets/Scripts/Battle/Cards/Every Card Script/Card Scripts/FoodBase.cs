using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBase : CardEffect, IFeed
{
    public override void Effect(GameManager gm, RaycastHit2D hit)
    {
        Feed(hit.transform.GetComponent<Customer>(), card.CalculateNP(gm));
    }

    public void Feed(Customer target, int amount)
    {
        target.Feed(amount);//,foodType
    }
}
