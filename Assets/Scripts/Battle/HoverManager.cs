using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image bubbleAwait;
    [SerializeField] Image bubbleAction;

    private bool entered;

    public void OnPointerEnter(PointerEventData eventData)
    {
        bubbleAwait.enabled = false;
        bubbleAction.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        bubbleAwait.enabled = true;
        bubbleAction.gameObject.SetActive(false);
    }

}
