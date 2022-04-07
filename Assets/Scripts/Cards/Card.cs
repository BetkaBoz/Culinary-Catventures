using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour //, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    #region PrivateVars
    private bool hasBeenPlayed;
    private int handIndex;
    private GameManager gm;
    private bool isDragged = true;
    private Camera cam;
    private Vector3 dragOffset;
    #endregion
    #region SerializeFields
    [SerializeField] private int energyCost;
    [SerializeField] private int nutritionPoints;
    [SerializeField] private bool canTarget = false;
    [SerializeField] private ArrowHandler arrowHandler;
    [SerializeField] private ManouverTargetController targetController;
    #endregion

    // Start is called before the first frame update
    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        cam = Camera.main;
    }

    #region DragNDrop
    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    Debug.Log("OnPointerDown");
    //}

    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    Debug.Log("OnBeginDrag");
    //}

    //public void OnDrag(PointerEventData eventData)
    //{
    //    Debug.Log("OnDrag");
    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    Debug.Log("OnEndDrag");
    //}
    #endregion

    #region OldDragNDrop
    private void OnMouseDown()
    {
        Debug.Log(canTarget.ToString());
        if (canTarget)
        {
            Debug.Log("YO!");
            arrowHandler.setVisibile(true);
            arrowHandler.SetOrigin(new Vector2(this.transform.position.x, this.transform.position.y));
        }
        else
        {
            targetController.setPos(false);
            dragOffset = this.transform.position - GetMousePos();
        }

        this.isDragged = true;
    }

    private void OnMouseDrag()
    {
        this.transform.position = GetMousePos() + dragOffset;
    }

    private void OnMouseUp()
    {
        LayerMask dragTarget = LayerMask.GetMask("DragTarget");
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.up, 1, dragTarget.value);
        if (canTarget)
        {
            if (hit.collider != null)
            {
                if (hasBeenPlayed == false && gm.SpendEnergy(energyCost))
                {
                    hasBeenPlayed = true;
                    MoveToDiscardPile(false);
                    hit.transform.GetComponentInParent<Customer>().Feed((ushort)nutritionPoints);
                }
            }
            else
            {
                Debug.Log("Jejda");
            }
            arrowHandler.setVisibile(false);
        }
        else
        {
            if (hit.collider != null)
            {
                if (hasBeenPlayed == false && gm.SpendEnergy(energyCost))
                {
                    hasBeenPlayed = true;
                    MoveToDiscardPile(false);
                }
            }
            targetController.setPos(true);
        }
        this.isDragged = false;
    }
    #endregion

    #region IndexGetSet
    public void SetIndex (int handIndex)
    {
        this.handIndex = handIndex;
    }

    public int GetIndex()
    {
        return this.handIndex;
    }
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
        gm.SendToDiscard(this, isEndTurn);
        gameObject.SetActive(false);
    }
}
