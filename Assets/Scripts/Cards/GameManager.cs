using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player player;
    private List<Card> deck;
    private List<Card> discardPile = new List<Card>();
    private List<Card> hand = new List<Card>();
    private List<Card> exhaustPile = new List<Card>();
    [SerializeField] private List<Customer> customers = new List<Customer>();
    [SerializeField] private CardSlot[] cardSlots;
    [SerializeField] private bool[] availableCardSlots;
    [SerializeField] private Text energyUI;
    [SerializeField] private DiscardController discardController;
    [SerializeField] private CombineController combineController;
    public bool discardPhase;
    public bool combinePhase;
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
        combinePhase = false;
        discardPhase = false;
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

    public void SendToExhaust(Card card)
    {
        int idx = card.HandIndex;
        availableCardSlots[idx] = true;
        card.HandIndex = -1;
        //card.SetHasBeenPlayed(false);
        cardSlots[idx].gameObject.SetActive(false);
        hand.Remove(card);
        exhaustPile.Add(card);
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
        //foreach (var customer in customers)
        //{
        //    customer.EndTurn();
        //}
        hand.Clear();
        DrawCards(5);
        SpendEnergy(player.Energy);
        AddEnergy(player.MaxEnergy);
    }

    public async Task StartDiscard()
    {
        Debug.Log("Start Discard");
        discardController.ToggleDiscard();
        discardPhase = true;
        for (int i = 0; i < cardSlots.Length; i++)
        {
            cardSlots[i].Deselect();
        }
        while (discardPhase)
        {
            Debug.Log("I'm yealding ova here!");
            await Task.Yield();
        }
        Debug.Log("I'm done mate!");

        //StopDiscard();
    }

    public void StopDiscard()
    {
        Debug.Log("Stop Discard");
        discardController.ToggleDiscard();
        discardPhase = false;
    }

    public void SetCard(CardSlot card)
    {
        if (discardPhase)
        {
            discardController.SelectCard(card);
        }
    }

    public void ToggleCombine()
    {
        if (combinePhase == true)
        {
            StopCombine();
        }
        else
        {
            StartCombine();
        }
    }

    private void StartCombine()
    {
        Debug.Log("Start Combine");
        combineController.ToggleCombine();
        combinePhase = true;
        for (int i = 0; i < cardSlots.Length; i++)
        {
            cardSlots[i].Deselect();
        }
    }

    private void StopCombine()
    {
        Debug.Log("Stop Combine");
        combineController.ToggleCombine();
        combinePhase = false;
    }

    public void FindCombineTarget()
    {
        List<string> find = new List<string>();

        for (int i = 0; i < cardSlots.Length; i++)
        {
            if (cardSlots[i].Selected)
            {
                find.Add(cardSlots[i].GetCard().CardName);
            }
        }
        combineController.FindCard(find.ToArray());
    }

    public void CombineCards(Card target)
    {
        for (int i = 0; i < cardSlots.Length; i++)
        {
            if (cardSlots[i].Selected)
            {
                cardSlots[i].Deselect();
                SendToExhaust(cardSlots[i].GetCard());
            }
        }
        for (int i = 0; i < availableCardSlots.Length; i++)
        {
            if (availableCardSlots[i])
            {
                cardSlots[i].SetHasBeenPlayed(false);
                cardSlots[i].gameObject.SetActive(true);
                cardSlots[i].SetCard(target);
                target.HandIndex = i;
                hand.Add(target);
                availableCardSlots[i] = false;
                break;
            }
        }
        ToggleCombine();
    }
}
