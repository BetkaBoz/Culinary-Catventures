using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscardController : MonoBehaviour
{
    [SerializeField] private ManouverTargetController targetController;
    [SerializeField] private Text discardText;
    [SerializeField] private Transform cardSlot;
    [SerializeField] private GameManager gm;
    [SerializeField] private Button discardBttn;
    private string[] filter;
    public string[] Filter
    {
        set
        {
            filter = value;
        }
    }

    private CardSlot selectedCard;

    public void DiscardCard()
    {
        if(selectedCard != null)
        {
            //gm.SendToDiscard(selectedCard.HandIndex, false);
            gm.StopDiscard();
        }
    }
    
    public void ToggleDiscard()
    {
        if (!gm.discardPhase)
        {
            this.gameObject.SetActive(true);
            discardText.gameObject.SetActive(true);
            targetController.SetPos(false);
            discardBttn.onClick.RemoveAllListeners();
            discardBttn.onClick.AddListener(DiscardCard);
            discardBttn.interactable = false;
        }
        else
        {
            targetController.SetPos(true);
            discardBttn.interactable = false;
            this.gameObject.SetActive(false);
            discardText.gameObject.SetActive(false);
        }
    }

    public bool SelectCard(CardSlot otherSelectedCard)
    {
        if (IsinFilter(otherSelectedCard.GetCard().CardType))
        {
            if (this.selectedCard != null)
            {
                this.selectedCard.Deselect();
            }
            this.selectedCard = otherSelectedCard;
            this.selectedCard.transform.position = new Vector3(this.cardSlot.position.x, this.cardSlot.position.y, this.selectedCard.transform.position.z);
            discardBttn.interactable = true;
            return true;
        }
        return false;
    }

    private bool IsinFilter(string selectedCardType)
    {
        if (filter.Length == 0)
            return true;
        for(int i = 0; i < filter.Length; i++)
        {
            Debug.Log(filter[i]);
            if (selectedCardType == filter[i])
            {
                return true;
            }
        }
        return false;
    }
}
