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

    public static void Show(string message, string header, float endValue, float duration)
    {
        tm.tooltip.SetText(message, header);
        tm.tooltip.gameObject.SetActive(true);
        tm.tooltip.image.DOFade(endValue, duration);
    }

    public static void Hide(float duration = 0.2f)
    {
        tm.tooltip.gameObject.SetActive(false);
        tm.tooltip.image.DOFade(0f, duration);
    }
}
