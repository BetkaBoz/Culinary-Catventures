using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HighlightObjectController : MonoBehaviour
{
    [SerializeField] Material highlightMaterial;
    [SerializeField] Image highlightImage;
    bool dontIgnoreExit = false;

    private void Awake()
    {
        highlightImage = GetComponent<Image>();
        highlightMaterial = highlightImage.material;
        highlightImage.material = null;
        //highlightMaterial.color = new Color(highlightMaterial.color.r, highlightMaterial.color.g, highlightMaterial.color.b, 0);
    }

    public void ToggleHighlight(bool turnOn)
    {
        if (turnOn)
        {
            highlightImage.material = highlightMaterial;
        }
        else
        {
            highlightImage.material = null;
        }
    }
}
