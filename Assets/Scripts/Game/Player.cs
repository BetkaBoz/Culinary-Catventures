using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    #region Private Vars
    [SerializeField] float generalFoodModBase;
    [SerializeField] float meatFoodModBase;
    [SerializeField] float vegetarianFoodModBase;
    [SerializeField] float generalFoodModBonus;
    [SerializeField] float meatFoodModBonus;
    [SerializeField] float vegetarianFoodModBonus;
    [SerializeField] List<Card> deck = new List<Card>();
    private string className;
    private int reputation = 0;
    private int money = 100 ;
    private int score;
    private int maxEnergy = 5;
    private int energy;
    private int maxRep = 100;
    private int rep = 100;
    
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
            return generalFoodModBase + generalFoodModBonus;
        }
    }
    public float GeneralFoodModBonus
    {
        get
        {
            return generalFoodModBonus;
        }
        set
        {
            generalFoodModBonus = value;
        }
    }
    public float MeatFoodMod
    {
        get
        {
            return meatFoodModBase+meatFoodModBonus;
        }
    }
    public float MeatFoodModBonus
    {
        get
        {
            return meatFoodModBonus;
        }
        set
        {
            meatFoodModBonus = value;
        }
    }
    public float VegetarianFoodMod
    {
        get
        {
            return vegetarianFoodModBase + vegetarianFoodModBonus;
        }
    }
    public float VegetarianFoodModBonus
    {
        get
        {
            return vegetarianFoodModBonus;
        }
        set
        {
            vegetarianFoodModBonus = value;
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
    public int MaxRep
    {
        get
        {
            return maxRep;
        }
        set
        {
            maxRep = value;
        }
    }
    public int Rep
    {
        get => rep;
        set => rep = value;
    }
    public int Money
    {
        get => money;
        set => money = value;
    }
    #endregion
    
    private void LoadPlayer()
    {
        Player data = this; //just some bs so intelisense works
        this.className = data.className;
        this.reputation = data.reputation;
        this.money = data.money;
        this.score = data.score;
        this.generalFoodModBase = data.generalFoodModBase;
        this.generalFoodModBonus = data.generalFoodModBonus;
        this.meatFoodModBase = data.meatFoodModBase;
        this.meatFoodModBonus = data.meatFoodModBonus;
        this.vegetarianFoodModBase = data.vegetarianFoodModBase;
        this.vegetarianFoodModBonus = data.vegetarianFoodModBonus;
    }

    public void ChangeMoney(int amount)
    {
        money += amount;
        if (money <= 0)
        {
            Die(false);
        }
    }
    
    //CHANGED -= TO +=, USE THIS INSTEAD
    public void ChangeReputation(int amount)
    {
        rep += amount;
        if (rep <= 0)
        {
            Die(true);
        }
    }
    public void TakeDamage(int amount)
    {
        rep -= amount;
        if (rep <= 0)
        {
            Die(true);
        }
    }

    public void Die(bool status)
    {
        Debug.Log("Oh nou I'm ded :(");
    }
}
