using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoverable: MonoBehaviour
{
    string _message;
    string _header;
    public string Message => _message;
    public string Header => _header;

    public event Action OnTooltipChanged;

    public void SetMessageHeader(string message, string header)
    {
        _message = message;
        _header = header;

        OnTooltipChanged?.Invoke();
    }
}
