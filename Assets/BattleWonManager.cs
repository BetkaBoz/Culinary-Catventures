using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleWonManager : MonoBehaviour
{
    [SerializeField] List<Helper> helpers = new List<Helper>();
    [SerializeField] TextMeshProUGUI reputation;
    [SerializeField] TextMeshProUGUI coins;
    [SerializeField] Button continueButton;
    [SerializeField] Customer customer;
    [SerializeField] Player player;

    //public Player Player => player;
    public void Start()
    {
        gameObject.SetActive(true);
        continueButton.onClick.AddListener(NextLevel);

        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();

        reputation.text = $"+{player.RepAmount}";
        coins.text = $"+{player.MoneyAmount}";
    }

    private void NextLevel()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
