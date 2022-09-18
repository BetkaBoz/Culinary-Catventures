using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFoodMod:IBuffable
{
    [SerializeField] float value;
    [SerializeField] CardTypes type;
    public void SetValue(float value)
    {
        this.value = value;
    }
    public void SetType(CardTypes type)
    {
        this.type = type;
        switch (type)
        {
            case CardTypes.Meat:
                _typeOfDebuff = DebuffTypes.WeaknessMeat;
                break;
            case CardTypes.Vegetarian:
                _typeOfDebuff = DebuffTypes.WeaknessVegg;
                break;
            case CardTypes.Mix:
            case CardTypes.Neutral:
                _typeOfDebuff = DebuffTypes.WeaknessMix;
                break;
            default:
                _typeOfDebuff = DebuffTypes.WeaknessMeat;
                Debug.Log("You can only use food types!");
                break;
        }
    }
    public override void SetTarget(Customer target)
    {
        base.SetTarget(target);
        if (targetCustomer != null)
        {
            switch (type)
            {
                case CardTypes.Meat:
                    targetCustomer.MeatFoodDefenceBonus += value;
                    break;
                case CardTypes.Vegetarian:
                    targetCustomer.VeggieFoodDefenceBonus += value;
                    break;
                case CardTypes.Mix:
                case CardTypes.Neutral:
                    targetCustomer.GeneralFoodDefenceBonus += value;
                    break;
                default:
                    Debug.Log("You can only use food types!");
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
                case CardTypes.Meat:
                    targetPlayer.MeatFoodModBonus += value;
                    break;
                case CardTypes.Vegetarian:
                    targetPlayer.VegetarianFoodModBonus += value;
                    break;
                case CardTypes.Mix:
                case CardTypes.Neutral:
                    targetPlayer.GeneralFoodModBonus += value;
                    break;
                default:
                    Debug.Log("You can only use food types!");
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
                case CardTypes.Meat:
                    targetPlayer.MeatFoodModBonus -= value;
                    break;
                case CardTypes.Vegetarian:
                    targetPlayer.VegetarianFoodModBonus -= value;
                    break;
                case CardTypes.Mix:
                case CardTypes.Neutral:
                    targetPlayer.GeneralFoodModBonus -= value;
                    break;
                default:
                    Debug.Log("You can only use food types!");
                    break;
            }
        }
        if (targetCustomer != null)
        {
            switch (type)
            {
                case CardTypes.Meat:
                    targetCustomer.MeatFoodDefenceBonus -= value;
                    break;
                case CardTypes.Vegetarian:
                    targetCustomer.VeggieFoodDefenceBonus -= value;
                    break;
                case CardTypes.Mix:
                case CardTypes.Neutral:
                    targetCustomer.GeneralFoodDefenceBonus -= value;
                    break;
                default:
                    Debug.Log("You can only use food types!");
                    break;
            }
        }
        base.EndEffect();
    }

}
