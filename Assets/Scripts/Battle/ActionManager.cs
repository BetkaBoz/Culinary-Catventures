using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class ActionManager : MonoBehaviour
{
    public Image Image;
    [SerializeField] List<Sprite> Sprites;

    public int CurrentIndex { get; private set; }

    void Awake()
    {
        CurrentIndex = UnityEngine.Random.Range(0, Sprites.Count);
        Image.sprite = Sprites[CurrentIndex];
    }
}
