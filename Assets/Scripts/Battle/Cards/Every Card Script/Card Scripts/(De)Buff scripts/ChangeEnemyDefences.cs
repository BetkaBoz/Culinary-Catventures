using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEnemyDefences : CardEffectWithDebuff
{
    [SerializeField] CardTypes type;
    public override void Effect(GameManager gm, RaycastHit2D hit)
    {
        Customer target = hit.transform.GetComponent<CustomerView>().Customer;
        ChangeFoodPower(gm, target, (float)amount / 100, type);
    }

    public void ChangeFoodPower(GameManager gm, Customer target, float amount, CardTypes type)
    {
        GameObject temp = Instantiate(_buffable);
        ChangeFoodMod tempBuff = temp.GetComponent<ChangeFoodMod>();
        tempBuff.SetType(type);
        tempBuff.SetValue(amount);
        tempBuff.increaseTimer(_duration);
        gm.SetBuff(tempBuff, target);
    }
}
