using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private bool hasBeenPlayed;
    private int handIndex;
    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void OnMouseDown()
    {
        if(hasBeenPlayed == false)
        {
            hasBeenPlayed = true;
            MoveToDiscardPile();
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

    private void MoveToDiscardPile()
    {
        gm.SendToDiscard(this);
        gameObject.SetActive(false);
    }
}
