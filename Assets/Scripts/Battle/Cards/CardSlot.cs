using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardSlot : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    #region PrivateVars
    private bool hasBeenPlayed;
    //private int handIndex;
    private bool isDragged = false;
    private bool isSelected = false;
    private bool isRised = false;
    private Camera cam;
    private Vector3 dragOffset;
    private Vector3 originalPos;
    //private Vector3 originalMousePos;
    private CanvasGroup canvasGroup;
    private int spriteOrder;
    #endregion

    #region SerializeFields
    [SerializeField] private Image artworkImage;
    [SerializeField] private TextMeshProUGUI energyTxt;
    [SerializeField] private GameManager gm;
    [SerializeField] private ArrowHandler arrowHandler;
    [SerializeField] private ManouverTargetController targetController;
    [SerializeField] private Card card = null;
    [SerializeField] private Text nutritionalValue;
    #endregion

    #region Getters/Setters
    public bool Selected
    {
        get
        {
            return isSelected;
        }
    }

    public void SetHasBeenPlayed(bool hasBeenPlayed)
    {
        this.hasBeenPlayed = hasBeenPlayed;
    }

    public void SetCard(Card otherCard)
    {
        card = otherCard;
        if (otherCard.NutritionPoints == -1)
        {
            nutritionalValue.text = "";
        }
        else
        {
            nutritionalValue.text = "" + otherCard.NutritionPoints;
        }
        artworkImage.sprite = otherCard.Artwork;
        UpdateEnergy();
        UpdateNP();
    }

    public Card GetCard()
    {
        if (card != null)
        {
            return card;
        }
        return null;
    }

    private Vector3 GetMousePos()
    {
        var mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }
    #endregion

    #region UpdateTextboxes
    private void UpdateEnergy()
    {
        int numOfEnergy = card.EnergyCost;
        energyTxt.text = $"{numOfEnergy}";
    }

    public void UpdateNP()
    {
        int result = card.CalculateNP(gm);
        if (result != -1)
            nutritionalValue.text = "" + result;
    }
    #endregion

    #region Misc. Functions
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

    public void MoveToDiscardPile(bool isEndTurn)
    {
        gm.SendToDiscard(card.HandIndex, isEndTurn);
        //gameObject.SetActive(false);
    }

    public void ResetPos()
    {
        transform.position = originalPos;
    }

    public void Deselect()
    {
        if (isSelected)
        {
            ResetPos();
            isDragged = false;
            isSelected = false;
            isRised = false;
        }
    }

    private void Select()
    {
        if(card.NutritionPoints == -1) { return; }
        if (!gm.discardPhase && gm.combinePhase)
        {
            if (isSelected)
            {
                //Debug.Log("wat");
                isSelected = false;
                transform.position = originalPos;
            }
            else
            {
                //Debug.Log("yed");
                isSelected = true;
                if (isRised) { return; }
                originalPos = transform.position;
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            }
        }
    }
    #endregion

    #region DragNDrop
    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("Click");
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
            dragOffset = transform.position - GetMousePos();
            isSelected = false;
            isDragged = true;
        }
        else
        {
            if (!card.CanTarget)
            {
                targetController.setPos(false);
                dragOffset = transform.position - GetMousePos();
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
        //originalPos = this.transform.position;
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
        isDragged = true;
        //}
        if (isDragged)
        {
            //if you are a manouver or if you have to discard a card don't use targeting arrows
            if (card.CanTarget && !gm.discardPhase)
            {
                if (!arrowHandler.isVisible)
                {
                    arrowHandler.setVisibile(true);
                    arrowHandler.SetOrigin(new Vector2(transform.position.x, transform.position.y)+Vector2.up);
                }
            }
            else
            {
                transform.position = GetMousePos() + dragOffset;
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
                isSelected = true;
            }
            else
            {
                if (card.CanTarget)
                {
                    if (hasBeenPlayed == false && gm.SpendEnergy(card.EnergyCost))
                    {
                        hasBeenPlayed = true;
                        card.CardEffect(gm, hit);
                        MoveToDiscardPile(false);
                    }
                    arrowHandler.setVisibile(false);
                }
                else
                {
                    if (hasBeenPlayed == false && gm.SpendEnergy(card.EnergyCost))
                    {
                        hasBeenPlayed = true;
                        MoveToDiscardPile(false);
                        card.CardEffect(gm, hit);
                    }
                    else
                    {
                        transform.position = originalPos;
                    }
                    if (!gm.discardPhase)
                    {
                        targetController.setPos(true);
                    }
                }
                isDragged = false;
                isSelected = false;
                isRised = false;
                transform.position = originalPos;
            }

        }
        else
        {
            if (card.CanTarget && !gm.discardPhase)
            {
                arrowHandler.setVisibile(false);
            }
            isRised = false;
            transform.position = originalPos;
        }
    }
    #endregion

    #region MouseHover
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isRised) { return;}
        //spriteOrder = transform.GetSiblingIndex();
        originalPos = transform.position;
        transform.position = transform.position + (Vector3.up*0.5f);
        //transform.SetAsLastSibling();
        gm.MoveNeighbours(card.HandIndex,false);
        isRised = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDragged || isSelected) { return; }
        transform.position = originalPos;
        //transform.SetSiblingIndex(spriteOrder);
        gm.MoveNeighbours(card.HandIndex, true);
        isRised = false;
    }

    public void Hide(bool isHiding)
    {
        gameObject.SetActive(!isHiding);
    }

    public void MoveLeft(float amount)
    {
        if (isRised) { return; }
        originalPos = transform.position;
        transform.position = transform.position + (Vector3.left * amount);//0.5f
    }

    public void MoveRight(float amount)
    {
        if (isRised) { return; }
        originalPos = transform.position;
        transform.position = transform.position + (Vector3.right * amount);
    }
    #endregion
}
