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
        UpdateUI();
    }

    private void UpdateUI()
    {
        repUI.text = $"{player.rep}";
        coinUI.text = $"{player.Money}";
    }

    public void ShowNotification()
    {
        coinNotification.gameObject.SetActive(true);
        coinNotification.DOFade(1f, 1f).OnComplete(() => {
            coinNotification.DOFade(0, 2f).OnComplete(() =>
            {
                coinNotification.gameObject.SetActive(false);
            });
        });
    }
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
    }
}