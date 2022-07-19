using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chungus : Customer
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
            if (currHunger >= Data.MaxHunger / 2)
            {
                gm.Player.TakeDamage(50);
                finalMoney -= 50;
            }
            else if (currHunger >= Data.MaxHunger / 3)
            {
                finalMoney += Data.Money + 10;
                finalRep += Data.Reputation + 10;
            }
            else if (currHunger >= Data.MaxHunger / 4)
            {
                finalMoney += Data.Money + 25;
                finalRep += Data.Reputation + 25;
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
    
