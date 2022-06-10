using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardNDrawCard : CardEffect, IDrawCards
{
    [SerializeField] private string[] filter;
    public override void Effect(GameManager gm, RaycastHit2D hit)
    {
        if(discardAmount == -1)
        {
            gm.DiscardHand();
            DrawCards(gm, amount);
        }
        else
        {
            Discard(gm);
        }
    }
    public void DrawCards(GameManager gm, int amount)
    {
        gm.DrawCards(amount);
    }

    private async void Discard(GameManager gm)
    {
        gm.SetDiscardFilter(filter);
        await gm.StartDiscard();
        DrawCards(gm, amount);
    }
}
