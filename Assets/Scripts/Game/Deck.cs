using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

public class Deck : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject merchantCardPrefab;

    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI name;

    private void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            Destroy();
        }
    }

    public void GenerateDeck(List<CardBaseInfo> deck)
    {
        //Debug.Log("GenerateDeck!");

        foreach (CardBaseInfo card in deck)
        {
            //Debug.Log(card);
            GameObject cardGameObject = Instantiate(cardPrefab, panel.transform);

            cardGameObject.GetComponent<Image>().sprite = card.Artwork;
            cardGameObject.transform.Find("Energy").gameObject.GetComponentInChildren<TextMeshProUGUI>().text = $"{card.EnergyCost}";
            /*if (card.CardType != "Manoeuvre")
            {
                cardGameObject.GetComponentInChildren<Text>().text= $"{card.NutritionPoints}";
            }*/
            //Debug.Log(card.NutritionPoints );
            if (card.NutritionPoints == 0)
            {
                cardGameObject.GetComponentInChildren<Text>().text = "";
            }
            else
            {
                cardGameObject.GetComponentInChildren<Text>().text = $"{card.NutritionPoints}";
            }
        }
        //panel.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.Unconstrained;
    }

    private void SetUpCard(CardBaseInfo card, GameObject cardGameObject, bool merchant = true)
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        UILayer uiLayer = GameObject.FindGameObjectWithTag("UILayer").GetComponent<UILayer>();
        //GameObject cardGameObject = null;

        GameObject energy = cardGameObject.transform.Find("Energy").gameObject;
        energy.GetComponentInChildren<TextMeshProUGUI>().text = $"{card.EnergyCost}";
        Text nutritionalValue = cardGameObject.GetComponentInChildren<Text>();
        cardGameObject.GetComponent<Image>().sprite = card.Artwork;

        if (merchant)
        {
            //MERCHANT
            if (card.NutritionPoints > 0)
            {
                //cardGameObject = Instantiate(merchantCardPrefab, panel.transform);
                cardGameObject.GetComponentInChildren<Text>().text = $"{card.NutritionPoints}";

                GameObject coin = cardGameObject.transform.Find("Coin").gameObject;
                TextMeshProUGUI price = coin.GetComponentInChildren<TextMeshProUGUI>();
                int value = card.NutritionPoints / 2;
                price.text = $"+{value}";
                price.color = Color.green;

                Button button = cardGameObject.GetComponent<Button>();
                button.onClick.RemoveAllListeners();

                button.onClick.AddListener(delegate {
                    SellCard(card, cardGameObject, player, uiLayer, price, coin, nutritionalValue, energy, button);
                    /*uiLayer.ChangeMoney(int.Parse(price.text));
                    player.Deck.Remove(card);
                    cardGameObject.GetComponent<Image>().color = Color.gray;
                    price.color = Color.gray;
                    coin.GetComponentInChildren<Image>().color = Color.gray;
                    nutritionalValue.color = Color.gray;
                    foreach (Transform child in energy.transform)
                    {
                        if (child.GetComponent<Image>())
                        {
                            child.GetComponent<Image>().color = Color.grey;
                        }
                        else if (child.GetComponent<TextMeshProUGUI>())
                        {
                            child.GetComponent<TextMeshProUGUI>().color = Color.gray;
                        }
                    }

                    Destroy(button);*/
                });
            }
            else
            {
                //cardGameObject = Instantiate(cardPrefab, panel.transform);
                nutritionalValue.text = "";
            }
        }
        else
        {
            //SENSEI
            if (card.NutritionPoints == 0)
            {
                //cardGameObject = Instantiate(merchantCardPrefab, panel.transform);
                cardGameObject.GetComponentInChildren<Text>().text = "";
                GameObject coin = cardGameObject.transform.Find("Coin").gameObject;
                TextMeshProUGUI price = coin.GetComponentInChildren<TextMeshProUGUI>();

                int value = 50;
                price.text = $"-{value}";
                price.color = Color.red;
                energy.GetComponentInChildren<TextMeshProUGUI>().text = $"{card.EnergyCost}";

                Button button = cardGameObject.GetComponent<Button>();
                button.onClick.RemoveAllListeners();

                button.onClick.AddListener(delegate {
                    if (!player.HaveMoney(value))
                    {
                        uiLayer.ShowCoinsNotification();
                        return;
                    }
                    SellCard(card, cardGameObject, player, uiLayer, price, coin, nutritionalValue, energy, button);
                    /*uiLayer.ChangeMoney(int.Parse(price.text));
                    player.Deck.Remove(card);
                    cardGameObject.GetComponent<Image>().color = Color.gray;
                    price.color = Color.gray;
                    coin.GetComponentInChildren<Image>().color = Color.gray;
                    //energy.GetComponentsInChildren<Image>();
                    //nutritionalValue.color = Color.gray;
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
                    Destroy(button);*/
                });

            }
            else
            {
                //cardGameObject = Instantiate(cardPrefab, panel.transform);
                nutritionalValue.text = $"{card.NutritionPoints}";
            }
        }


        /*if (card.CardType != "Manoeuvre")
        {
            cardGameObject.GetComponentInChildren<Text>().text= $"{card.NutritionPoints}";
        }*/
        /*
        if (card.NutritionPoints == 0)
        {
            cardGameObject.GetComponentInChildren<Text>().text = "";
        }
        else
        {
            cardGameObject.GetComponentInChildren<Text>().text = $"{card.NutritionPoints}";
        }*/


    }


    public void GenerateDeckInMerchant(List<CardBaseInfo> deck)
    {
        int count = 0;
        int rows = 1;
        //Debug.Log("GenerateDeck!");
        foreach (CardBaseInfo card in deck)
        {
            GameObject cardGameObject;
            //Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            //UILayer uiLayer = GameObject.FindGameObjectWithTag("UILayer").GetComponent<UILayer>();
            if (card.NutritionPoints > 0)
            {
                cardGameObject = Instantiate(merchantCardPrefab, panel.transform);
            }
            else
            {
                cardGameObject = Instantiate(cardPrefab, panel.transform);
            }

            SetUpCard(card, cardGameObject, true);
            /*
            if (card.NutritionPoints > 0)
            {
                cardGameObject = Instantiate(merchantCardPrefab, panel.transform);

                GameObject coin = cardGameObject.transform.Find("Coin").gameObject;
                TextMeshProUGUI price = coin.GetComponentInChildren<TextMeshProUGUI>();
                GameObject energy = cardGameObject.transform.Find("Energy").gameObject;
                Text text = cardGameObject.GetComponentInChildren<Text>();

                int value = card.NutritionPoints / 2;
                price.text = $"+{value}";
                price.color = Color.green;
                energy.GetComponentInChildren<TextMeshProUGUI>().text = $"{card.EnergyCost}";

                Button button = cardGameObject.GetComponent<Button>();
                button.onClick.RemoveAllListeners();

                button.onClick.AddListener(delegate {
                    uiLayer.ChangeMoney(int.Parse(price.text));
                    player.Deck.Remove(card);
                    cardGameObject.GetComponent<Image>().color = Color.gray;
                    price.color = Color.gray;
                    coin.GetComponentInChildren<Image>().color = Color.gray;
                    //energy.GetComponentsInChildren<Image>();
                    text.color = Color.gray;
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
            else
            {
                cardGameObject = Instantiate(cardPrefab, panel.transform);
                cardGameObject.transform.Find("Energy").gameObject.GetComponentInChildren<TextMeshProUGUI>().text = $"{card.EnergyCost}";
            }

            cardGameObject.GetComponent<Image>().sprite = card.Artwork;*/
            /*if (card.CardType != "Manoeuvre")
            {
                cardGameObject.GetComponentInChildren<Text>().text= $"{card.NutritionPoints}";
            }*/
            /*if (card.NutritionPoints == 0)
            {
                cardGameObject.GetComponentInChildren<Text>().text = "";
            }
            else
            {
                cardGameObject.GetComponentInChildren<Text>().text = $"{card.NutritionPoints}";
            }*/
            count++;
            if (count > 1 && count % 5 == 1)
            {
                rows++;
            }
        }

        SetPanelHeight(rows);
    }



    public void GenerateDeckInSensei(List<CardBaseInfo> deck)
    {
        //Debug.Log("GenerateDeck!");
        int count = 0;
        int rows = 1;
        foreach (CardBaseInfo card in deck)
        {
            GameObject cardGameObject;
            if (card.NutritionPoints == 0)
            {
                cardGameObject = Instantiate(merchantCardPrefab, panel.transform);
            }
            else
            {
                cardGameObject = Instantiate(cardPrefab, panel.transform);
            }

            //Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            //UILayer uiLayer = GameObject.FindGameObjectWithTag("UILayer").GetComponent<UILayer>();
            /*
            if (card.NutritionPoints == 0)
            {
                cardGameObject = Instantiate(merchantCardPrefab, panel.transform);
                GameObject coin = cardGameObject.transform.Find("Coin").gameObject;
                TextMeshProUGUI price = coin.GetComponentInChildren<TextMeshProUGUI>();
                GameObject energy = cardGameObject.transform.Find("Energy").gameObject;
                Text text = cardGameObject.GetComponentInChildren<Text>();

                int value = 50;
                price.text = $"-{value}";
                price.color = Color.red;
                energy.GetComponentInChildren<TextMeshProUGUI>().text = $"{card.EnergyCost}";

                Button button = cardGameObject.GetComponent<Button>();
                button.onClick.RemoveAllListeners();

                button.onClick.AddListener(delegate {
                    if (!player.HaveMoney(value))
                    {
                        uiLayer.ShowCoinsNotification();
                        return;
                    }
                    uiLayer.ChangeMoney(int.Parse(price.text));
                    player.Deck.Remove(card);
                    cardGameObject.GetComponent<Image>().color = Color.gray;
                    price.color = Color.gray;
                    coin.GetComponentInChildren<Image>().color = Color.gray;
                    //energy.GetComponentsInChildren<Image>();
                    text.color = Color.gray;
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
            else
            {
                cardGameObject = Instantiate(cardPrefab, panel.transform);
                cardGameObject.transform.Find("Energy").gameObject.GetComponentInChildren<TextMeshProUGUI>().text = $"{card.EnergyCost}";

            }
            */
            //cardGameObject.GetComponent<Image>().sprite = card.Artwork;

            /*if (card.CardType != "Manoeuvre")
            {
                cardGameObject.GetComponentInChildren<Text>().text= $"{card.NutritionPoints}";
            }*/
            /*if (card.NutritionPoints == 0)
            {
                cardGameObject.GetComponentInChildren<Text>().text = "";
            }
            else
            {
                cardGameObject.GetComponentInChildren<Text>().text = $"{card.NutritionPoints}";
            }*/

            SetUpCard(card, cardGameObject, false);
            count++;
            if (count > 1 && count % 5 == 1)
            {
                rows++;
            }
        }
        SetPanelHeight(rows);

    }

    public void GenerateDeckInBattle(List<Card> deck)
    {
        //Debug.Log("GenerateDeck!");

        foreach (Card card in deck)
        {
            //Debug.Log(card);
            GameObject cardGameObject = Instantiate(cardPrefab, panel.transform);

            cardGameObject.GetComponent<Image>().sprite = card.Artwork;
            cardGameObject.transform.Find("Energy").gameObject.GetComponentInChildren<TextMeshProUGUI>().text = $"{card.EnergyCost}";
            /*if (card.CardType != "Manoeuvre")
            {
                cardGameObject.GetComponentInChildren<Text>().text= $"{card.NutritionPoints}";
            }*/
            //Debug.Log(card.NutritionPoints );
            if (card.NutritionPoints == 0)
            {
                cardGameObject.GetComponentInChildren<Text>().text = "";
            }
            else
            {
                cardGameObject.GetComponentInChildren<Text>().text = $"{card.NutritionPoints}";
            }
        }
    }

    private void GetDeckType(List<Object> deck)
    {
        if (deck.GetType() == typeof(List<CardBaseInfo>))
        {
            Debug.Log(deck.GetType());
        }
        else
        {
            Debug.Log(deck.GetType());
        }
    }


    public void ChangeName(string input)
    {
        name.text = $"{input}";
    }
    public void Destroy()
    {
        Destroy(gameObject);
        //Time.timeScale = 1;

        if (MerchantWindowControl.IsInMerchant || SenseiWindowControl.IsInSensei)
        {
            EventManager.IsInEvent = true;
            Debug.Log("IsInEvent = " + EventManager.IsInEvent);
        }
        else
        {
            EventManager.IsInEvent = false;
            Debug.Log("IsInEvent = " + EventManager.IsInEvent);
        }

    }
    //MANUALNE NASTAVOVANIE VYSKY PANELU PRI MERCHANT/SENSEI EVENTE
    //(ABY SA UKAZOVALA CENA DOLE)
    private void SetPanelHeight(int rows)
    {
        int height = 333 * rows + (rows - 1) * 67 + 44;
        Debug.Log(height);
        panel.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.Unconstrained;
        Vector2 sizeDelta = panel.GetComponent<RectTransform>().sizeDelta;
        sizeDelta = new Vector2(sizeDelta.x, height);
        panel.GetComponent<RectTransform>().sizeDelta = sizeDelta;
    }
    
    private void SellCard(
        CardBaseInfo card, GameObject cardGameObject, Player player, UILayer uiLayer,
        TextMeshProUGUI price, GameObject coin, Text nutritionalValue, GameObject energy, Button button
    )
    {
        uiLayer.ChangeMoney(int.Parse(price.text));
        player.Deck.Remove(card);
        cardGameObject.GetComponent<Image>().color = Color.gray;
        price.color = Color.gray;
        coin.GetComponentInChildren<Image>().color = Color.gray;
        nutritionalValue.color = Color.gray;
        foreach (Transform child in energy.transform)
        {
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
    }
}
