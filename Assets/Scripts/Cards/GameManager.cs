using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player player;
    private List<Card> deck;
    [SerializeField] private List<Card> discardPile = new List<Card>();
    [SerializeField] private List<Card> hand = new List<Card>();
    [SerializeField] private List<Customer> customers = new List<Customer>();
    [SerializeField] private CardSlot[] cardSlots;
    [SerializeField] private bool[] availableCardSlots;
    [SerializeField] private Text energyUI;
    [SerializeField] private DiscardController discardController;
    public bool discardPhase;
    //public Player Player
    //{
    //    get
    //    {
    //        return player;
    //    }
    //}


    private void Start()
    {
        deck = new List<Card>(player.Deck);
        DrawCards(5);
        AddEnergy(player.MaxEnergy);
    }

    private void AddEnergy(int amount)
    {
        player.Energy += amount;
        energyUI.text = player.Energy.ToString() + " / " + player.MaxEnergy.ToString();
    }

    public bool SpendEnergy(int amount)
    {
        if(player.Energy - amount < 0)
        {
            return false;
        }
        else
        {
            player.Energy -= amount;
            energyUI.text = player.Energy.ToString() + " / " + player.MaxEnergy.ToString();
            return true;
        }
    }

    private bool DrawCard()
    {
        Card randCard = deck[Random.Range(0, deck.Count)];

        for (int i = 0; i < availableCardSlots.Length; i++){
            if (availableCardSlots[i]){
                //randCard.gameObject.SetActive(true);
                //randCard.transform.position = cardSlots[i].transform.position;
                cardSlots[i].SetHasBeenPlayed(false);
                cardSlots[i].gameObject.SetActive(true);
                cardSlots[i].SetCard(randCard);
                randCard.HandIndex = i;
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
        int idx = card.HandIndex;
        availableCardSlots[idx] = true;
        card.HandIndex = -1;
        //card.SetHasBeenPlayed(false);
        cardSlots[idx].gameObject.SetActive(false);
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
            //card.MoveToDiscardPile(true);
            SendToDiscard(card, true);
        }
        foreach (var customer in customers)
        {
            customer.EndTurn();
        }
        hand.Clear();
        DrawCards(5);
        SpendEnergy(player.Energy);
        AddEnergy(player.MaxEnergy);
    }

    public void StartDiscard()
    {
        discardPhase = true;
        discardController.gameObject.SetActive(true);
        for(int i = 0; i < cardSlots.Length; i++)
        {
            cardSlots[i].Deselect();
        }
    }

    public void StopDiscard()
    {
        discardPhase = false;
        discardController.gameObject.SetActive(false);
    }

    public void SetCard(CardSlot card)
    {
        if (discardPhase)
        {
            discardController.SelectCard(card);
        }
    }
}
