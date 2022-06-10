using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawACard : CardEffect, IDrawCards
{
    public override void Effect(GameManager gm, RaycastHit2D hit)
    {
        DrawCards(gm, amount);
    }

    public void DrawCards(GameManager gm, int amount)
    {
        gm.DrawCards(amount);
    }
}
