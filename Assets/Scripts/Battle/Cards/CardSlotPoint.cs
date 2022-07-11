using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlotPoint : MonoBehaviour
{
    [SerializeField] int defaultSiblingIdx;
    [SerializeField] CardSlot cardSlot;
    bool pointsToCardSlot = false;
    //[SerializeField] int siblingIdx;


    public CardSlot CardSlot
    {
        set
        {
            cardSlot = value;
            cardSlot.transform.SetSiblingIndex(defaultSiblingIdx);
            pointsToCardSlot = true;
        }
        get
        {
            return cardSlot;
        }
    }

    public bool PointsToCardSlot => pointsToCardSlot;

    public int DefaultSiblingIdx => defaultSiblingIdx;

    public void RemoveCardSlot()
    {
        cardSlot = null;
        pointsToCardSlot = false;
    }

    public void ResetCardSlotPos()
    {
        cardSlot.transform.position = transform.position;
    }

    public void ResetCardSlotSiblingIdx()
    {
        cardSlot.transform.SetSiblingIndex(defaultSiblingIdx);
    }
}
