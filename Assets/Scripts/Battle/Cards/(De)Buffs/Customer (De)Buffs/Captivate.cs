using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Captivate : IBuffable
{
    public override void Apply()
    {
        if (targetCustomer != null && targetCustomer.CurrentAction == 0)
        {
            targetCustomer.Captivate();
            base.Apply();
        }
    }
}
