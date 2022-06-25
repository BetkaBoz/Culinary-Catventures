using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class WindowControl : MonoBehaviour
{
    [SerializeField] protected IslandManager islandManager;
    [SerializeField] protected UILayer uiLayer;
    [SerializeField] protected Player player;
    [SerializeField] private List<CardBaseInfo> allFoodScriptableObjects;
    [SerializeField] private List<CardBaseInfo> allManoeuvreScriptableObjects;

    [SerializeField] private int timeCost = 1;
    
    //DONT USE AWAKE IN CHILDREN CLASS CAUSE IT WILL OVERRIDE METHOD IN PARENT
    private void Awake()
    {
        islandManager = FindObjectOfType<IslandManager>();
        uiLayer = FindObjectOfType<UILayer>();
        player = FindObjectOfType<Player>();

    }
    void Start()
    {
        GetAllCards();
        HideWindow();
    }

   
    
    public void Init(int timeCost)
    {
        //Debug.Log("TimeCost initialized to " + timeCost);
        this.timeCost = timeCost;
    }
    
    private void HideWindow()
    {
        gameObject.SetActive(false);
    }
    protected void ShowWindow()
    {
        gameObject.SetActive(true);
    }
    
    public void CloseEvent()
    {
        Time.timeScale = 1;
        islandManager.LowerTime(timeCost);
        HideWindow();
    }
    private void GetAllCards()
    {
        CardBaseInfo[] allScriptableObjectsTemp = Resources.LoadAll<CardBaseInfo>("Scriptable Objects/");
        foreach (var t in allScriptableObjectsTemp)
        {
            if (t.CardType == "Manoeuvre" )
            {
                allManoeuvreScriptableObjects.Add(t);
            }
            else if ( !t.CardName.Contains("Pile") )
            {
                allFoodScriptableObjects.Add(t);
            }
        }
    }
    
    protected CardBaseInfo GetRandomIngredient()
    {
        int random = Random.Range(0, allFoodScriptableObjects.Count);
        return allFoodScriptableObjects[random];
    }
    
    protected CardBaseInfo GetRandomManoeuvre()
    {
        int random = Random.Range(0, allManoeuvreScriptableObjects.Count);
        return allManoeuvreScriptableObjects[random];
    }
    protected CardBaseInfo GetIngredient(string cardName)
    {
        return allFoodScriptableObjects.Find(x => x.CardName == cardName);
    }
}
