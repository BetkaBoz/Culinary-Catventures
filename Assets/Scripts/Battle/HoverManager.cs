using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class HoverManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image bubbleAwait;
    [SerializeField] Image bubbleAction;

    public void OnPointerEnter(PointerEventData eventData)
    {
        bubbleAwait.DOFade(0f, 0.15f).OnComplete(() =>
        {
            bubbleAction.DOFade(1f, 0.15f).OnPlay(() =>
            {
                bubbleAction.gameObject.SetActive(true);
            });
        });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        bubbleAction.DOFade(0f, 0.15f).OnPlay(() =>
        {
            bubbleAwait.DOFade(1f, 0.15f).OnComplete(() =>
            {
                bubbleAwait.gameObject.SetActive(true);
            });
        });
    }

}
