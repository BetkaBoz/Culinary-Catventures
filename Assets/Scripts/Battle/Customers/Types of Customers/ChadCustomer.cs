using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChadCustomer : Customer
{
    public override void EndTurn()
    {
        turnsLeft--;
        if (numTurnsStunned > 0) numTurnsStunned--;

        //when there are 0 turns left, check how much was customer satisfied
        //and based on this info add money and reputation to the player
        //or cause reputation demage on player
        if (turnsLeft == 0)
        {
            if(currHunger == Data.MaxHunger) { gm.Player.TakeDamage(15); }
            else if (currHunger >= Data.MaxHunger / 2)
            {
                gm.Player.TakeDamage(Data.MaxDamageMade / Data.HalfHealthLeftPercentage);
            }
            else if (currHunger >= Data.MaxHunger / 3)
            {
                gm.Player.TakeDamage(Data.MaxDamageMade / Data.ThirdHealthLeftPercentage);
                finalMoney += Data.Money + 15;
            }
            else if (currHunger >= Data.MaxHunger / 4)
            {
                gm.Player.TakeDamage(Data.MaxDamageMade / Data.QuarterHealthLeftPercentage);
                finalMoney += Data.Money + 30;
            }
            Die(true);
            gm.Player.ChangeMoney(finalMoney);
            gm.Player.earnedRep += finalRep;
            return;
        }
        satisfied = false;
        base.EndTurn();
    }


}
