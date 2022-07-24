using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DiscardCardEffect : CardEffect
{
    [SerializeField] protected string[] filter;

    protected async void Discard(GameManager gm, Action discardSuccessfulCallback = null)
    {
        // if (CanBeUsed(gm))
        // {
            gm.SetDiscardFilter(filter);
            await gm.StartDiscard();
            discardSuccessfulCallback?.Invoke();
        // }
    }

    /// <summary>
    /// Default check for whether Discard can be used, can be overriden in child classes
    /// Checks for given amount of any card by default
    /// 'virtual' makes it possible to be ovrwritten in child classes and used here
    /// </summary>
    /// <param name="gm"></param>
    /// <returns></returns>
    public virtual bool CanBeUsed(GameManager gm)
    {
        return gm.IsEnoughCardsOnHand(discardAmount);
    }
}