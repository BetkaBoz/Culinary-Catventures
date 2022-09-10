using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    [SerializeField] static TooltipManager tm;
    public Tooltip tooltip;

    private void Awake()
    {
        tm = this;
        tooltip.gameObject.SetActive(false);
    }

    public static void Show(string message, string header, float endValue, float duration, Action onComplete =
        null, float onCompleteDelay = 0)
    {
        tm.tooltip.SetText(message, header);
        tm.tooltip.gameObject.SetActive(true);
        tm.tooltip.image.DOFade(endValue, duration);
        tm.tooltip.tooltipHeader.DOFade(endValue, duration);
        tm.tooltip.tooltipText.DOFade(endValue, duration).OnComplete(() =>
        {
            if (onComplete != null) onComplete();
        }).SetDelay(onCompleteDelay);
    }

    public static void Hide(float duration = 0.2f)
    {
        tm.tooltip.image.DOFade(0f, duration);
        tm.tooltip.tooltipHeader.DOFade(0f, duration);
        tm.tooltip.tooltipText.DOFade(0f, duration).OnComplete(() => tm.tooltip.gameObject.SetActive(false));
    }
}
