using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseFoodMod:IBuffable
{
    [SerializeField] float value;
    [SerializeField] string type;
    public void SetValue(float value)
    {
        this.value = value;
    }
    public void SetType(string type)
    {
        this.type = type;
    }
    public override void Apply()
    {
        switch (type)
        {
            case "meat":
                target.MeatFoodModBonus += value;
                break;
            case "veggie":
                target.VegetarianFoodModBonus += value;
                break;
            default:
                target.GeneralFoodModBonus += value;
                break;
        }
    }

}
