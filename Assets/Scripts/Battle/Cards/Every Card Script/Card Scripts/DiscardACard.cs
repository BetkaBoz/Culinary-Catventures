using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardACard : CardEffect, IGainEnergy
{
    [SerializeField] private string[] filter;

    public override void Effect(GameManager gm, RaycastHit2D hit)
    {
        Discard(gm);
    }

    public void GainEnergy(GameManager gm, int amount)
    {
        gm.AddEnergy(amount);
    }

    private async void Discard(GameManager gm)
    {
        gm.SetDiscardFilter(filter);
        await gm.StartDiscard();
        GainEnergy(gm, amount);
    }
}
