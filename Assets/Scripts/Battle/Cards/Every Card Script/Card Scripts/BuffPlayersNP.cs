using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffPlayersNP : CardEffect, IChangeFoodPower
{
    [SerializeField] string type;
    [SerializeField] GameObject buffable;
    public override void Effect(GameManager gm, RaycastHit2D hit)
    {
        ChangeFoodPower(gm,(float)amount/100, type);
    }

    public void ChangeFoodPower(GameManager gm, float amount, string type)
    {
        GameObject temp = Instantiate(buffable);
        IncreaseFoodMod tempBuff = temp.GetComponent<IncreaseFoodMod>();
        tempBuff.SetType(type);
        tempBuff.SetValue(amount);
        gm.BuffPlayer(tempBuff,true);
    }
}
