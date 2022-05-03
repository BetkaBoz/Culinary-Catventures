using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IslandManager : MonoBehaviour
{
    [SerializeField] private int time;
    [SerializeField] TextMeshProUGUI  timeText;

    void Start()
    {
    }

    void Update()
    {
    }

    public void lowerTime(int lowerBy)
    {
        this.time -= lowerBy;
        if (time <= 0)
        {
            time = 0;
            Debug.Log("Cas bojovat!");
            // TODO : zamkni ostatne eventy
        }
        timeText.text = "Time: " + time;
    }
}