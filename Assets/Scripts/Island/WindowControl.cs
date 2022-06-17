using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class WindowControl : MonoBehaviour
{
    [SerializeField] protected IslandManager islandManager;
    [SerializeField] protected UILayer uiLayer;
    [SerializeField] protected Player player;
    [SerializeField] private List<CardBaseInfo> allFoodScriptableObjects;

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
        GetAllFoodCards();
        HideWindow();
    }

   
    
    public void Init(int timeCost)
    {
        //Debug.Log("TimeCost initialized to " + timeCost);
        this.timeCost = timeCost;
    }
    // Start is called before the first frame update
    
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
        //Debug.Log("CLOSE");
    }
    private void GetAllFoodCards()
    {
        CardBaseInfo[] allScriptableObjectsTemp = Resources.LoadAll<CardBaseInfo>("Scriptable Objects/");
        foreach (var t in allScriptableObjectsTemp)
        {
            if (t.CardType != "Manoeuvre" && !t.CardName.Contains("Pile") )
            {
                allFoodScriptableObjects.Add(t);
            }
        }
    }
    protected CardBaseInfo GenerateRandomIngredient()
    {
        int random = Random.Range(0, allFoodScriptableObjects.Count);
        return allFoodScriptableObjects[random];
    }
}
