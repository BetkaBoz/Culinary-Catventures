using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour, IDamageable
{
    // TODO: script should be renamed to PlayerData, PlayerDataSingleton or similiar to convey it only carries variables
    //  ^in case of name change, the Player tag should probably also be renamed

    #region Private Vars

    public static Player Instance { get; set; }
    [SerializeField] float generalFoodModBase;
    [SerializeField] float meatFoodModBase;
    [SerializeField] float vegetarianFoodModBase;
    [SerializeField] float generalFoodModBonus;
    [SerializeField] float meatFoodModBonus;
    [SerializeField] float vegetarianFoodModBonus;
    [SerializeField] List<CardBaseInfo> deck = new List<CardBaseInfo>();
    [SerializeField] Customer customer;
    public bool isDead;
    public bool isVictorious;
    string className;
    // int reputation = 0;
    int money;
    int maxEnergy = 5;
    int energy;
    int maxRep = 150;

    int currExp = 400;
    int nextLvl = 1000;
    public List<string> helpers = new List<string>();
    public int moneyAmount, repAmount, earnedRep;
    public int rep = 150;

    #endregion

    public bool canCallEmergencyDelivery = true;

    #region Getters/Setters

    public int CurrentExp
    {
        get { return currExp; }
        set { currExp = value; }
    }

    public int NextLevel
    {
        get { return nextLvl; }
    }

    public string ClassName
    {
        get { return className; }
        set { className = value; }
    }

    public int Money
    {
        get { return money; }
        set { money = value; }
    }

    public float GeneralFoodMod
    {
        get { return generalFoodModBase + generalFoodModBonus; }
    }

    public float GeneralFoodModBonus
    {
        get { return generalFoodModBonus; }
        set { generalFoodModBonus = value; }
    }

    public float MeatFoodMod
    {
        get { return meatFoodModBase + meatFoodModBonus; }
    }

    public float MeatFoodModBonus
    {
        get { return meatFoodModBonus; }
        set { meatFoodModBonus = value; }
    }

    public float VegetarianFoodMod
    {
        get { return vegetarianFoodModBase + vegetarianFoodModBonus; }
    }

    public float VegetarianFoodModBonus
    {
        get { return vegetarianFoodModBonus; }
        set { vegetarianFoodModBonus = value; }
    }

    public List<CardBaseInfo> Deck
    {
        get { return deck; }
        set { deck = value; }
    }

    public int MaxEnergy
    {
        get { return maxEnergy; }
        set { maxEnergy = value; }
    }

    public int Energy
    {
        get { return energy; }
        set { energy = value; }
    }

    public int MaxRep
    {
        get { return maxRep; }
        set { maxRep = value; }
    }

    public int MoneyAmount => moneyAmount;
    public int RepAmount => repAmount;
    public List<string> Helpers => helpers;

    #endregion

    public void Awake()
    {
        // singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            isDead = false;
            isVictorious = false;

            DontDestroyOnLoad(this.gameObject);
            // StartCoroutine(Test());
        }
    }

    // // TODO: Testing method for loading scenes after 10 seconds, DELETE WHEN NOT NEEDED
    // IEnumerator Test()
    // {
    //     yield return new WaitForSeconds(10);
    //     SceneManager.LoadScene(3);
    // }

    void LoadPlayer()
    {
        Player data = this; //just some bs so intelisense works
        this.className = data.className;
        this.rep = data.rep;
        this.money = data.money;
        //this.score = data.score;
        this.generalFoodModBase = data.generalFoodModBase;
        this.generalFoodModBonus = data.generalFoodModBonus;
        this.meatFoodModBase = data.meatFoodModBase;
        this.meatFoodModBonus = data.meatFoodModBonus;
        this.vegetarianFoodModBase = data.vegetarianFoodModBase;
        this.vegetarianFoodModBonus = data.vegetarianFoodModBonus;
    }

    public void ChangeMoney(int amount)
    {
        moneyAmount += amount;
        money += amount;

        if (money < 0)
        {
            money = 0;
        }
    }

    //CHANGED -= TO +=, USE THIS INSTEAD
    public void ChangeReputation(int amount)
    {
        repAmount += amount;
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

    public bool HaveMoney(int amount)
    {
        return money >= amount;
    }


    public void Die(bool status)
    {
        if (isDead || isVictorious) return;
        isDead = true;
        StartCoroutine(LoadGameOver());
    }

    void Win()
    {
        if (isDead || isVictorious) return;
        isVictorious = true;
        StartCoroutine(LoadBattleWon());
    }

    public void CheckCondition()
    {
        if (!isDead) Win();
    }

    IEnumerator LoadGameOver()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game Over", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Debug.Log("Oh nou I'm ded :(");
    }

    IEnumerator LoadBattleWon()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Battle Won", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Debug.Log("Yay I won :)");
    }

    //AK NIE JE POUZITY PARAMETER TAK JE TO NAHODNA KARTA
    public void RemoveCardFromDeck(string cardName = null)
    {
        if (!deck.Any())
        {
            return;
        }

        if (string.IsNullOrEmpty(cardName))
        {
            CardBaseInfo card = deck[Random.Range(0, deck.Count)];
            deck.Remove(deck.Find(x => x.CardName == card.CardName));

            Debug.Log(card.CardName);
            return;
        } /*
        else
        {
            card = deck.Find(x => x.CardName == cardName);
        }*/

        deck.Remove(deck.Find(x => x.CardName == cardName));
        //return card;
    }

    public CardBaseInfo FindCardFromDeck(string cardName = null)
    {
        if (!deck.Any())
        {
            Debug.Log("EMPTY DECK");
            return null;
        }

        CardBaseInfo card = null;

        //NAJDI NAHODNU INGREDIENCIU Z DECKU
        if (string.IsNullOrEmpty(cardName))
        {
            foreach (CardBaseInfo tmpCard in deck)
            {
                if (tmpCard.CardType == "Vegetarian" || tmpCard.CardType == "Meat" || tmpCard.CardType == "Mix")
                {
                    card = tmpCard;
                    return card;
                }
            }
        }
        //NAJDI VYBRANU KARTU Z DECKU
        else
        {
            card = deck.Find(x => x.CardName == cardName);
        }


        return card;
        //return card;
    }

    public bool CheckIfDeckHasIngredient()
    {
        if (!Deck.Any())
        {
            Debug.Log("EMPTY DECK");
            return false;
        }

        foreach (CardBaseInfo tmpCard in deck)
        {
            if (tmpCard.CardType == "Vegetarian" || tmpCard.CardType == "Meat" || tmpCard.CardType == "Mix")
            {
                return true;
            }
        }

        return false;
    }
    // async Task LoadBattleWon()
    //{
    //    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Battle Won", LoadSceneMode.Additive);
    //    while (!asyncLoad.isDone)
    //        await Task.Delay(10);
    //    Debug.Log("Yay I won :)");
    //}
}
