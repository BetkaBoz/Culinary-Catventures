using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HelperSelection : MonoBehaviour
{
    [SerializeField] Image helper;

    public void Select()
    {
        helper.DOFade(1f, 0.2f);

    }

    public void Deselect()
    {
        helper.DOFade(0.5f, 0.2f);
    }
}
