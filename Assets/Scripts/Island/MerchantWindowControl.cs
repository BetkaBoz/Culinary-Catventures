using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MerchantWindowControl : WindowControl
{
    [SerializeField] private List<GameObject> merchantCards;
    public static bool IsInMerchant;

    //DONT USE AWAKE CAUSE IT WILL OVERRIDE FROM PARENT CLASS


    private void AssignMerchantCards()
    {
        foreach (GameObject card in merchantCards)
        {

            Image artwork = card.GetComponent<Image>();
            Text nutritionalValue = card.GetComponentInChildren<Text>();
            GameObject energy = card.transform.Find("Energy").gameObject;
            TextMeshProUGUI energyCost = energy.GetComponentInChildren<TextMeshProUGUI>();
            GameObject coin = card.transform.Find("Coin").gameObject;
            TextMeshProUGUI price = coin.GetComponentInChildren<TextMeshProUGUI>();

            Button button = card.GetComponent<Button>();

            button.onClick.RemoveAllListeners();
            artwork.color = Color.white;

            CardBaseInfo randomCard = GetRandomIngredient();
            artwork.sprite = randomCard.Artwork;
            nutritionalValue.text = $"{randomCard.NutritionPoints}";
            energyCost.text = $"{randomCard.EnergyCost}";
            //CARD PRICE
            if (int.Parse(nutritionalValue.text) > 8)
            {
                price.text = $"{Random.Range(10, 16)}";
            }
            else
            {
                price.text = $"{Random.Range(5, 11)}";
            }
            //BUY CARD
            button.onClick.AddListener(delegate {
                if (!player.HaveMoney(int.Parse(price.text)))
                {
                    uiLayer.ShowCoinsNotification();
                    return;
                }
                uiLayer.ChangeMoney(-int.Parse(price.text));
                player.Deck.Add(randomCard);
                artwork.color = Color.gray;
                price.color = Color.gray;
                coin.GetComponentInChildren<Image>().color = Color.gray;
                nutritionalValue.color = Color.gray;
                foreach (Transform child in energy.transform)
                {
                    //print("Foreach loop: " + child);
                    if (child.GetComponent<Image>())
                    {
                        child.GetComponent<Image>().color = Color.grey;
                    }
                    else if (child.GetComponent<TextMeshProUGUI>())
                    {
                        child.GetComponent<TextMeshProUGUI>().color = Color.gray;
                    }
                }

                Destroy(button);
                

            });
        }
    }



    public void StartWindow()
    {
        ShowWindow();
        IsInMerchant = true;
        AssignMerchantCards();
    }

    private void SellIngredients()
    {
        
    }

    //IN INSPECTOR 

}
