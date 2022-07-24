using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardNGainEnergy : DiscardCardEffect /*, IGainEnergy*/
{
    // [SerializeField] private string[] filter;

    public override void Effect(GameManager gm, RaycastHit2D hit)
    {
        Discard(gm, () => gm.AddEnergy(amount));
        // if (CanBeUsed(gm))
        // {
        //     Discard(gm); // miesto volania lokalnej Discard metody sa vola ta z rodica DiscardCardEffect, robi to iste
        //     gm.AddEnergy(amount);
        // }
    }

    // // TODO : ak niekomu napadne, ako toto vlozit uz do DiscardCardEffect, spravte to. Ja to sem davam len preto, ze
    // //  inak by bolo treba zbytocne overridovat aj Discard metodu
    // /// <summary>
    // /// Default check for whether Discard can be used, can be overriden in child classes
    // /// Checks for given amount of any card by default
    // /// </summary>
    // /// <param name="gm"></param>
    // /// <returns></returns>
    // protected virtual bool CanBeUsed(GameManager gm)
    // {
    //     return gm.IsEnoughCardsOnHand(discardAmount);
    // }

    // TODO : su tieto funkcie potrebne? Preco sa nepouzivaju funkcie z GameManagera priamo?

    // public void GainEnergy(GameManager gm, int amount)
    // {
    //     gm.AddEnergy(amount);
    // }
    
    // private async void Discard(GameManager gm)
    // {
    //     base.Discard(gm);
    //     // gm.SetDiscardFilter(filter);
    //     // await gm.StartDiscard();
    //     GainEnergy(gm, amount);
    // }
}
