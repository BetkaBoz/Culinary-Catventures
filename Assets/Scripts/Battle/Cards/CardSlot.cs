using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardSlot : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    #region PrivateVars
    bool hasBeenPlayed;
    bool isDragged = false;
    bool isSelected = false;
    bool isRised = false;
    Camera cam;
    Vector3 dragOffset;
    Vector3 originalPos;
    //private Vector3 originalMousePos;
    CanvasGroup canvasGroup;
    int handIndex;
    Canvas tempCanvas;
    GraphicRaycaster tempReycaster;
    int originalSiblingIndex;
    #endregion

    #region SerializeFields
    [SerializeField] Image artworkImage;
    [SerializeField] TextMeshProUGUI energyTxt;
    [SerializeField] GameManager gm;
    [SerializeField] ArrowHandler arrowHandler;
    [SerializeField] ManouverTargetController targetController;
    [SerializeField] Card card = null;
    [SerializeField] Text nutritionalValue;
    #endregion

    #region Getters/Setters
    public int HandIndex
    {
        get
        {
            return handIndex;
        }
        set
        {
            handIndex = value;
        }
    }
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
        if (otherCard.CardType == "Manoeuvre")
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
        if (card.CardType != "Manoeuvre")
            nutritionalValue.text = "" + result;
    }
    #endregion

    #region Misc. Functions
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        originalSiblingIndex = transform.GetSiblingIndex();
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
        ResetPos();
        gm.SendToDiscard(handIndex, isEndTurn);
        //gameObject.SetActive(false);
    }

    public void MoveToExhaustPile()
    {
        ResetPos();
        card.DeleteOnBattleEnd = true;
        gm.SendToExhaust(handIndex);
    }

    public void ResetPos()
    {
        transform.position = originalPos;
        transform.localScale = new Vector2(0.65f, 0.65f);
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
        if(card.CardType == "Manoeuvre") { return; }
        if (!gm.discardPhase && gm.combinePhase)
        {
            if (isSelected)
            {
                //Debug.Log("wat");
                //isSelected = false;
                //transform.position = originalPos;
                Deselect();
            }
            else
            {
                if (gm.GetNumOfSelected() < 5)
                {
                    //Debug.Log("yed");
                    isSelected = true;
                    Rise(true);
                }
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
                if(gm.SetCard(this))
                    isSelected = true;
                else
                {
                    isSelected = true;
                    Deselect();
                }

            }
            else
            {
                if (card.CanTarget)
                {
                    if (hasBeenPlayed == false && gm.SpendEnergy(card.EnergyCost))
                    {
                        hasBeenPlayed = true;
                        if (card.CardType == "Manoeuvre")
                        {
                            MoveToDiscardPile(false);
                            card.TriggerCardEffect(gm, hit);
                        }
                        else
                        {
                            MoveToExhaustPile();
                            card.TriggerCardEffect(gm, hit);
                        }
                    }
                    arrowHandler.setVisibile(false);
                }
                else
                {
                    if (hasBeenPlayed == false && gm.SpendEnergy(card.EnergyCost))
                    {
                        hasBeenPlayed = true;
                        if (card.CardType == "Manoeuvre")
                        {
                            MoveToDiscardPile(false);
                            card.TriggerCardEffect(gm, hit);
                        }
                        else
                        {
                            MoveToExhaustPile();
                            card.TriggerCardEffect(gm, hit);                            
                        }
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
                Rise(false);
            }

        }
        else
        {
            if (card.CanTarget && !gm.discardPhase)
            {
                arrowHandler.setVisibile(false);
            }
            isDragged = false;
            Rise(false);
        }
    }
    #endregion

    #region MouseHover
    public void OnPointerEnter(PointerEventData eventData)
    {
        Rise(true);
    }

    public void CreateCanvas(bool enable)
    {
        if (enable)
        {
            //tempCanvas = gameObject.AddComponent<Canvas>();
            //tempCanvas.overrideSorting = true;
            //tempCanvas.sortingOrder = 1;
            //tempReycaster = gameObject.AddComponent<GraphicRaycaster>();
            //transform.position += (Vector3.up * 2f);
            originalPos = transform.localPosition;
            Debug.Log("transform: " + transform.position + " original: " + originalPos);
            transform.SetAsLastSibling();
            transform.position += (Vector3.up * 0.5f);
            Debug.Log("transform: " + transform.position + " original: " + originalPos);
        }
        else
        {
            transform.SetSiblingIndex(originalSiblingIndex);
            //Destroy(tempReycaster);
            //Destroy(tempCanvas);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Rise(false);
    }

    private void Rise(bool willRise)
    {
        if (willRise)
        {
            //Debug.Log(card.CardEffect.Card);
            if (isRised) { return; }
            //spriteOrder = transform.GetSiblingIndex();

            originalPos = transform.position;
            transform.localScale = new Vector2(1f, 1f);
            transform.position += (Vector3.up * 0.5f);

            //this seems kinda jank, will most likely replace it later
            //tempCanvas = gameObject.AddComponent<Canvas>();
            //tempCanvas.overrideSorting = true;
            //tempCanvas.sortingOrder = 1;
            //tempReycaster = gameObject.AddComponent<GraphicRaycaster>();
            //transform.SetAsLastSibling();
            gm.MoveNeighbours(handIndex, false);
            isRised = true;
        }
        else
        {
            if (isDragged || isSelected) { return; }
            //Destroy(tempReycaster);
            //Destroy(tempCanvas);
            ResetPos();
            //transform.SetSiblingIndex(spriteOrder);
            gm.MoveNeighbours(handIndex, true);
            isRised = false;
        }
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
