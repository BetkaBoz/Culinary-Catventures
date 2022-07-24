using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manoeuvre : Card
{
    public Manoeuvre()
    {
        this.CardType = "Manoeuvre";
    }

    public abstract bool CanBeUsed();
}
