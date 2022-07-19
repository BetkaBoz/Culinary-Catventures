using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

public class HoverManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] HelperSelection helper;
    [SerializeField] Hoverable hoverable;
    public string message = "", header = "";

    private void Start()
    {
        if(hoverable != null)
        {
            hoverable.OnTooltipChanged += UpdateMessage;
        }
    }
    
    private void Update()
    {
        UpdateMessage();
    }

    private void UpdateMessage()
    {
        if (hoverable != null)
        {
            message = hoverable.Message;
            header = hoverable.Header;
        }

        switch(gameObject.name)
        {
            case "helper_1":
                message = "They're nothing special, good vibes only";
                header = "Basic Helper";
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverable != null && !hoverable.TooltipEnabled) return;
        switch (gameObject.tag)
        {
            case "Action":
                TooltipManager.Show(message, header, 0.7f, 0.2f);
                break;
            case "Helper":
                helper.transform.DOScale(1.2f, 0.5f).OnPlay(() => { TooltipManager.Show(message, header, 0.7f, 0.2f); });
                break;
            default:
                TooltipManager.Show(message, header, 0.7f, 0.2f);
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        switch (gameObject.tag)
        {
            case "Action":
                TooltipManager.Hide();
                break;
            case "Helper":
                helper.transform.DOScale(1f, 0.5f).OnPlay(() => { TooltipManager.Hide(); });
                break;
            default:
                TooltipManager.Hide();
                break;
        }
    }
    private void OnDestroy()
    {
        if(hoverable != null)
        {
            hoverable.OnTooltipChanged -= UpdateMessage;
        }
    }
}
