using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class CardSlot : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    #region PrivateVars
    //bool hasBeenPlayed;
    [SerializeField] bool isDragged = false;
    [SerializeField] bool isSelected = false;
    [SerializeField] bool isRised = false;
    public bool isSelectedForDiscard = false;
    public bool hasMoved = false;
    Camera cam;
    Vector3 dragOffset;
    Vector3 originalPos;
    //private Vector3 originalMousePos;
    CanvasGroup canvasGroup;
    [SerializeField] int handIndex;
    Canvas tempCanvas;
    GraphicRaycaster tempReycaster;
    int originalSiblingIndex;
    GameObject notification;
    HighlightObjectController highlightController;
    string message, header;
    List<DebuffTypes> currentDebuffs = new List<DebuffTypes>();
    #endregion

    #region SerializeFields
    [SerializeField] List<Image> debuffImage;
    [SerializeField] Image artworkImage;
    [SerializeField] TextMeshProUGUI energyTxt;
    [SerializeField] GameManager gm;
    [SerializeField] ArrowHandler arrowHandler;
    [SerializeField] ManouverTargetController targetController;
    [SerializeField] Card card = null;
    [SerializeField] Text nutritionalValue;
    [SerializeField] bool isHighlight = false;
    [SerializeField] CardSlot highlightedSlot = null;
    [SerializeField] List<Hoverable> hoverableDebuffs;
    #endregion

    #region Getters/Setters
    public bool IsRised => isRised;
    public bool IsDragged => isDragged;
    public int SlotIndex;//this is used in layout manager
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

    public List<DebuffTypes> CurrentDebuffs => currentDebuffs;

    //public void SetHasBeenPlayed(bool hasBeenPlayed)
    //{
    //    this.hasBeenPlayed = hasBeenPlayed;
    //}

    public void SetCard(Card otherCard)
    {
        card = otherCard;
        if (otherCard.CardType == CardTypes.Manoeuvre)
        {
            nutritionalValue.text = "";
        }
        else
        {
            nutritionalValue.text = "" + otherCard.NutritionPoints;
        }
        artworkImage.sprite = otherCard.Artwork;
        SetBuffImages();
        UpdateEnergy();
        UpdateNP();
    }

    private void SetBuffImages()
    {
        if(card.CardType == CardTypes.Manoeuvre)
        {
            foreach(var img in debuffImage)
            {
                img.GetComponentInParent<Image>().gameObject.SetActive(false);
            }
            return;
        }
        else
        {
            foreach (var img in debuffImage)
            {
                img.GetComponentInParent<Image>().gameObject.SetActive(true);
            }
            
        }
        int idx = 0;
        foreach(var effect in card.CardEffect)
        {
            if(effect is CardEffectWithDebuff)
            {
                if(idx > debuffImage.Count)
                {
                    Debug.Log("HOW IS YOUR NUMBER OF EFFECTS MORE THAN 3?!");
                    return;
                }
                debuffImage[idx].gameObject.SetActive(true);
                debuffImage[idx].sprite = (effect as CardEffectWithDebuff).BuffArtworkCard;
                idx++;
            }
        }
        for(int i = idx; i < debuffImage.Count; i++)
        {
            debuffImage[i].gameObject.SetActive(false);

        //    switch (CurrentDebuffs[idx])
        //    {
        //        case DebuffTypes.Stun:
        //            message = "Customer will be unable to perform their action for a turn. Stacking of this effect should increase the number of turns this effect lasts.";
        //            header = "STUN";
        //            break;
        //        case DebuffTypes.Captivate:
        //            message = "Prevents customer from leaving. Stacking of this effect increases the number of turns this effect lasts.";
        //            header = "CAPTIVATE";
        //            break;
        //        case DebuffTypes.Flavourful:
        //            message = "Customer's Hunger will get decreased by X at the start of their turn, then X will decrease by 1. Stacking of this effect should increase X.";
        //            header = "FLAVORFUL";
        //            break;
        //        case DebuffTypes.WeaknessMeat:
        //            message = "Any Meat only food used on customer will decrease Hunger by additional 25%. Stacking of this effect should increase the number of turns this effect lasts.";
        //            header = "WEAKNESS: Meat";
        //            break;
        //        case DebuffTypes.WeaknessVegg:
        //            message = "Any Mixed food used on customer will decrease Hunger by additional 25%. Stacking of this effect should increase the number of turns this effect lasts.";
        //            header = "WEAKNESS: Vegetables";
        //            break;
        //        case DebuffTypes.WeaknessMix:
        //            message = "Any Vegetarian food used on customer will decrease Hunger by additional 25%. Stacking of this effect should increase the number of turns this effect lasts.";
        //            header = "WEAKNESS: Mix";
        //            break;
        //    }
        //    hoverableDebuffs[i].SetMessageHeader(message, header);
        }
    }

    public void SetHighlight(CardSlot otherSlot)
    {
        if (handIndex != -1)
            gm.MoveNeighbours(SlotIndex, true);
        otherSlot.Rise(true);
        handIndex = otherSlot.HandIndex;
        Hide(false);
        transform.position = new Vector2(otherSlot.transform.position.x, otherSlot.transform.position.y);
        otherSlot.MakeInvisible(true);
        SetCard(otherSlot.GetCard());
        highlightedSlot = otherSlot;
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
        if (card.CardType != CardTypes.Manoeuvre)
            nutritionalValue.text = "" + result;
    }
    #endregion

    #region Misc. Functions
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        highlightController = GetComponent<HighlightObjectController>();
        //originalSiblingIndex = transform.GetSiblingIndex();
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

    public void ResetPos()//potentially pointless
    {
        //if (isSelected)
        //    transform.position = new Vector2(originalPos.x, transform.position.y);
        //else
            transform.position = originalPos;
            hasMoved = false;
        if(!isHighlight && !isSelected)
            transform.localScale = new Vector2(0.65f, 0.65f);
    }

    private void StorePos()//potentially pointless
    {
        if (!hasMoved)
        {
            originalPos = transform.position;
            hasMoved = true;
        }
    }

    public void Deselect()
    {
        if (isSelected)
        {
            isDragged = false;
            isSelected = false;
            isRised = false;
            isSelectedForDiscard = false;
            gm.MoveNeighbours(SlotIndex, true);
            highlightController.ToggleHighlight(isSelected);
        }
    }

    private void Select()
    {
        if(card.CardType == CardTypes.Manoeuvre) { return; }
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
                if (gm.GetNumOfSelected() < 5)                {
                    //Debug.Log("yed");

                    isSelected = true;
                    Rise(true);
                    highlightController.ToggleHighlight(isSelected);
                }
            }
        }
    }
    #endregion

    #region DragNDrop
    public void OnPointerClick(PointerEventData eventData)
    {
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
        if (gm.discardPhase)//ARTURITO
        {
            isSelectedForDiscard = true;//Horrible jank FIX LATER!
            if (gm.SetCard(this))
            {
                isSelected = true;
            }
            else
            {
                isSelected = true;
                Deselect();
            }
            //dragOffset = transform.position - GetMousePos();
            //if(highlightedSlot != null)
            //{
            //    highlightedSlot.isSelected = false;
            //    highlightedSlot.isDragged = true;
            //}
        }
        else
        {
            if (!card.CanTarget)
            {
                targetController.SetPos(false);
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
        //during the combine and discard phases you can only select cards by clicking
        if (gm.combinePhase || gm.discardPhase) { return; }
        canvasGroup.blocksRaycasts = false;
        //originalPos = this.transform.position;
        //Debug.Log("idx:" + card.HandIndex);
        //Debug.Log("BEGIN DRAG");
        //Select();
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("DRAG");
        if (gm.combinePhase || gm.discardPhase) { return; }//ARTURITO (maybe pointless?)
        isDragged = true;
        //if you are a manouver or if you have to discard a card don't use targeting arrows
        if (card.CanTarget && !gm.discardPhase)
        {
            if (!arrowHandler.IsVisible)
            {
                arrowHandler.SetVisibile(true);
                arrowHandler.SetOrigin(new Vector2(transform.position.x, transform.position.y)+Vector2.up);
            }
        }
        else
        {
            transform.position = GetMousePos() + dragOffset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (gm.combinePhase || gm.discardPhase) { return; }//ARTURITO (maybe pointless?)
        canvasGroup.blocksRaycasts = true;
        //Debug.Log("END");
        LayerMask dragTarget = LayerMask.GetMask("DragTarget");
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.up, 1, dragTarget.value);
        if (hit.collider != null)
        {
            if (card.CanTarget)
            {
                if ( gm.SpendEnergy(card.EnergyCost))//hasBeenPlayed == false &&
                {
                    //hasBeenPlayed = true;
                    if (card.CardType == CardTypes.Manoeuvre)
                    {
                        MoveToDiscardPile(false);
                        card.TriggerCardEffect(gm, hit);
                    }
                    else
                    {
                        MoveToExhaustPile();
                        card.TriggerCardEffect(gm, hit);
                    }
                        
                    gm.hasCardBeenPlayed = true;
                }
                else
                {
                    gm.ShowNotification();
                }
                    
                arrowHandler.SetVisibile(false);
            }
            else
            {
                if ((hit.transform.gameObject.tag != "Customer") && gm.SpendEnergy(card.EnergyCost))//hasBeenPlayed == false && 
                {
                    //hasBeenPlayed = true;
                    if (card.CardType == CardTypes.Manoeuvre)
                    {
                        if(DiscardCheck())
                        {
                            MoveToDiscardPile(false);
                            card.TriggerCardEffect(gm, hit);
                        }
                    }
                    else
                    {
                        MoveToExhaustPile();
                        card.TriggerCardEffect(gm, hit);
                    }
                    if (!gm.discardPhase)
                        gm.hasCardBeenPlayed = true;
                }
                else
                {
                    transform.position = originalPos;
                }
                if (!gm.discardPhase)
                {
                    targetController.SetPos(true);
                }
            }
            isDragged = false;
            isSelected = false;
            Rise(false);
        }
        else
        {
            if (card.CanTarget && !gm.discardPhase)
            {
                arrowHandler.SetVisibile(false);
            }
            isDragged = false;
            Rise(false);
        }
    }
    #endregion

    private bool DiscardCheck()
    {
        return card.CardEffect.FindAll(item => item is DiscardCardEffect).Count==0 || card.CardEffect.FindAll(item => item is DiscardCardEffect && (item as DiscardCardEffect).CanBeUsed(gm)).Count>0;
        //foreach (var effect in card.CardEffect)
        //{
        //    if (
        //    !(effect is DiscardCardEffect)
        //    || (effect is DiscardCardEffect
        //    && (effect as DiscardCardEffect).CanBeUsed(gm)))
        //    {
        //        return true;
        //    }
        //}
        //return false;
    }

    #region MouseHover
    public void OnPointerEnter(PointerEventData eventData)
    {
        //if(!isHighlight)
            gm.MoveNeighbours(SlotIndex, false);
    }

    //public void CreateCanvas(bool enable)
    //{
    //   if (enable)
    //    {
            //tempCanvas = gameObject.AddComponent<Canvas>();
            //tempCanvas.overrideSorting = true;
            //tempCanvas.sortingOrder = 1;
            //tempReycaster = gameObject.AddComponent<GraphicRaycaster>();
            //transform.position += (Vector3.up * 2f);
    //        originalPos = transform.localPosition;
    //        Debug.Log("transform: " + transform.position + " original: " + originalPos);
    //        transform.SetAsLastSibling();
    //        transform.position += (Vector3.up * 0.5f);
    //        Debug.Log("transform: " + transform.position + " original: " + originalPos);
    //    }
    //    else
    //    {
    //        transform.SetSiblingIndex(originalSiblingIndex);
            //Destroy(tempReycaster);
            //Destroy(tempCanvas);
    //    }

    //}

    public void OnPointerExit(PointerEventData eventData)
    {
        //if(isHighlight)
            Rise(false);
    }

    public void Rise(bool willRise)
    {
        if (willRise)
        {
            //Debug.Log("Start Of Rise " + card.name + " " + handIndex);
            //Debug.Log(card.CardEffect.Card);
            if (isRised) { return; }
            //spriteOrder = transform.GetSiblingIndex();

            //originalPos = transform.position;
            //Debug.Log("Before StorePos " + card.name + " " + handIndex);
            //StorePos();
            //transform.localScale = new Vector2(1f, 1f);
            //Debug.Log("before transform " + card.name + " " + handIndex);
            //transform.position += (Vector3.up * 0.5f);

            //this seems kinda jank, will most likely replace it later
            //tempCanvas = gameObject.AddComponent<Canvas>();
            //tempCanvas.overrideSorting = true;
            //tempCanvas.sortingOrder = 1;
            //tempReycaster = gameObject.AddComponent<GraphicRaycaster>();
            //transform.SetAsLastSibling();
            //Debug.Log("End Of DeRise " + card.name + " " + handIndex);
            isRised = true;
        }
        else
        {
            //Debug.Log("Start Of DeRise " + card.name + " " + handIndex);
            if (isDragged || isSelectedForDiscard) { //THIS IS SO BAD FIX LATER!!!
                //Debug.Log(card.name + " " + handIndex + " rised "+isRised+" selected "+isSelected+" dragged "+isDragged);
                return;
            }
            //Destroy(tempReycaster);
            //Destroy(tempCanvas);
            //Debug.Log("Before Reset "+ card.name + " " + handIndex);
            //ResetPos();
            //transform.SetSiblingIndex(spriteOrder);
            //if (isHighlight)
            //{
                //Debug.Log("In Move " + card.name + " " + handIndex);
            //}
            isRised = false;
            gm.MoveNeighbours(handIndex, true);
            //Debug.Log("End Of DeRise " + card.name + " " + handIndex);
        }
    }

    public void Hide(bool isHiding)
    {
        gameObject.SetActive(!isHiding);
    }

    public void MakeInvisible(bool isInvisible)
    {
        if (isInvisible)
        {
            transform.position += (Vector3.down * 4f);
        }
        else
        {
            transform.position += (Vector3.up * 4f);
        }
    }

    public void MoveLeft(float amount)
    {
        if (!isRised)
            StorePos();
            //originalPos = transform.position;
        transform.position = transform.position + (Vector3.left * amount);//0.5f
    }

    public void MoveRight(float amount)
    {
        if (!isRised)
            StorePos();
            //originalPos = transform.position;
        transform.position = transform.position + (Vector3.right * amount);
    }
    #endregion
}
