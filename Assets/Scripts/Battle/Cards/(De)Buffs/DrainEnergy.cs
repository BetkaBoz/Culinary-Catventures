using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainEnergy : Buffable
{
    [SerializeField] int value;
    public void SetValue(int value)
    {
        this.value = value;
    }
    public override void Apply()
    {
        target.Energy -= value;
    }
}
