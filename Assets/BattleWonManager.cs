using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleWonManager : MonoBehaviour//, IPointerDownHandler
{
    [SerializeField] List<Helper> helpers = new List<Helper>();
    [SerializeField] TextMeshProUGUI reputation;
    [SerializeField] TextMeshProUGUI coins;
    [SerializeField] Button continueButton;

    private Helper currentHelper;

    public void Start()
    {
        gameObject.SetActive(true);
        continueButton.onClick.AddListener(NextLevel);

        for (int i = 0; i < helpers.Count; i++)
        {
            helpers[i].onClick.AddListener(delegate { HelperClicked(i); });
            helpers[i].
        }

        foreach (Helper currentHelper in helpers)
        {

        }

        //reputation.text = $"{customer.rep}";
        //coins.text = $"{customer.money}";
    }

    private void NextLevel()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    private void HelperClicked(int index)
    {
        Debug.Log($"Helper with {index} index was clicked");
    } 

}
