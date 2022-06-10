using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseEnergy : CardEffect, IGainEnergy
{
    public override void Effect(GameManager gm, RaycastHit2D hit)
    {
        GainEnergy(gm, amount);
    }

    public void GainEnergy(GameManager gm, int amount)
    {
        gm.AddEnergy(amount);
    }
}
