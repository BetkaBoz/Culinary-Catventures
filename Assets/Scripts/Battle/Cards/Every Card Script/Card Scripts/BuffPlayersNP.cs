using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BuffPlayersNP", menuName = "Cards/Buff food power")]
public class BuffPlayersNp : Card, IChangeFoodPower
{
    [SerializeField] float amount;
    [SerializeField] string type;
    [SerializeField] GameObject buffable;
    public override void CardEffect(GameManager gm, RaycastHit2D hit)
    {
        ChangeFoodPower(gm,amount, type);
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
