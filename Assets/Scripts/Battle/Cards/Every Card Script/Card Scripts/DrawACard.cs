using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DravACard", menuName = "Cards/Draw a card")]
public class DrawACard : Card, IDrawCards
{
    public override void CardEffect(GameManager gm, RaycastHit2D hit)
    {
        DrawCards(gm, 1);
    }

    public void DrawCards(GameManager gm, int amount)
    {
        gm.DrawCards(amount);
    }
}
