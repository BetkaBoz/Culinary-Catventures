using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gain Energy", menuName = "Cards/GainEnergy")]
public class GainEnergy : Card
{
    public override void CardEffect(GameManager gm, RaycastHit2D hit)
    {
        gm.SpendEnergy(-1);
    }
}
