using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private bool hasBeenPlayed;
    private int handIndex;
    private GameManager gm;
    private bool isDragged = true;
    [SerializeField] private int energyCost;
    [SerializeField] private int nutritionPoints;
    [SerializeField] private bool canTarget = false;
    [SerializeField] private ArrowHandler arrowHandler;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void OnMouseDown()
    {
        Debug.Log(canTarget.ToString());
        if (canTarget)
        {
            Debug.Log("YO!");
            arrowHandler.setVisibile(true);
            arrowHandler.SetOrigin(new Vector2(this.transform.position.x, this.transform.position.y));
        }

        this.isDragged = true;
    }

    private void OnMouseUp()
    {
        if (hasBeenPlayed == false && gm.SpendEnergy(energyCost))
        {
            hasBeenPlayed = true;
            MoveToDiscardPile(false);
        }
        if (canTarget)
        {
            arrowHandler.setVisibile(false);
        }
    }

    public void SetIndex (int handIndex)
    {
        this.handIndex = handIndex;
    }

    public int GetIndex()
    {
        return this.handIndex;
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
