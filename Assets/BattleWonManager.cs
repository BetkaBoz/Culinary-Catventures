using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleWonManager : MonoBehaviour
{
    [SerializeField] List<Helper> helpers = new List<Helper>();
    [SerializeField] TextMeshProUGUI reputation;
    [SerializeField] TextMeshProUGUI coins;
    [SerializeField] Button continueButton;
    [SerializeField] Customer customer;

    public void Start()
    {
        gameObject.SetActive(true);
        continueButton.onClick.AddListener(NextLevel);
        reputation.text = $"{customer.rep}";
        coins.text = $"{customer.money}";
    }

    private void NextLevel()
    {
        Debug.Log("funny text haha");
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        
    }
}
