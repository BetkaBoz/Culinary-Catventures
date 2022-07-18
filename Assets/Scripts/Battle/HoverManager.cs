using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class HoverManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] HelperSelection helper;
    //[SerializeField] CustomerView customerView;
    [SerializeField] Customer customer;
    public string message, header;

    private void Update()
    {
        if (customer == null) return;
            switch (customer.CurrentAction)
            {
                case 0:
                    message = "Customer will leave next round!";
                    header = "";
                break;
                case 1:
                    message = "If customer won't be fed this round, they will take your Reputation Points";
                    header = "Reputation Debuff";
                    break;
                case 2:
                    message = "If customer won't be fed this round, they will cause Energy Points loss for the next round";
                    header = "Energy Debuff";
                    break;
                //case "helper_1":
                //    message = "They're nothing special, good vibes only";
                //    header = "Basic Helper";
                //    break;
            }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        switch (gameObject.tag)
        {
            case "Action":
                TooltipManager.Show(message, header);
                break;
            case "Helper":
                helper.transform.DOScale(1.2f, 0.5f).OnPlay(() => { TooltipManager.Show(message, header); });
                break;
            default:
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
                break;
        }
    }
}
