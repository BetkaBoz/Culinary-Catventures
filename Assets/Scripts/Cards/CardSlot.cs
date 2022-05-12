using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    #region PrivateVars
    private bool hasBeenPlayed;
    //private int handIndex;
    private bool isDragged = false;
    private bool isSelected = false;
    private Camera cam;
    private Vector3 dragOffset;
    private Vector3 originalPos;
    //private Vector3 originalMousePos;
    private CanvasGroup canvasGroup;

    [SerializeField] private Image artworkImage;
    [SerializeField] private Image[] energyImage;
    [SerializeField] private GameManager gm;
    #endregion
    [SerializeField] private ArrowHandler arrowHandler;
    [SerializeField] private ManouverTargetController targetController;
    [SerializeField] private Card card = null;
    [SerializeField] private Text nutritionalValue;

    public bool Selected
    {
        get
        {
            return isSelected;
        }
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        //gm = FindObjectOfType<GameManager>();
        cam = Camera.main;
        isDragged = false;
        isSelected = false;
        if (card != null)
        {
            SetCard(card);
        }
    }

    public void SetCard(Card otherCard)
    {
        this.card = otherCard;
        if(otherCard.NutritionPoints == -1)
        {
            this.nutritionalValue.text = "";
        }
        else
        {
            this.nutritionalValue.text = ""+otherCard.NutritionPoints;
        }
        this.artworkImage.sprite = otherCard.Artwork;
        UpdateEnergy();
        //UpdateNP();
    }

    private void UpdateEnergy()
    {
        int numOfEnergy = card.EnergyCost;
        for (int i = 0; i < energyImage.Length; i++)
        {
            if (numOfEnergy > 0)
            {
                numOfEnergy--;
                energyImage[i].enabled = true;
            }
            else
            {
                energyImage[i].enabled = false;
            }
        }
    }

    //public void UpdateNP()
    //{
    //    int result;
    //    result = card.NutritionPoints * 
    //    this.nutritionalValue.text
    //}

    public Card GetCard()
    {
        if(card != null)
        {
            return card;
        }
        return null;
    }

    #region OldDragNDrop
    //private void OnMouseDown()
    //{
    //    //Debug.Log(canTarget.ToString());
    //    if (this.isSelected)
    //    {
    //        isSelected = false;
    //        this.transform.position = originalPos;
    //    }
    //    else
    //    {
    //        originalPos = this.transform.position;
    //        if (gm.discardPhase)
    //        {
    //            dragOffset = this.transform.position - GetMousePos();
    //            isSelected = false;
    //            isDragged = true;
    //        }
    //        else
    //        {
    //            if (!card.CanTarget)
    //            {
    //                targetController.setPos(false);
    //                dragOffset = this.transform.position - GetMousePos();
    //                //this.isDragged = true;
    //            }
    //            isSelected = true;
    //            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z);
    //        }
    //    }
    //    originalMousePos = GetMousePos();
    //}

    //private void OnMouseDrag()
    //{
    //    if (this.isSelected && (originalMousePos != GetMousePos()))
    //    {
    //        this.isDragged = true;
    //    }
    //    if (this.isDragged)
    //    {
    //        if (this.card.CanTarget && !gm.discardPhase)
    //        {
    //            if (!arrowHandler.isVisible)
    //            {
    //                arrowHandler.setVisibile(true);
    //                arrowHandler.SetOrigin(new Vector2(this.transform.position.x, this.transform.position.y));
    //            }
    //        }
    //        else
    //        {
    //            this.transform.position = GetMousePos() + dragOffset;
    //        }

    //    }
    //}

    //private void OnMouseUp()
    //{
    //    LayerMask dragTarget = LayerMask.GetMask("DragTarget");
    //    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.up, 1, dragTarget.value);
    //    if (hit.collider != null)
    //    {
    //        if (gm.discardPhase)
    //        {
    //            gm.SetCard(this);
    //            this.isSelected = true;
    //        }
    //        else
    //        {
    //            if (card.CanTarget)
    //            {
    //                if (hasBeenPlayed == false && gm.SpendEnergy(card.EnergyCost))
    //                {
    //                    hasBeenPlayed = true;
    //                    this.card.CardEffect(gm, hit);
    //                    MoveToDiscardPile(false);
    //                    arrowHandler.setVisibile(false);
    //                }
    //            }
    //            else
    //            {
    //                if (hasBeenPlayed == false && gm.SpendEnergy(card.EnergyCost))
    //                {
    //                    hasBeenPlayed = true;
    //                    this.card.CardEffect(gm, hit);
    //                    MoveToDiscardPile(false);
    //                }
    //                else
    //                {
    //                    this.transform.position = this.originalPos;
    //                }
    //                if (!gm.discardPhase)
    //                {
    //                    targetController.setPos(true);
    //                }
    //            }
    //            this.isDragged = false;
    //            this.isSelected = false;
    //            this.transform.position = this.originalPos;
    //        }

    //    }
    //    else
    //    {
    //        if (card.CanTarget && !gm.discardPhase)
    //        {
    //            arrowHandler.setVisibile(false);
    //        }
    //        else
    //        {
    //            if (isDragged)
    //            {
    //                this.transform.position = this.originalPos;
    //            }
    //        }
    //    }
    //}
    #endregion

    #region IndexGetSet
    //public void SetIndex(int handIndex)
    //{
    //    this.handIndex = handIndex;
    //}

    //public int GetIndex()
    //{
    //    return this.handIndex;
    //}
    #endregion

    private Vector3 GetMousePos()
    {
        var mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }

    public void SetHasBeenPlayed(bool hasBeenPlayed)
    {
        this.hasBeenPlayed = hasBeenPlayed;
    }

    public void MoveToDiscardPile(bool isEndTurn)
    {
        gm.SendToDiscard(card.HandIndex, isEndTurn);
        //gameObject.SetActive(false);
    }

    public void Deselect()
    {
        if (this.isSelected)
        {
            this.transform.position = originalPos;
            this.isDragged = false;
            this.isSelected = false;
        }
    }

    private void Select()
    {
        if(card.NutritionPoints == -1) { return; }
        if (!gm.discardPhase && gm.combinePhase)
        {
            if (this.isSelected)
            {
                Debug.Log("wat");
                isSelected = false;
                this.transform.position = originalPos;
            }
            else
            {
                Debug.Log("yed");
                isSelected = true;
                originalPos = this.transform.position;
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click");
        Select();
        if (gm.combinePhase)
        {
            gm.FindCombineTarget();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("DOWN");
        //Debug.Log(canTarget.ToString());
        if (gm.discardPhase)
        {
            dragOffset = this.transform.position - GetMousePos();
            isSelected = false;
            isDragged = true;
        }
        else
        {
            if (!card.CanTarget)
            {
                targetController.setPos(false);
                dragOffset = this.transform.position - GetMousePos();
                //this.isDragged = true;
            }
            //isSelected = true;
            //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z);
        }
        //originalMousePos = GetMousePos();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //you can only select cards (by clicking) during select phase
        if (gm.combinePhase) { return; }
        canvasGroup.blocksRaycasts = false;
        originalPos = this.transform.position;
        //Debug.Log("idx:" + card.HandIndex);
        //Debug.Log("BEGIN DRAG");
        //Select();
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("DRAG");
        if (gm.combinePhase) { return; }
        //if (this.isSelected && (originalMousePos != GetMousePos()))
        //{
        //the whole isDragged thing might be useless now
        this.isDragged = true;
        //}
        if (this.isDragged)
        {
            //if you are a manouver or if you have to discard a card don't use targeting arrows
            if (this.card.CanTarget && !gm.discardPhase)
            {
                if (!arrowHandler.isVisible)
                {
                    arrowHandler.setVisibile(true);
                    arrowHandler.SetOrigin(new Vector2(this.transform.position.x, this.transform.position.y));
                }
            }
            else
            {
                this.transform.position = GetMousePos() + dragOffset;
            }

        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (gm.combinePhase) { return; }
        canvasGroup.blocksRaycasts = true;
        //Debug.Log("END");
        LayerMask dragTarget = LayerMask.GetMask("DragTarget");
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.up, 1, dragTarget.value);
        if (hit.collider != null)
        {
            if (gm.discardPhase)
            {
                gm.SetCard(this);
                this.isSelected = true;
            }
            else
            {
                if (card.CanTarget)
                {
                    if (hasBeenPlayed == false && gm.SpendEnergy(card.EnergyCost))
                    {
                        hasBeenPlayed = true;
                        this.card.CardEffect(gm, hit);
                        MoveToDiscardPile(false);
                    }
                    arrowHandler.setVisibile(false);
                }
                else
                {
                    if (hasBeenPlayed == false && gm.SpendEnergy(card.EnergyCost))
                    {
                        hasBeenPlayed = true;
                        this.card.CardEffect(gm, hit);
                        MoveToDiscardPile(false);
                    }
                    else
                    {
                        this.transform.position = this.originalPos;
                    }
                    if (!gm.discardPhase)
                    {
                        targetController.setPos(true);
                    }
                }
                this.isDragged = false;
                this.isSelected = false;
                //this.transform.position = this.originalPos;
            }

        }
        else
        {
            if (card.CanTarget && !gm.discardPhase)
            {
                arrowHandler.setVisibile(false);
            }
            else
            {
                if (isDragged)
                {
                    this.transform.position = this.originalPos;
                }
            }
        }
    }

    public void Hide(bool isHiding)
    {
        if (isHiding)
        {
            Debug.Log("psst I'm hiding");
        }
        else
        {
            Debug.Log("Yahaha you've found me!");
        }
        gameObject.SetActive(!isHiding);
    }
}
