using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class TasteTheDish : DiscardNGainEnergy
{
    public override bool CanBeUsed(GameManager gm)
    {
        return gm.IsEnoughFoodOnHand(discardAmount);
    }
}
