using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum BattleState { PLAYERTURN, ENEMYTURN, WON, LOST }
public class GameManager : MonoBehaviour
{
    #region SerializeFields
    [SerializeField] Player player;
    [SerializeField] CardSlot[] cardSlots;
    [SerializeField] CardSlot highlightSlot;
    [SerializeField] bool[] availableCardSlots;
    [SerializeField] TextMeshProUGUI energyUI;
    [SerializeField] TextMeshProUGUI repUI;
    [SerializeField] DiscardController discardController;
    [SerializeField] CombineController combineController;
    [SerializeField] Card[] cardPrefabs;
    [SerializeField] public List<Card> deck;
    [SerializeField] public List<Card> discardPile = new List<Card>();
    [SerializeField] List<Card> hand = new List<Card>();
    [SerializeField] public List<Card> exhaustPile = new List<Card>();
    [SerializeField] List<IBuffable> buffs = new List<IBuffable>();
    [SerializeField] Button emergencyDelivery;//emergency delivery code
    [SerializeField] Image energyNotification;
    [SerializeField] CardLayoutManager layoutManager;//card layout
    [SerializeField] HighlightObjectController bellHighlight;
    #endregion

    int numOfCards = 0;//right now useless
    List<Customer> customers = new List<Customer>();

    #region Public Vars
    public bool combinePhase;
    public bool discardPhase;
    public int count;
    public BattleState battleState;
    public Player Player => player;      //getter
    public bool hasCardBeenPlayed = false;//emergency delivery code
    #endregion

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        SetUpDeck();
        for (int i = 0; i < cardSlots.Length; i++)
        {
            cardSlots[i].Hide(true);
        }
        DrawCards(5);
        AddEnergy(player.MaxEnergy);
        repUI.text = $"{player.rep}";
        combinePhase = false;
        discardPhase = false;
    }

    private void SetUpDeck()
    {
        deck = new List<Card>();
        int idx = 0;
        foreach (var card in player.Deck)
        {
            if (card.CardType != "Manoeuvre")
                idx = 1;
            else
                idx = 0;
            Card cardTemp = Instantiate(cardPrefabs[idx]);
            cardTemp.GetDataFromBase(card);
            deck.Add(cardTemp);
        }
    }

    private void RemoveExhaustedCards()
    {
        if (player.IsVictorious) { return; }
        foreach(var exhaustedCard in exhaustPile)
        {
            Debug.Log("deleting" + exhaustedCard.CardName);
            CardBaseInfo foundCard = null;
           foreach(var card in player.Deck)
            {
                if(card.CardName == exhaustedCard.CardName)
                {
                    foundCard = card;
                }
            }
            if(foundCard != null) player.Deck.Remove(foundCard);
        }
    }

    public bool IsEnoughCardsOnHand(int requiredAmount)
    {
        return hand.Count > requiredAmount;
    }
    
    public bool IsEnoughFoodOnHand(int requiredAmount)
    {
        return hand.FindAll(card => card.CardType == "Vegetarian" || card.CardType == "Meat" || card.CardType == "Mix" || card.CardType == "Neutral").Count >= requiredAmount;
    }

    //emergency delivery code
    public void AddCardsToDeck(List<Card> newCards)
    {
        foreach(var newCard in newCards)
        {
            deck.Add(newCard);
        }
        player.canCallEmergencyDelivery = false;
    }

    public void ShowNotification()
    {
        energyNotification.gameObject.SetActive(true);
        energyNotification.DOFade(1f, 1f).OnComplete(() => {
            energyNotification.DOFade(0, 2f).OnComplete(() =>
            {
                energyNotification.gameObject.SetActive(false);
            });
        });
    }
    
    #region Player Functions
    public void AddEnergy(int amount)
    {
        player.Energy += amount;
        energyUI.text = $"{player.Energy}/{player.MaxEnergy}";
        bellHighlight.ToggleHighlight(false);
    }

    public bool SpendEnergy(int amount)
    {
        if (player.Energy - amount < 0)
        {
            return false;
        }
        else
        {
            player.Energy -= amount;
            energyUI.text = $"{player.Energy}/{player.MaxEnergy}";
            return true;
        }
    }

    public void HurtPlayer(int amount)
    {
        player.TakeDamage(amount);
        repUI.text = $"{player.rep}";
    }

    public void BuffPlayer(IBuffable buff, bool applyNow = false)
    {
        buff.SetTarget(player);
        buffs.Add(buff);
        if (applyNow)
            ApplyBuff(buff);
    }

    //run this at the start of Player's turn so all the buffs are added properly
    public void ApplyBuff(IBuffable buff)
    {
        if (buff.Finished)
            buffs.Remove(buff);
        else
        {
            Debug.Log("apply");
            buff.Apply();
        }
        UpdateUI();
    }

    //run this at the start of Player's turn so all the buffs are added properly
    public void ApplyBuffables()
    {
        player.GeneralFoodModBonus = 0;
        player.MeatFoodModBonus = 0;
        player.VegetarianFoodModBonus = 0;
        List<IBuffable> delBuffs = new List<IBuffable>();
        foreach (var buff in buffs)
        {
            if (buff.Finished)
                delBuffs.Add(buff);
            else
            {
                buff.Apply();
            }
        }
        //TODO: Find a better way to do this
        foreach (var buff in delBuffs)
        {
            buffs.Remove(buff);
            buff.EndEffect();
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        repUI.text = $"{player.rep}";
        energyUI.text = $"{player.Energy}/{player.MaxEnergy}";
        foreach (var card in cardSlots)
        {
            card.UpdateNP();
        }
    }
    #endregion

    #region Turn Base Functions
    public void EndPlayersTurn()
    {
        if (discardPhase || combinePhase) return;
        Debug.Log("ITS ENEMIES TURN");

        DiscardHand();
        count = customers.Count - 1;

        SpendEnergy(player.Energy);
        AddEnergy(player.MaxEnergy);

        foreach (var customer in customers)
        {
            customer.StartTurn();
        }
        
        //hand.Clear();
        EndEnemyTurn();
        ApplyBuffables();
        hasCardBeenPlayed = false;//emergency delivery code
    }

    public void EndEnemyTurn()
    {
        Debug.Log("ITS PLAYERS TURN");

        DrawCards(5);

        foreach (var customer in customers.ToArray())
        {
            customer.EndTurn();
            customer.RandomizeDebuffs();
        }
    }
    public void CustomerListAdd(Customer customer)
    {
        customers.Add(customer);
    }
    public void CustomerListDelete(Customer customer)
    {
        customers.Remove(customer);
    }
    #endregion

    #region Card Functions
    private bool DrawCard()
    {
        Card randCard = deck[Random.Range(0, deck.Count)];

        for (int i = 0; i < availableCardSlots.Length; i++)
        {
            if (availableCardSlots[i])
            {
                //randCard.gameObject.SetActive(true);
                //randCard.transform.position = cardSlots[i].transform.position;
                //cardSlots[i].SetHasBeenPlayed(false);
                HideCardSlot(i, false);
                cardSlots[i].HandIndex = i;
                cardSlots[i].SetCard(randCard);
                hand.Add(randCard);
                availableCardSlots[i] = false;
                deck.Remove(randCard);
                numOfCards++;
                return true;
            }
        }
        return false;
    }

    public void DrawCards(int count)
    {
        if (count > (deck.Count + hand.Count + discardPile.Count))
            count = (deck.Count + hand.Count + discardPile.Count);
        for (int i = 0; i < count; i++)
        {
            if (deck.Count >= 1)
            {
                if (!DrawCard())
                {
                    //Debug.Log("Couldn't draw a card. The hand is full!");
                    return;
                }
            }
            else
            {
                //Debug.Log("Couldn't draw a card. The deck is empty!"); 
                Shuffle();
                i--;
            }
        }
        //Debug.Log("I DID IT! :)");
    }

    public void DeselectAllCards()
    {
        for (int i = 0; i < cardSlots.Length; i++)
        {
            cardSlots[i].Deselect();
        }
    }

    public void DiscardHand()
    {
        foreach (var card in cardSlots)
        {
            //card.MoveToDiscardPile(true);
            if (card.gameObject.activeSelf)
                SendToDiscard(card.HandIndex, true);
        }
        hand.Clear();
    }

    public void HideCardSlot(int idx, bool isHidden)
    {
        cardSlots[idx].Hide(isHidden);
        if (isHidden)//card layout
            layoutManager.RemoveChild(cardSlots[idx]);
        else
            layoutManager.AddChild(cardSlots[idx]);
    }

    private int GetNumberOfCards()
    {
        int numberOfCards = 0;
        foreach(var slot in cardSlots)
        {
            if (slot.gameObject.activeSelf)
                numberOfCards++;
        }
        return numberOfCards;
    }

    //Called when pointer hovers over a card to make it more visible
    //TODO: Find a way to move a card to the front. Z-order and Child indexes don't work
    public void MoveNeighbours(int idx, bool isReturning)
    {
        foreach(var card in cardSlots)
        {
            if (card.IsDragged) return;
        }
        if (isReturning)
        {
            layoutManager.OrderChildren();
            //highlightSlot.Hide(true);
            //highlightSlot.HandIndex = -1;
            //cardSlots[idx].MakeInvisible(false);
            //cardSlots[idx].Rise(false);
            //for (int i = 0; i < cardSlots.Length; i++)
            //{
            //    if(i != idx)
            //        cardSlots[i].ResetPos();
            //}
        }
        else
        {
           layoutManager.HighlightChild(idx);
        }
    }

    public void SendToDiscard(int idx, bool isEndTurn)
    {
        Debug.Log("idx:" + idx);
        Card card = cardSlots[idx].GetCard();
        availableCardSlots[idx] = true;
        //card.HandIndex = -1;
        //card.SetHasBeenPlayed(false);
        HideCardSlot(idx, true);
        //if (!isEndTurn){
        hand.Remove(card);
        //}
        discardPile.Add(card);
        numOfCards--;
    }

    public void SendToExhaust(int idx)
    {
        Card card = cardSlots[idx].GetCard();
        availableCardSlots[idx] = true;
        //card.HandIndex = -1;
        //card.SetHasBeenPlayed(false);
        HideCardSlot(idx, true);
        hand.Remove(card);
        //card.DeletionCheck(true);
        exhaustPile.Add(card);
    }

    private void Shuffle()
    {
        deck = new List<Card>(discardPile);
        discardPile.Clear();
    }
    #endregion

    #region Discard Phase Functions
    public async Task StartDiscard()
    {
        if (combinePhase) return;
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

    //this is here for testing purposes DELETE
    bool canvas = false;

    public void Update()
    {
        if (customers.Count == 0)
        {
            RemoveExhaustedCards();
            Player.CheckCondition();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            StopDiscard();
            //HurtPlayer(1000);
        }

        //emergency delivery
        if (player.canCallEmergencyDelivery)
        {
            if (!emergencyDelivery.interactable)
                emergencyDelivery.interactable = true;
        }
        else
        {
            if (emergencyDelivery.interactable)
                emergencyDelivery.interactable = false;
        }

        if(player.Energy == 0)
        {
            bellHighlight.ToggleHighlight(true);
        }
    }

    public void StopDiscard()
    {
        Debug.Log("Stop Discard");
        for (int i = 0; i < cardSlots.Length; i++)
        {
            if (cardSlots[i].Selected)
            {
                cardSlots[i].Deselect();
                cardSlots[i].MoveToDiscardPile(false);
            }
        }
        discardController.ToggleDiscard();
        discardPhase = false;
        if (highlightSlot.gameObject.activeSelf)
        {
            highlightSlot.Hide(true);
            highlightSlot.Deselect();
        }
    }
    
    public void SetDiscardFilter(string[] filter)
    {
        discardController.Filter = filter;
    }

    public bool SetCard(CardSlot card)
    {
        if (discardPhase)
        {
            //Debug.Log("SET");
            return discardController.SelectCard(card);
        }
        return false;
    }
    #endregion

    #region Combine Phase Functions
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
        if (discardPhase) return;
        //Debug.Log("Start Combine");
        combineController.ToggleCombine();
        combinePhase = true;
        DeselectAllCards();
    }

    private void StopCombine()
    {
        //Debug.Log("Stop Combine");
        combineController.ToggleCombine();
        combinePhase = false;
        DeselectAllCards();
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
                Debug.Log(card.CardName);
                find.Add(card.CardName);
                types.Add(card.CardType);
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
                cardSlots[i].MoveToExhaustPile();
            }
        }
        for (int i = 0; i < availableCardSlots.Length; i++)
        {
            if (availableCardSlots[i])
            {
                //cardSlots[i].SetHasBeenPlayed(false);
                HideCardSlot(i, false);
                cardSlots[i].SetCard(target);
                cardSlots[i].HandIndex = i;
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
        int highest = 0;
        for (int i = 0; i < cardSlots.Length; i++)
        {
            if (cardSlots[i].Selected)
            {
                result += 8;
                if (cardSlots[i].GetCard().NutritionPoints > highest)
                    highest = cardSlots[i].GetCard().NutritionPoints;
            }
        }
        result = highest + result;
        return result + (int)(result * 0.08f);
    }

    public int GetComboEnergy()
    {
        int result = -1;
        for (int i = 0; i < cardSlots.Length; i++)
        {
            if (cardSlots[i].Selected)
            {
                result += cardSlots[i].GetCard().EnergyCost;
            }
        }
        if (result > 9)
            return 9;//do we want to have energy cost in double digits?
        else
            return result;
    }

    public int GetNumOfSelected()
    {
        int selected = 0;
        for (int i = 0; i < cardSlots.Length; i++)
        {
            if (cardSlots[i].Selected)
            {
                selected++;
            }
        }
        return selected;
    }
    #endregion
}
