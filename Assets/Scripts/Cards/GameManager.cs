using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Card> deck = new List<Card>();
    [SerializeField] private List<Card> discardPile = new List<Card>();
    [SerializeField] private Transform[] cardSlots;
    [SerializeField] private bool[] availableCardSlots;

    private bool DrawCard()
    {
        Card randCard = deck[Random.Range(0, deck.Count)];

            for (int i = 0; i < availableCardSlots.Length; i++){
                if (availableCardSlots[i]){
                    randCard.gameObject.SetActive(true);
                    randCard.transform.position = cardSlots[i].position;
                    randCard.SetIndex(i);
                    randCard.SetHasBeenPlayed(false);
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
                Debug.Log("Couldn't draw a card. The deck is empty!"); //TODO: create reshuffle function
                Shuffle();
                return;
            }
        }
        Debug.Log("I DID IT! :)");
    }

    public void SendToDiscard(Card card)
    {
        discardPile.Add(card);
        availableCardSlots[card.GetIndex()] = true;
    }

    private void Shuffle()
    {
        deck = new List<Card>(discardPile);
        discardPile.Clear();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            DrawCards(5);
        }
    }
}
