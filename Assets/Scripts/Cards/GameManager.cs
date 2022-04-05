using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Card> deck = new List<Card>();
    [SerializeField] private List<Card> discardPile = new List<Card>();
    [SerializeField] private List<Card> hand = new List<Card>();
    [SerializeField] private List<Customer> customers = new List<Customer>();
    [SerializeField] private Transform[] cardSlots;
    [SerializeField] private bool[] availableCardSlots;
    private int maxEnergy = 3;
    private int energy;


    private void Start()
    {
        DrawCards(5);
        AddEnergy(maxEnergy);
    }

    private void AddEnergy(int amount)
    {
        energy += amount;
        Debug.Log(energy.ToString() + " / " + maxEnergy.ToString());
    }

    public bool SpendEnergy(int amount)
    {
        if(energy-amount < 0)
        {
            return false;
        }
        else
        {
            energy -= amount;
            Debug.Log(energy.ToString() + " / " + maxEnergy.ToString());
            return true;
        }
    }

    private bool DrawCard()
    {
        Card randCard = deck[Random.Range(0, deck.Count)];

        for (int i = 0; i < availableCardSlots.Length; i++){
            if (availableCardSlots[i]){
                randCard.gameObject.SetActive(true);
                randCard.transform.position = cardSlots[i].transform.position;
                randCard.SetIndex(i);
                randCard.SetHasBeenPlayed(false);
                hand.Add(randCard);
                availableCardSlots[i] = false;
                deck.Remove(randCard);
                return true;
            }
        }
        return false;
    }

    public void DrawCards(int count)
    {
        for(int i = 0; i < count; i++)
        {
            if (deck.Count >= 1)
            {
                if (!DrawCard())
                {
                    Debug.Log("Couldn't draw a card. The hand is full!");
                    return;
                }
            }
            else
            {
                Debug.Log("Couldn't draw a card. The deck is empty!"); 
                Shuffle();
                i--;
            }
        }
        Debug.Log("I DID IT! :)");
    }

    public void SendToDiscard(Card card, bool isEndTurn)
    {
        int idx = card.GetIndex();
        availableCardSlots[idx] = true;
        card.SetIndex(-1);
        card.SetHasBeenPlayed(false);
        if (!isEndTurn){
            hand.Remove(card);
        }
        discardPile.Add(card);
    }

    private void Shuffle()
    {
        deck = new List<Card>(discardPile);
        discardPile.Clear();
    }

    public void EndTurn()
    {
        foreach (var card in hand)
        {
            card.MoveToDiscardPile(true);
        }
        foreach (var customer in customers)
        {
            customer.EndTurn();
        }
        hand.Clear();
        DrawCards(5);
        SpendEnergy(energy);
        AddEnergy(maxEnergy);
    }
}
