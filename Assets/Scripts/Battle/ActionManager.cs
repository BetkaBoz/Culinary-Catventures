using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class ActionManager : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] List<Sprite> sprites;

    public int CurrentIndex { get; private set; }

    void Start()
    {
        CurrentIndex = UnityEngine.Random.Range(0, sprites.Count);
        image.sprite = sprites[CurrentIndex];
    }
}
