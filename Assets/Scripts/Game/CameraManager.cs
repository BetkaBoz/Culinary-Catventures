using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    private Camera mainCamera;

    public void Awake()
    {
        //mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mainCamera = Camera.main;
        canvas.worldCamera = mainCamera;
    }
}
