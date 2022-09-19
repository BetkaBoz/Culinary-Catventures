using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsTooltips : MonoBehaviour
{
    [SerializeField] List<Hoverable> hoverableDebuffs;
    string message, header;
    DebuffTypes _debuffTypes;

    public void EffectChange(DebuffTypes debuffTypes, int i)
    {
        switch (debuffTypes)
        {
            case DebuffTypes.Stun:
                message = "Customer will be unable to perform their action for a turn. Stacking of this effect should increase the number of turns this effect lasts.";
                header = "STUN";
                break;
            case DebuffTypes.Captivate:
                message = "Prevents customer from leaving. Stacking of this effect increases the number of turns this effect lasts.";
                header = "CAPTIVATE";
                break;
            case DebuffTypes.Flavourful:
                message = "Customer's Hunger will get decreased by X at the start of their turn, then X will decrease by 1. Stacking of this effect should increase X.";
                header = "FLAVORFUL";
                break;
            case DebuffTypes.WeaknessMeat:
                message = "Any Meat only food used on customer will decrease Hunger by additional 25%. Stacking of this effect should increase the number of turns this effect lasts.";
                header = "WEAKNESS: Meat";
                break;
            case DebuffTypes.WeaknessVegg:
                message = "Any Mixed food used on customer will decrease Hunger by additional 25%. Stacking of this effect should increase the number of turns this effect lasts.";
                header = "WEAKNESS: Vegetables";
                break;
            case DebuffTypes.WeaknessMix:
                message = "Any Vegetarian food used on customer will decrease Hunger by additional 25%. Stacking of this effect should increase the number of turns this effect lasts.";
                header = "WEAKNESS: Mix";
                break;
        }
        hoverableDebuffs[i].SetMessageHeader(message, header);
    }
}
