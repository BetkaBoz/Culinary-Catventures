using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : IBuffable
{
    public override void Apply()
    {
        if (targetCustomer != null)
        {
            targetCustomer.Stun();
            base.Apply();
        }
    }
}
