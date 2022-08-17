using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class WindowControl : MonoBehaviour
{
    [SerializeField] protected IslandManager islandManager;
    [SerializeField] protected UILayer uiLayer;
    [SerializeField] protected Player player;
    [SerializeField] private List<CardBaseInfo> allIngredientScriptableObjects;
    [SerializeField] private List<CardBaseInfo> allManoeuvreScriptableObjects;
    [SerializeField] private List<CardBaseInfo> allFoodScriptableObjects;

     private static int TimeCost = 1;
    
    //DONT USE AWAKE IN CHILDREN CLASS CAUSE IT WILL OVERRIDE METHOD IN PARENT
    private void Awake()
    {
        islandManager = FindObjectOfType<IslandManager>();
        uiLayer = FindObjectOfType<UILayer>();
        player = FindObjectOfType<Player>();
        GetAllCards();

    }
    
    public void Init(int timeCost)
    {
        //Debug.Log("TimeCost initialized to " + timeCost);
        TimeCost = timeCost;
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
        EventManager.IsInEvent = false;
        islandManager.LowerTime(TimeCost);
        HideWindow();
        
    }
    //TODO PREMIESTNIT KOD ABY SA NEOPAKOVAL V OKNACH
    private void GetAllCards()
    {
        /*
            if (t.CardType == "Manoeuvre" )
            {
                allManoeuvreScriptableObjects.Add(t);
            }
            else if ( !t.CardName.Contains("Pile") )
            {
                allIngredientScriptableObjects.Add(t);
            }*/
        CardBaseInfo[] allScriptableObjectsTemp = Resources.LoadAll<CardBaseInfo>("Scriptable Objects/Manouvers");
        foreach (CardBaseInfo t in allScriptableObjectsTemp)
        {
            allManoeuvreScriptableObjects.Add(t);
        }
        allScriptableObjectsTemp = Resources.LoadAll<CardBaseInfo>("Scriptable Objects/Ingredients");
        foreach (CardBaseInfo t in allScriptableObjectsTemp)
        {
            allIngredientScriptableObjects.Add(t);
        }
        allScriptableObjectsTemp = Resources.LoadAll<CardBaseInfo>("Scriptable Objects/Food");
        foreach (CardBaseInfo t in allScriptableObjectsTemp)
        {
            allFoodScriptableObjects.Add(t);
        }
    }
    
    protected CardBaseInfo GetRandomIngredient()
    {
        int random = Random.Range(0, allIngredientScriptableObjects.Count);
        return allIngredientScriptableObjects[random];
    }
    protected CardBaseInfo GetRandomFood()
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
        return allIngredientScriptableObjects.Find(x => x.CardName == cardName);
    }
}
