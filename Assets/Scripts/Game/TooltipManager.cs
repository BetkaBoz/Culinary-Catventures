using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    }

    public static void Hide()
    {
        tm.tooltip.gameObject.SetActive(false);
    }
}
