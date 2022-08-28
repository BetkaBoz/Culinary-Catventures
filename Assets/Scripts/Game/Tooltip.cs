using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI tooltipHeader;
    public TextMeshProUGUI tooltipText;
    public LayoutElement layoutElement;
    public int characterWrapLimit;
    public RectTransform rectTransform;
    public Image image;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            int headLength = tooltipHeader.text.Length;
            int messageLength = tooltipText.text.Length;

            layoutElement.enabled = (headLength > characterWrapLimit || messageLength > characterWrapLimit) ? true : false;
        }

        Vector2 position = Input.mousePosition;

        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;

        float finalPivotX = 0f;
        float finalPivotY = 0f;

        //if mouse is close to left / right border move tooltip to the other side
        if (pivotX < 0.5) finalPivotX = -0.1f;
        else finalPivotX = 1.01f;

        //if mouse is close to upper / down border move tooltip to the other side
        if (pivotY < 0.5) finalPivotY = 0;
        else finalPivotY = 1;

        rectTransform.pivot = new Vector2(finalPivotX, finalPivotY);
        transform.position = (Vector2) Camera.main.ScreenToWorldPoint(position);
    }

    public void SetText(string message, string header)
    {
        if (string.IsNullOrEmpty(header)) tooltipHeader.gameObject.SetActive(false);
        else
        {
            tooltipHeader.gameObject.SetActive(true);
            tooltipHeader.text = header;
        }

        tooltipText.text = message;

        int headLength = tooltipHeader.text.Length;
        int messageLength = tooltipText.text.Length;

        layoutElement.enabled = (headLength > characterWrapLimit || messageLength > characterWrapLimit) ? true : false;
    }
}
