using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DiscardACard", menuName = "Cards/Discard a card")]
public class DiscardACard : Card
{
    public override void CardEffect(GameManager gm, RaycastHit2D hit)
    {
        Discard(gm);
    }
    private async void Discard(GameManager gm)
    {
        await gm.StartDiscard();
        //yield return new WaitUntil(() => gm.discardPhase == false);
        gm.SpendEnergy(-2);
    }
}
