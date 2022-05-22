using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food", menuName ="Cards/Food")]
public class FoodBase : Card, IFeed
{
    public override void CardEffect(GameManager gm, RaycastHit2D hit)
    {
        Feed(hit.transform.GetComponent<Customer>(), CalculateNp(gm));
    }

    public void Feed(Customer target, int amount)
    {
        target.Feed(amount);//,foodType
    }
}
