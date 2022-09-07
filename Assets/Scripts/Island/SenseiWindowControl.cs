using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SenseiWindowControl : WindowControl
{
    [SerializeField] private List<GameObject> senseiCards;
    public static bool IsInSensei;

    private void AssignSenseiCards()
    {
        foreach (GameObject card in senseiCards)
        {

            Image artwork = card.GetComponent<Image>();
            //Text nutritionalValue = card.GetComponentInChildren<Text>();
            Button button = card.GetComponent<Button>();
            GameObject energy = card.transform.Find("Energy").gameObject;

            TextMeshProUGUI energyCost = energy.GetComponentInChildren<TextMeshProUGUI>();
            GameObject coin = card.transform.Find("Coin").gameObject;
            TextMeshProUGUI price = coin.GetComponentInChildren<TextMeshProUGUI>();

            button.onClick.RemoveAllListeners();
            artwork.color = Color.white;

            CardBaseInfo randomCard = GetRandomManoeuvre();
            artwork.sprite = randomCard.Artwork;
            energyCost.text = $"{randomCard.EnergyCost}";


            //CARD PRICE
            price.text = $"{Random.Range(20, 41)}";

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
        IsInSensei = true;
        AssignSenseiCards();
    }
}
