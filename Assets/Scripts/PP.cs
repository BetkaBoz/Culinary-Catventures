using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP : MonoBehaviour
{
    public static PP Instance { get; set; }

    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // StartCoroutine(Test());
        }

    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
