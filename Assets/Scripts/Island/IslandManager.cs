using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IslandManager : MonoBehaviour
{
    [SerializeField] private int time;
    [SerializeField] TextMeshProUGUI  timeText;
    [SerializeField] private GameObject grabberPrefab;

    public int Time { get => time;}
    
    void Start()
    {
        timeText.text = "Time: " + time;
    }

    void Update()
    {
    }

    public void lowerTime(int lowerBy)
    {
        time -= lowerBy;
        if (time <= 0)
        {
            time = 0;
            Debug.Log("Cas bojovat!");
            // TODO : zamkni ostatne eventy
            // TODO : vytvor instanciu ruky
            //  chod s rukou k hracovi (dlzka ruky je vzdy distance haca od fightu)
            //  vypni hracovi movement
            //  pritiahni hraca k stredu a zacni boj
            Instantiate(grabberPrefab);
        }
        timeText.text = "Time: " + time;
    }
}