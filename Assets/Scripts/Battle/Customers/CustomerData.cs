using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

[CreateAssetMenu(menuName = "Customers/Customer")]
public class CustomerData : ScriptableObject
{
    public string Name;
    public int Value;
    public int MaxHunger;
    public int Turns;
    public int Money;
    public int MaxMoneyEarned;
    public int Reputation;
    public int MaxReputationEarned;
    public int MaxDamageMade;
    public int HalfHealthLeftPercentage;
    public int ThirdHealthLeftPercentage;
    public int QuarterHealthLeftPercentage;
    public float VeggieFoodDefence = 1; //1.25 == 25% weakness to food, 0.75 == 25% resistance to food
    public float MeatFoodDefence = 1;
    public float GeneralFoodDefence = 1;
    public List<Sprite> Sprites;
    public List<Sprite> ActionSprites;
    public List<Sprite> DebuffSprites;
    public SpriteLibraryAsset AnimationSprites;

    [Header("Sprite position offset")]
    public float shadowOffset;
    public float actionOffset;
    public float spriteOffset;
}
