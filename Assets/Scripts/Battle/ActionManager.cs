using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class ActionManager : MonoBehaviour
{
    [SerializeField] List<Sprite> Sprites;
    [SerializeField] Customer customer;
    public Image Image;
    public int CurrentIndex { get; private set; }
    public string CurrentName { get; private set; }

    void Awake()
    {
        Suffle();
    }

    public void Suffle()
    {
        Debug.Log(customer.TurnsLeft);
        if(customer.TurnsLeft == 1)
        {
            Image.sprite = Sprites[0];
            CurrentName = Sprites[0].name;
        }
        else
        {
            CurrentIndex = UnityEngine.Random.Range(1, Sprites.Count);
            Image.sprite = Sprites[CurrentIndex];
            CurrentName = Sprites[CurrentIndex].name;
            
        }
    }
}
