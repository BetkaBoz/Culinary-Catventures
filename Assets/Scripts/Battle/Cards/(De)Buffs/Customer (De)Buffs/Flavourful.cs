using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flavourful : IBuffable
{
    public override void Apply()
    {
        if(targetCustomer != null)
        {
            targetCustomer.Feed(numOfTurns, CardTypes.None, false);
            base.Apply();
        }
    }
}
