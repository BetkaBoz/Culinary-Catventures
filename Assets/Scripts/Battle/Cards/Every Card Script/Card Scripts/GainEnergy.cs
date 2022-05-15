using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gain Energy", menuName = "Cards/GainEnergy")]
public class GainEnergy : Card, ISpendEnergy
{
    public override void CardEffect(GameManager gm, RaycastHit2D hit)
    {
        SpendEnergy(gm, -1);
    }

    public void SpendEnergy(GameManager gm, int amount)
    {
        gm.SpendEnergy(amount);
    }
}
