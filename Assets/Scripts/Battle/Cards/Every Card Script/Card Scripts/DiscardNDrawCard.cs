using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardNDrawCard : DiscardCardEffect /*, IDrawCards*/
{
    public override void Effect(GameManager gm, RaycastHit2D hit)
    {
        Discard(gm, () => gm.DrawCards(amount));
    }
}
