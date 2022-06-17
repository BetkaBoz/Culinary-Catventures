using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyDeliveryHandler : MonoBehaviour
{
    [SerializeField] GameManager gm;
    [SerializeField] Card cardPrefab;
    [SerializeField] CardBaseInfo[] meatIngredients;
    [SerializeField] CardBaseInfo[] veggieIngredients;
    [SerializeField] CardBaseInfo[] mixIngredients;
    [SerializeField] GameObject layer;
    int npLimit;

    void Awake()
    {
        npLimit = GameObject.FindGameObjectsWithTag("Customer")[0].GetComponent<Customer>().MaxHunger * 2;
    }

    public void OpenDelivery()
    {
        if (gm.discardPhase || gm.combinePhase) return;
        if (!gm.hasCardBeenPlayed)
            layer.SetActive(true);
    }

    public void GenerateMeat()
    {
        GenerateCards(meatIngredients);
    }

    public void GenerateVeggies()
    {
        GenerateCards(veggieIngredients);
    }

    public void GenerateMixt()
    {
        GenerateCards(mixIngredients);
    }

    private void GenerateCards(CardBaseInfo[] selectedPack)
    {
        int currentNP = 0;
        List<Card> generatedCards = new List<Card>();
        int maxRange = selectedPack.Length;
        while (currentNP < npLimit)
        {
            int idx = Random.Range(0, maxRange);
            Card newCard = Instantiate(cardPrefab);
            newCard.GetDataFromBase(selectedPack[idx]);
            Debug.Log(newCard.CardName);
            currentNP += newCard.NutritionPoints;
            newCard.DeleteOnBattleEnd = true;
            generatedCards.Add(newCard);
        }
        gm.AddCardsToDeck(generatedCards);
        layer.SetActive(false);
        gm.EndPlayersTurn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
