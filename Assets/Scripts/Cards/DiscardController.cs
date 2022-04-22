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
            ToggleDiscard();
        }
    }
    
    public void ToggleDiscard()
    {
        if (!gm.discardPhase)
        {
            gm.StartDiscard();
            targetController.setPos(false);
            discardBttn.interactable = false;
        }
        else
        {
            gm.StopDiscard();
            targetController.setPos(true);
            discardBttn.interactable = false;
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
