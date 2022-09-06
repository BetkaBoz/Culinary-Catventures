using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI repUI;
    [SerializeField] private TextMeshProUGUI coinUI;
    [SerializeField] Image coinNotification;
    [SerializeField] Image repNotification;

    private Player player;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Player>();
    }

    public void ChangeMoney(int amount)
    {
        player.ChangeMoney(amount);
        UpdateUI();
    }

    //CHANGED -= TO +=, USE THIS INSTEAD
    public void ChangeReputation(int amount)
    {
        player.ChangeReputation(amount);
        if (player.rep < 30)
        {
            ShowReputationNotification();
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        repUI.text = $"{player.rep}";
        coinUI.text = $"{player.Money}";
    }

    public void ShowCoinsNotification()
    {
        coinNotification.gameObject.SetActive(true);
        coinNotification.DOFade(1f, 1f).OnComplete(() => {
            coinNotification.DOFade(0, 2f).OnComplete(() =>
            {
                coinNotification.gameObject.SetActive(false);
            });
        });
    }
    private void ShowReputationNotification()
    {
        repNotification.gameObject.SetActive(true);
        TextMeshProUGUI text = repNotification.GetComponentInChildren<TextMeshProUGUI>();
        text.DOFade(1f, 1f).OnComplete(() => {
            text.DOFade(0, 2f).OnComplete(() =>
            {
                text.gameObject.SetActive(false);
            });
        });
        repNotification.DOFade(1f, 1f).OnComplete(() => {
            repNotification.DOFade(0, 2f).OnComplete(() =>
            {
                repNotification.gameObject.SetActive(false);
            });
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
    }
}