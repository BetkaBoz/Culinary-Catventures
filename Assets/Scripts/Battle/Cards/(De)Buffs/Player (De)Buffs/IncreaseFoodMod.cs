using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFoodMod:IBuffable
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
    public override void SetTarget(Customer target)
    {
        base.SetTarget(target);
        if (targetCustomer != null)
        {
            switch (type)
            {
                case "meat":
                    targetCustomer.MeatFoodDefenceBonus += value;
                    break;
                case "veggie":
                    targetCustomer.VeggieFoodDefenceBonus += value;
                    break;
                default:
                    targetCustomer.GeneralFoodDefenceBonus += value;
                    break;
            }
        }
    }
    public override void SetTarget(Player target)
    {
        base.SetTarget(target);
        if(targetPlayer != null)
        {
            switch (type)
            {
                case "meat":
                    targetPlayer.MeatFoodModBonus += value;
                    break;
                case "veggie":
                    targetPlayer.VegetarianFoodModBonus += value;
                    break;
                default:
                    targetPlayer.GeneralFoodModBonus += value;
                    break;
            }
        }
    }
    public override void EndEffect()
    {
        if (targetPlayer != null)
        {
            switch (type)
            {
                case "meat":
                    targetPlayer.MeatFoodModBonus -= value;
                    break;
                case "veggie":
                    targetPlayer.VegetarianFoodModBonus -= value;
                    break;
                default:
                    targetPlayer.GeneralFoodModBonus -= value;
                    break;
            }
        }
        if (targetCustomer != null)
        {
            switch (type)
            {
                case "meat":
                    targetCustomer.MeatFoodDefenceBonus -= value;
                    break;
                case "veggie":
                    targetCustomer.VeggieFoodDefenceBonus -= value;
                    break;
                default:
                    targetCustomer.GeneralFoodDefenceBonus -= value;
                    break;
            }
        }
        base.EndEffect();
    }

}
