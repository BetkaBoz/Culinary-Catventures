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
    [SerializeField] GameObject helperParent;
    [SerializeField] TextMeshProUGUI reputation;
    [SerializeField] TextMeshProUGUI coins;
    [SerializeField] Button continueButton;
    [SerializeField] Canvas canvas;
    [SerializeField] Text notification;
    private Player player;
    private GameObject currentHelper;
    private List<GameObject> helpers = new List<GameObject>();

    public void Start()
    {
        canvas.worldCamera = Camera.main;

        gameObject.SetActive(true);
        continueButton.onClick.AddListener(NextLevel);

        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();

        //cycle for dynamically getting all helpers to choose from
        for(int i = 0; i < helperParent.transform.childCount; i++)
        {
            helpers.Add(helperParent.transform.GetChild(i).gameObject);
        } 

        reputation.text = $"+{player.earnedRep}";
        coins.text = $"+{player.MoneyAmount}";
    }

    private void NextLevel()
    {
        if(currentHelper == null)
        {
            Debug.Log("You have to choose Helper ma dude");
            notification.gameObject.SetActive(true);
            return;
        }

        string helperName = currentHelper.GetComponent<ActionManager>().CurrentName;
        SceneManager.LoadScene(3, LoadSceneMode.Single);
    }

    public void HelperSelected(GameObject selected)
    {
        currentHelper = selected;
        currentHelper.GetComponent<HelperSelection>().Select();

        notification.gameObject.SetActive(false);

        foreach (GameObject helper  in helpers)
        {
            if(helper != currentHelper) helper.GetComponent<HelperSelection>().Deselect();
        }
    }
}
