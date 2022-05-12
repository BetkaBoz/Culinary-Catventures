using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Card : ScriptableObject
{
    #region SerializeFields
    private int handIndex = -1;
    [SerializeField] private string cardName;
    [SerializeField] private int energyCost;
    [SerializeField] private bool canTarget = false;
    [SerializeField] private Sprite artwork;
    [SerializeField] private int nutritionPoints;
    [SerializeField] private string cardType;
    #endregion
    #region Getters/Setters
    public string CardType
    {
        get
        {
            return cardType;
        }
    }
    public int NutritionPoints
    {
        get
        {
            return nutritionPoints;
        }
        set
        {
            nutritionPoints = value;
        }
    }
    public int HandIndex
    {
        get
        {
            return handIndex;
        }
        set
        {
            handIndex = value;
        }
    }
    public string CardName
    {
        get
        {
            return cardName;
        }
    }
    public int EnergyCost
    {
        get
        {
            return energyCost;
        }
        set
        {
            energyCost = value;
        }
    }
    public bool CanTarget
    {
        get
        {
            return canTarget;
        }
    }
    public Sprite Artwork
    {
        get
        {
            return artwork;
        }
    }
    #endregion
    public abstract void CardEffect(GameManager gm, RaycastHit2D hit);
}
