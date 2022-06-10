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
    [SerializeField] ActionManager ac;
    public string message, header;

    private void Start()
    {
        switch (ac.CurrentIndex)
        {
            case 0:
                message = "If customer won't be fed this round, they will take your reputation points";
                header = "Reputation Debuff";
                break;
            case 1:
                message = "If customer won't be fed this round, they will cause less EP in next round";
                header = "Energy Debuff";
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //TooltipManager.Show(message, header);

        tooltip.DOFade(0.7f, 0.2f).OnPlay(() =>
           {
               TooltipManager.Show(message, header);
           });
               
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //TooltipManager.Hide();

        tooltip.DOFade(0f, 0.2f).OnPlay(() =>
            {
                TooltipManager.Hide();
            });
    }

}
