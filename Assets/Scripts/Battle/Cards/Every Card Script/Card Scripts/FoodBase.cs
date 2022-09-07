using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBase : CardEffect, IFeed
{
    public override void Effect(GameManager gm, RaycastHit2D hit)
    {
        Feed(hit.transform.GetComponent<CustomerView>(), card.CalculateNP(gm));
    }

    public void Feed(CustomerView target, int amount)
    {
        target.Customer.Feed(amount, card.CardType);//,foodType
    }
}
