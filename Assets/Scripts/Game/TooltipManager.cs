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

    public static void Show(string message, string header)
    {
        tm.tooltip.SetText(message, header);
        tm.tooltip.gameObject.SetActive(true);
        tm.tooltip.image.DOFade(0.7f, 0.2f);
    }

    public static void Hide()
    {
        tm.tooltip.gameObject.SetActive(false);
        tm.tooltip.image.DOFade(0f, 0.2f);
    }
}
