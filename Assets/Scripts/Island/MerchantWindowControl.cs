using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MerchantWindowControl : MonoBehaviour
{
    [SerializeField] private List<GameObject> merchantCards;
    [SerializeField] private List<CardBaseInfo> allFoodScriptableObjects;
    
    private UILayer uiLayer;
    private Player player;
    private IslandManager islandManager;

    private void Awake()
    {
        islandManager = FindObjectOfType<IslandManager>();
        uiLayer = GameObject.FindGameObjectWithTag("UILayer").GetComponent<UILayer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        GetAllFoodCards();
        //AssignMerchantCards();
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
    private void AssignMerchantCards()
    {
        foreach (GameObject card in merchantCards)
        {   

            Image artwork = card.GetComponent<Image>();
            Text nutritionalValue = card.GetComponentInChildren<Text>();
            TextMeshProUGUI price = card.GetComponentInChildren<TextMeshProUGUI>();
            Button button = card.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            artwork.color= Color.white;

            CardBaseInfo randomCard = GenerateRandomIngredient();
            artwork.sprite = randomCard.Artwork;
            nutritionalValue.text = randomCard.NutritionPoints.ToString();
            
            //CARD PRICE
            if (int.Parse(nutritionalValue.text) > 8)
            {
                price.text = Random.Range(10, 15).ToString();
            }
            else
            {
                price.text = Random.Range(5, 10).ToString();
            }
            //BUY CARD
            button.onClick.AddListener(delegate {
                uiLayer.ChangeMoney(- int.Parse(price.text));
                player.Deck.Add(randomCard);
                artwork.color= Color.gray;
                button.onClick.RemoveAllListeners();
            });


        }
    }
    
    private CardBaseInfo GenerateRandomIngredient()
    {
        
        int random = Random.Range(0, allFoodScriptableObjects.Count);
        return allFoodScriptableObjects[random];
        
    }
    
    
    public void ShowWindow()
    {
        gameObject.SetActive(true);
        AssignMerchantCards();
    }
    
    //IN INSPECTOR
    public void HideWindow()
    {
        gameObject.SetActive(false);
    }
    public void CloseEvent()
    {
        //RemoveAllListeners();
        Time.timeScale = 1;
        islandManager.LowerTime(1);
        HideWindow();
        //Debug.Log("CLOSE");
    }
}
