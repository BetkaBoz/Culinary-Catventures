using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SenseiWindowControl : WindowControl
{
    [SerializeField] private List<GameObject> senseiCards;

    private void AssignSenseiCards()
    {
        foreach (GameObject card in senseiCards)
        {   

            Image artwork = card.GetComponent<Image>();
            //Text nutritionalValue = card.GetComponentInChildren<Text>();
            TextMeshProUGUI price = card.GetComponentInChildren<TextMeshProUGUI>();
            Button button = card.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            artwork.color= Color.white;

            CardBaseInfo randomCard = GetRandomManoeuvre();
            artwork.sprite = randomCard.Artwork;
            //nutritionalValue.text = $"{randomCard.NutritionPoints}";
            
            //CARD PRICE
            price.text =  $"{Random.Range(20, 41)}"; 

            //BUY CARD
            button.onClick.AddListener(delegate {
                if (player.HaveMoney(int.Parse(price.text)))
                {
                    uiLayer.ChangeMoney(- int.Parse(price.text));
                    player.Deck.Add(randomCard);
                    artwork.color= Color.gray;
                    button.onClick.RemoveAllListeners();
                }
                
            });


        }
    }
    public void StartWindow()
    {
        AssignSenseiCards();
        ShowWindow();
    }
}
