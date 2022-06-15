using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class HoverManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image tooltip;
    [SerializeField] Helper helper;
    [SerializeField] ActionManager ac;
    public string message, header;

    private void Start()
    {
        switch (ac.CurrentName)
        {
            case "rep debuff":
                message = "If customer won't be fed this round, they will take your Reputation Points";
                header = "Reputation Debuff";
                break;
            case "energy debuff":
                message = "If customer won't be fed this round, they will cause Energy Points loss for the next round";
                header = "Energy Debuff";
                break;
            case "helper_1":
                message = "They're nothing special, good vibes only";
                header = "Basic Helper";
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        switch (gameObject.tag)
        {
            case "Action":
                tooltip.DOFade(0.7f, 0.2f).OnPlay(() => { TooltipManager.Show(message, header); });
                break;
            case "Helper":
                helper.transform.DOScale(1.2f, 0.5f);
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        switch (gameObject.tag)
        {
            case "Action":
                tooltip.DOFade(0f, 0.2f).OnPlay(() => { TooltipManager.Hide(); });
                break;
            case "Helper":
                helper.transform.DOScale(1f, 0.5f);
                break;
        }
    }
}
