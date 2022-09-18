using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "New Card", menuName = "CardBase")]
public class CardBaseInfo : ScriptableObject
{
    #region SerializeFields
    [SerializeField] private Sprite artwork;
    [SerializeField] private List<CardEffect> cardEffect;
    [SerializeField] private string cardName;
    [SerializeField] private CardTypes cardType;
    [SerializeField] private bool canTarget = false;
    [SerializeField] private int energyCost;
    [SerializeField] private int nutritionPoints;
    //[SerializeField] private int amount;
    //[SerializeField] private int discardAmount;
    #endregion
    #region Getters
    public Sprite Artwork
    {
        get
        {
            return artwork;
        }
    }
    public List<CardEffect> CardEffect
    {
        get
        {
            return cardEffect;
        }
    }
    public string CardName
    {
        get
        {
            return cardName;
        }
    }
    public CardTypes CardType
    {
        get
        {
            return cardType;
        }
    }
    public bool CanTarget
    {
        get
        {
            return canTarget;
        }
    }
    public int EnergyCost
    {
        get
        {
            return energyCost;
        }
    }
    public int NutritionPoints
    {
        get
        {
            return nutritionPoints;
        }
    }
    //public int Amount
    //{
    //    get
    //    {
    //        return amount;
    //    }
    //}
    //public int DiscardAmount
    //{
    //    get
    //    {
    //        return discardAmount;
    //    }
    //}
    #endregion
}
