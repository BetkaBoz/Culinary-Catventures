using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscardController : MonoBehaviour
{
    [SerializeField] private ManouverTargetController targetController;
    [SerializeField] private Text header;
    [SerializeField] private Transform cardSlot;
    [SerializeField] private GameManager gm;
    [SerializeField] private Button discardBttn;
    private CardSlot selectedCard;

    public void DiscardCard()
    {
        if(selectedCard != null)
        {
            gm.SendToDiscard(selectedCard.GetCard(), false);
            gm.StopDiscard();
        }
    }
    
    public void ToggleDiscard()
    {
        if (!gm.discardPhase)
        {
            this.gameObject.SetActive(true);
            targetController.setPos(false);
            discardBttn.gameObject.SetActive(true);
            discardBttn.interactable = false;
        }
        else
        {
            targetController.setPos(true);
            discardBttn.interactable = false;
            discardBttn.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }

    public void SelectCard(CardSlot otherSelectedCard)
    {
        if(this.selectedCard != null)
        {
            this.selectedCard.Deselect();
        }
        this.selectedCard = otherSelectedCard;
        this.selectedCard.transform.position = new Vector3 (this.cardSlot.position.x, this.cardSlot.position.y, this.selectedCard.transform.position.z);
        discardBttn.interactable = true;
    }
}
