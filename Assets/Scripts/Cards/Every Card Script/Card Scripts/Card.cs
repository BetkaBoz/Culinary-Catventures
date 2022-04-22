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
    #endregion
    public int NutritionPoints
    {
        get
        {
            return nutritionPoints;
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

    public abstract void CardEffect(GameManager gm, RaycastHit2D hit);
}
