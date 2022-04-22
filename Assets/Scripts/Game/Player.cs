using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Private Vars
    private string className;
    private int reputation = 0;
    private int money = 0;
    private int score;
    private float generalFoodMod;
    private float meatFoodMod;
    private float vegetarianFoodMod;
    private int maxEnergy = 3;
    private int energy;
    [SerializeField] private List<Card> deck = new List<Card>();
    #endregion

    #region Getters/Setters
    public string ClassName
    {
        get
        {
            return className;
        }
        set
        {
            className = value;
        }
    }
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
        }
    }
    public float GeneralFoodMod
    {
        get
        {
            return generalFoodMod;
        }
        set
        {
            generalFoodMod = value;
        }
    }
    public float MeatFoodMod
    {
        get
        {
            return meatFoodMod;
        }
        set
        {
            meatFoodMod = value;
        }
    }
    public float VegetarianFoodMod
    {
        get
        {
            return vegetarianFoodMod;
        }
        set
        {
            vegetarianFoodMod = value;
        }
    }
    public List<Card> Deck
    {
        get
        {
            return deck;
        }
        set
        {
            deck = value;
        }
    }
    public int MaxEnergy
    {
        get
        {
            return maxEnergy;
        }
        set
        {
            maxEnergy = value;
        }
    }
    public int Energy
    {
        get
        {
            return energy;
        }
        set
        {
            energy = value;
        }
    }
    #endregion

    private void LoadPlayer()
    {
        Player data = this; //just some bs so intelisense works
        this.className = data.className;
        this.reputation = data.reputation;
        this.money = data.money;
        this.score = data.score;
        this.generalFoodMod = data.generalFoodMod;
        this.meatFoodMod = data.meatFoodMod;
        this.vegetarianFoodMod = data.vegetarianFoodMod;
    }

    public bool changeReputation(int amount)
    {
        this.reputation += amount;
        if (this.reputation <= 0)
        {
            return true; //true = Player is dead
        }
        else
        {
            return false; //false = player still lives
        }
    }

    public bool changeMoney(int amount)
    {
        this.money += amount;
        if (this.money <= 0)
        {
            return true; //true = Player is dead
        }
        else
        {
            return false; //false = player still lives
        }
    }
}
