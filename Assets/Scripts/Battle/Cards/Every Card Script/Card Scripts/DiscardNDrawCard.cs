using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DiscardNDraw", menuName = "Cards/Discard and draw")]
public class DiscardNDrawCard : Card, IDrawCards
{
    [SerializeField] int drawAmount;
    [SerializeField] int discardAmount;
    [SerializeField] private string[] filter;
    public override void CardEffect(GameManager gm, RaycastHit2D hit)
    {
        if(discardAmount == -1)
        {
            gm.DiscardHand();
            DrawCards(gm, drawAmount);
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
        DrawCards(gm, drawAmount);
    }
}
