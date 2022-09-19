using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Captivate : IBuffable
{
    public override void SetTarget(Customer target)
    {
        base.SetTarget(target);
        Apply();
    }
    public override void Apply()
    {
        if (targetCustomer != null && targetCustomer.CurrentAction == 0)
        {
            Debug.Log("Heyooo I'm apllyin");
            targetCustomer.Captivate();
            base.Apply();
        }
    }
}
