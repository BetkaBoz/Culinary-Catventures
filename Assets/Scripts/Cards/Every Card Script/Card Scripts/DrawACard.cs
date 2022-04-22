using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DravACard", menuName = "Cards/Draw a card")]
public class DrawACard : Card
{
    public override void CardEffect(GameManager gm, RaycastHit2D hit)
    {
        gm.DrawCards(1);
    }
}
