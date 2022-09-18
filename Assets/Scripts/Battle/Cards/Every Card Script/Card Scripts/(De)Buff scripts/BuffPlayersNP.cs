using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffPlayersNP : CardEffectWithDebuff, IChangeFoodPower
{
    [SerializeField] CardTypes type;
    public override void Effect(GameManager gm, RaycastHit2D hit)
    {
        ChangeFoodPower(gm,(float)amount/100, type);
    }

    public void ChangeFoodPower(GameManager gm, float amount, CardTypes type)
    {
        GameObject temp = Instantiate(_buffable);
        ChangeFoodMod tempBuff = temp.GetComponent<ChangeFoodMod>();
        tempBuff.SetType(type);
        tempBuff.SetValue(amount);
        tempBuff.increaseTimer(_duration);
        gm.SetBuff(tempBuff);
    }
}
