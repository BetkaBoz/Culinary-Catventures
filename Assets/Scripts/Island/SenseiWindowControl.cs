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
            Button button = card.GetComponent<Button>();
            TextMeshProUGUI energyCost = card.transform.Find("Energy").gameObject.GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI price = card.transform.Find("Coin").gameObject.GetComponentInChildren<TextMeshProUGUI>();

            button.onClick.RemoveAllListeners();
            artwork.color = Color.white;

            CardBaseInfo randomCard = GetRandomManoeuvre();
            artwork.sprite = randomCard.Artwork;
            energyCost.text = $"{randomCard.EnergyCost}";


            //CARD PRICE
            price.text = $"{Random.Range(20, 41)}";

            //BUY CARD
            button.onClick.AddListener(delegate {
                if (player.HaveMoney(int.Parse(price.text)))
                {
                    uiLayer.ChangeMoney(-int.Parse(price.text));
                    player.Deck.Add(randomCard);
                    artwork.color = Color.gray;
                    button.onClick.RemoveAllListeners();
                }
                else
                {
                    uiLayer.ShowCoinsNotification();
                }

            });


        }
    }
    public void StartWindow()
    {
        ShowWindow();

        AssignSenseiCards();
    }
}
