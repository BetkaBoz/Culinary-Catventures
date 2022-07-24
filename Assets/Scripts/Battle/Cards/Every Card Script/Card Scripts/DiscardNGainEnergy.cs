using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardNGainEnergy : DiscardCardEffect /*, IGainEnergy*/
{
    public override void Effect(GameManager gm, RaycastHit2D hit)
    {
        Discard(gm, () => gm.AddEnergy(amount));
    }
}
