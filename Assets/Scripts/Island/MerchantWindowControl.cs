using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MerchantWindowControl : WindowControl
{
    [SerializeField] private List<GameObject> merchantCards;

    //DONT USE AWAKE CAUSE IT WILL OVERRIDE FROM PARENT CLASS

    
    
    
   
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
            nutritionalValue.text = $"{randomCard.NutritionPoints}";
            
            //CARD PRICE
            if (int.Parse(nutritionalValue.text) > 8)
            {
                price.text =  $"{Random.Range(10, 16)}"; 
            }
            else
            {
                price.text =  $"{Random.Range(5, 11)}";  
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
    
    

    public void StartWindow()
    {
        AssignMerchantCards();
        ShowWindow();
    }

    
    
    //IN INSPECTOR 

}
