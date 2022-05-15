using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { PLAYERTURN, ENEMYTURN, WON, LOST }
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
    [SerializeField] private TextMeshProUGUI energyUI;
    [SerializeField] private DiscardController discardController;
    [SerializeField] private CombineController combineController;
    public bool discardPhase;
    public bool combinePhase;
    public BattleState battleState;


    //public Player Player
    //{
    //    get
    //    {
    //        return player;
    //    }
    //}

    public Player Player => player;      //getter

    private void Start()
    {
        if (customers.Count == 0)
        {
            //if player is not dead then you won
            battleState = BattleState.WON;
            Debug.Log("YAY YOU WON!");
            //else you lose and its end of game
        }
            
        else
        {
            deck = new List<Card>(player.Deck);
            for (int i = 0; i < cardSlots.Length; i++)
            {
                cardSlots[i].Hide(true);
            }
            DrawCards(5);
            AddEnergy(player.MaxEnergy);
            combinePhase = false;
            discardPhase = false;

            battleState = BattleState.PLAYERTURN;
        }
    }

    public void AddEnergy(int amount)
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

    //----------------- TURN-BASE RELATED CODE ---------------------
    public void EndPlayersTurn()
    {
        DiscardHand();
        // List<Customer> deadQueue = new List<Customer>();
        foreach (var customer in customers)
        {
            customer.StartTurn();
        }
        // customers = customers.Except(deadQueue).ToList();
        hand.Clear();
        
        SpendEnergy(player.Energy);
        AddEnergy(player.MaxEnergy);

        battleState = BattleState.ENEMYTURN;
        Debug.Log("ITS ENEMIES TURN");
    }

    public void EndEnemyTurn()
    {
        DrawCards(5);

        foreach (var customer in customers)
        {
            customer.EndTurn();
        }

        battleState = BattleState.PLAYERTURN;
        Debug.Log("ITS PLAYERS TURN");
    }

    // ----------------- CARDS RELATED CODE ---------------------
    public void HideCardSlot(int idx, bool isHidden)
    {
        cardSlots[idx].Hide(isHidden);
    }

    private bool DrawCard()
    {
        Card randCard = deck[Random.Range(0, deck.Count)];

        for (int i = 0; i < availableCardSlots.Length; i++){
            if (availableCardSlots[i]){
                //randCard.gameObject.SetActive(true);
                //randCard.transform.position = cardSlots[i].transform.position;
                cardSlots[i].SetHasBeenPlayed(false);
                HideCardSlot(i, false);
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

    public void SendToDiscard(int idx, bool isEndTurn)
    {
        ////Debug.Log("idx:"+idx);
        Card card = cardSlots[idx].GetCard();
        availableCardSlots[idx] = true;
        card.HandIndex = -1;
        //card.SetHasBeenPlayed(false);
        HideCardSlot(idx, true);
        if (!isEndTurn){
            hand.Remove(card);
        }
        discardPile.Add(card);
    }

    public void SendToExhaust(int idx)
    {
        Card card = cardSlots[idx].GetCard();
        availableCardSlots[idx] = true;
        card.HandIndex = -1;
        //card.SetHasBeenPlayed(false);
        HideCardSlot(idx, true);
        hand.Remove(card);
        exhaustPile.Add(card);
    }

    private void Shuffle()
    {
        deck = new List<Card>(discardPile);
        discardPile.Clear();
    }

    public void DiscardHand()
    {
        foreach (var card in hand)
        {
            //card.MoveToDiscardPile(true);
            SendToDiscard(card.HandIndex, true);
        }
    }

    public void DeselectAllCards()
    {
        for (int i = 0; i < cardSlots.Length; i++)
        {
            cardSlots[i].Deselect();
        }
    }

    public async Task StartDiscard()
    {
        Debug.Log("Start Discard");
        discardController.ToggleDiscard();
        discardPhase = true;
        DeselectAllCards();
        while (discardPhase)
        {
            //Debug.Log("I'm yealding ova here! " + discardPhase.ToString());
            await Task.Yield();
        }
        Debug.Log("I'm done mate!");

        //StopDiscard();
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StopDiscard();
        }
    }

    public void StopDiscard()
    {
        Debug.Log("Stop Discard");
        discardController.ToggleDiscard();
        discardPhase = false;
    }

    public void SetDiscardFilter(string[] filter)
    {
        discardController.Filter = filter;
    }

    public void SetCard(CardSlot card)
    {
        if (discardPhase)
        {
            Debug.Log("SET");
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
        DeselectAllCards();
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
        List<string> types = new List<string>();
        Card card;

        for (int i = 0; i < cardSlots.Length; i++)
        {
            if (cardSlots[i].Selected)
            {
                card = cardSlots[i].GetCard();
                //if(card.CardType != "Manoeuvre")
                //{
                find.Add(cardSlots[i].GetCard().CardName);
                types.Add(cardSlots[i].GetCard().CardType);
                //}
            }
        }
        combineController.FindCard(find.ToArray(), types.ToArray());
    }

    public void CombineCards(Card target)
    {
        for (int i = 0; i < cardSlots.Length; i++)
        {
            if (cardSlots[i].Selected)
            {
                cardSlots[i].Deselect();
                SendToExhaust(i);
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

    public int GetComboNP(bool isPile)
    {
        int result = 0;
        for (int i = 0; i < cardSlots.Length; i++)
        {
            if (cardSlots[i].Selected)
            {
                result += cardSlots[i].GetCard().NutritionPoints;
            }
        }
        if (isPile)
        {
            return result + (int)(result * 0.25f);
        }
        else
        {
            return result + (int)(result * 0.5f);
        }
    }

    public void customerListDelete(Customer customer)
    {
        customers.Remove(customer);
    }
}
