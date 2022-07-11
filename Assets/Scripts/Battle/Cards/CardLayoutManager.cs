using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLayoutManager : MonoBehaviour
{
    [SerializeField] List<CardSlotPoint> transformPoints;
    int numOfLeft, numOfRight;
    [SerializeField] float maxZoom, defaultZoom;
    //[SerializeField] float heightStep; Maybe we can use this later if we want to have more fancy looking hand
    //[SerializeField] float widthStep; IMO the way we have it now looks good
    //[SerializeField] float maxRotation;
    //[SerializeField] float rotationStep;

    //Set position of every Card Slot to the position of the point that points to it
    public void OrderChildren()
    {
        foreach(var point in transformPoints)
        {
            if (point.PointsToCardSlot)
            {
                point.ResetCardSlotPos();
                point.ResetCardSlotSiblingIdx();
                if(!point.CardSlot.Selected)
                    point.CardSlot.transform.localScale = new Vector3(defaultZoom, defaultZoom, 1);
            }
        }
    }

    public void AddChild(CardSlot newChild)
    {
        bool success = false;
        int idx = 0;
        bool leftSide = false;
        //we do this so the Card Slots are in this order
        //10 8 6 4 2 1 3 5 7 9  
        //it's not elegant but it works
        while(idx + 5 <= transformPoints.Count)
        {
            if (leftSide) //check if the next transformPoint is to the left of right of the middle
            {
                if (!transformPoints[5-idx].PointsToCardSlot) //if transformPoint is not pointing to another card slot then make it point to this one
                {
                    success = true;
                    transformPoints[5 - idx].CardSlot = newChild;
                    transformPoints[5 - idx].CardSlot.SlotIndex = 5 - idx; //this is here so highlight doesn't mess up since slot index and hand index will almost always be different
                    numOfLeft++; //this number tracks number of card slots that are left of cencer. It is only used in RemoveCard
                    break;
                }
                leftSide = false;
            }
            else
            {
                if (idx + 5 == transformPoints.Count)                       //because the max number of cards is even we can't really split it in two 
                {                                                           //this could technically lead to us trying to access transformPoint that doesn't exit
                    Debug.Log("That's one too many cards there buddy");     //since we have only 10 Card Slots this shouldn't happen
                    break;
                }
                if (!transformPoints[5 + idx].PointsToCardSlot) //if transformPoint is not pointing to another card slot then make it point to this one
                {
                    success = true;
                    transformPoints[5 + idx].CardSlot = newChild;
                    transformPoints[5 + idx].CardSlot.SlotIndex = 5 + idx; //this is here so highlight doesn't mess up since slot index and hand index will almost always be different
                    numOfRight++;//this number tracks number of card slots that are right of cencer. It is only used in RemoveCard
                    break;
                }
                leftSide = true;
                idx++;
            }
        }
        if (!success)//just another little check for us (maybe do something with this later?)
            Debug.Log("There's no space left!");
        OrderChildren();//after we add a card slot reorder them
    }

    public void RemoveChild(CardSlot unwantedChild)
    {
        bool success = false;
        int childIdx = 0;
        for(int i = 0; i < transformPoints.Count; i++)
        {
            if (transformPoints[i].CardSlot == unwantedChild)
            {
                success = true;
                childIdx = i;
                break;
            }
        }
        if (!success)
            Debug.Log("Couldn't find the slot!");
        else
        {
            Debug.Log("Success childIdx: "+childIdx);
            int removeIdx = childIdx;
            Debug.Log(numOfLeft + " " + numOfRight);
            //if (childIdx < 5)
            //{
                if(numOfLeft < numOfRight)
                {
                    Debug.Log("Move Right to Left");
                    numOfRight--;
                    for(int i = childIdx; i < transformPoints.Count-1; i++)
                    {
                        if (!transformPoints[i+1].PointsToCardSlot)
                        {
                            Debug.Log(i);
                            break;
                        }
                        transformPoints[i].CardSlot = transformPoints[i+1].CardSlot;
                        transformPoints[i].CardSlot.SlotIndex = i;
                        removeIdx = i+1;
                    }
                }
                else
                {
                    Debug.Log("Move Left");
                    numOfLeft--;
                    for (int i = childIdx; i > 0; i--)
                    {
                        if (!transformPoints[i-1].PointsToCardSlot)
                        {
                            Debug.Log(i);
                            break;
                        }
                        transformPoints[i].CardSlot = transformPoints[i-1].CardSlot;
                        transformPoints[i].CardSlot.SlotIndex = i;
                        removeIdx = i-1;
                    }
                }
            //}
            //else
            //{
            //    if (numOfLeft < numOfRight)
            //    {
            //        Debug.Log("Move Left to Right");
            //        numOfLeft--;
            //        numOfRight++;
            //        for (int i = childIdx - 1; i < transformPoints.Count; i++)
            //        {
            //            if (!transformPoints[i].PointsToCardSlot)
            //            {
            //                Debug.Log(i);
            //                removeIdx = i + 1;
            //                break;
            //            }
            //            transformPoints[i + 1].CardSlot = transformPoints[i].CardSlot;
            //        }
            //        if (removeIdx == -1) { removeIdx = 9; }
            //    }
            //    else
            //    {
            //        Debug.Log("Move Right");
            //        numOfRight--;
            //        for (int i = childIdx + 1; i > 0; i--)
            //        {
            //            if (!transformPoints[i].PointsToCardSlot)
            //            {
            //                Debug.Log(i);
            //                removeIdx = i - 1;
            //                break;
            //            }
            //            transformPoints[i - 1].CardSlot = transformPoints[i].CardSlot;
            //        }
            //        if (removeIdx == -1) { removeIdx = 0; }
            //    }
            //}
            Debug.Log(numOfLeft + " " + numOfRight + " removeIdx" + removeIdx);
            transformPoints[removeIdx].RemoveCardSlot();
        }
        OrderChildren();
    }

    public void HighlightChild(int idx)
    {
        if (!transformPoints[idx].PointsToCardSlot) return;
        if (idx < transformPoints.Count-1)
        {
            int siblingIdx = transformPoints[idx].DefaultSiblingIdx; //store the sibling index of our highlighted card slot
            for (int i = idx + 1; i < transformPoints.Count; i++)                               //adjust sibling index of all the neighbours so the highlighted card is on the top
            {                                                                                   //since indexes of the left neighbours will always be ordered correctly we only udjust the right ones
                if (transformPoints[i].PointsToCardSlot)
                {
                    int newSiblingIdx = (siblingIdx - (i - idx));
                    if (newSiblingIdx < 0) newSiblingIdx = 0;
                    transformPoints[i].CardSlot.transform.SetSiblingIndex(newSiblingIdx);           //idexes of the neighbours will look like this (s = sibling index of highlighted card)
                }                                                                                   // (s-5) (s-4) (s-3) (s-2) (s-1) (s) (s-1) (s-2) (s-3) (s-4)
            }
        }
        transformPoints[idx].CardSlot.transform.localScale = new Vector3(maxZoom, maxZoom, 1f);
        transformPoints[idx].CardSlot.transform.position += Vector3.up * 0.25f;
    }

    //I honestly don't know if this is even needed
    public void Awake()
    {
        numOfRight = 0;
        numOfLeft = 0;
    }
}
