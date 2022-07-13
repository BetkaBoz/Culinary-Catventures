using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chungus : Customer
{
    public List<Sprite> sprites;

    public override string Name => throw new System.NotImplementedException();

    public override bool EndTurn()
    {
        //turnsUntilAngry--;
        //if (numTurnsStunned > 0) numTurnsStunned--;

        //ChangeExpressions();
        ////change customers expressions based on how many turns are left till end of the battle

        ////when there are 0 turns left, check how much was customer satisfied
        ////and based on this info add money and reputation to the player
        ////or cause reputation demage on player
        //if (turnsUntilAngry == 0)
        //{
        //    if (currHunger >= maxHunger / 2)
        //    {
        //        gm.Player.TakeDamage(50);
        //        finalMoney -= money + 40;
        //    }
        //    else if (currHunger >= maxHunger / 3)
        //    {
        //        finalMoney += money + 10;
        //        finalRep += rep + 10;
        //    }
        //    else if (currHunger >= maxHunger / 4)
        //    {
        //        finalMoney += money + 25;
        //        finalRep += rep + 25;
        //    }
        //    Die(true);
        //    gm.Player.ChangeMoney(finalMoney);
        //    gm.Player.earnedRep += finalRep;
        //    //gm.Player.ChangeReputation(rep);
        //    return true;
        //}
        //satisfied = false;
        return false;
    }

    public override void ChangeExpressions()
    {
        //if (turnsUntilAngry >= turns - 2)
        //    Faces.DOFade(1, 0.2f).OnPlay(() => { Faces.sprite = sprites[0]; });
        //else if (turnsUntilAngry >= turns / 2)
        //    Faces.DOFade(1, 0.2f).OnPlay(() => { Faces.sprite = sprites[1]; });
        //else if (turnsUntilAngry >= turns / 3)
        //    Faces.DOFade(1, 0.2f).OnPlay(() => { Faces.sprite = sprites[2]; });
        //else if (turnsUntilAngry >= 1)
        //    Faces.DOFade(1, 0.2f).OnPlay(() => { Faces.sprite = sprites[3]; });

    }
}
    
