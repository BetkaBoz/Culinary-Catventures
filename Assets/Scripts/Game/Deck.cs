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
            GameObject cardGameObject = Instantiate(cardPrefab,panel.transform);

            cardGameObject.GetComponent<Image>().sprite = card.Artwork;
            cardGameObject.transform.Find("Energy").gameObject.GetComponentInChildren<TextMeshProUGUI>().text =  $"{card.EnergyCost}";
            /*if (card.CardType != "Manoeuvre")
            {
                cardGameObject.GetComponentInChildren<Text>().text= $"{card.NutritionPoints}";
            }*/
            //Debug.Log(card.NutritionPoints );
            if (card.NutritionPoints == 0)
            {
                cardGameObject.GetComponentInChildren<Text>().text= "";
            }
            else
            {
                cardGameObject.GetComponentInChildren<Text>().text= $"{card.NutritionPoints}";
            }
        }
    }
    
    public void GenerateDeckInMerchant(List<CardBaseInfo> deck)
    {
        //Debug.Log("GenerateDeck!");
        foreach (CardBaseInfo card in deck)
        {
            //Debug.Log(card);
            GameObject cardGameObject;
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            UILayer uiLayer = GameObject.FindGameObjectWithTag("UILayer").GetComponent<UILayer>();

            if (card.NutritionPoints > 0)
            {
                cardGameObject = Instantiate(merchantCardPrefab,panel.transform);
                GameObject coin = cardGameObject.transform.Find("Coin").gameObject;
                TextMeshProUGUI price = coin.GetComponentInChildren<TextMeshProUGUI>();
                GameObject energy = cardGameObject.transform.Find("Energy").gameObject;
                Text text = cardGameObject.GetComponentInChildren<Text>();
                
                int value = card.NutritionPoints / 2;
                price.text =  $"+{value}";
                price.color =  Color.green;
                energy.GetComponentInChildren<TextMeshProUGUI>().text =  $"{card.EnergyCost}";

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
                cardGameObject = Instantiate(cardPrefab,panel.transform);
                cardGameObject.transform.Find("Energy").gameObject.GetComponentInChildren<TextMeshProUGUI>().text =  $"{card.EnergyCost}";
            }
            
            cardGameObject.GetComponent<Image>().sprite = card.Artwork;
            /*if (card.CardType != "Manoeuvre")
            {
                cardGameObject.GetComponentInChildren<Text>().text= $"{card.NutritionPoints}";
            }*/
            //Debug.Log(card.NutritionPoints );
            if (card.NutritionPoints == 0)
            {
                cardGameObject.GetComponentInChildren<Text>().text= "";
            }
            else
            {
                cardGameObject.GetComponentInChildren<Text>().text= $"{card.NutritionPoints}";
            }
        }
    }
    
    
     public void GenerateDeckInSensei(List<CardBaseInfo> deck)
    {
        //Debug.Log("GenerateDeck!");
        foreach (CardBaseInfo card in deck)
        {
            GameObject cardGameObject;
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            UILayer uiLayer = GameObject.FindGameObjectWithTag("UILayer").GetComponent<UILayer>();

            if (card.NutritionPoints == 0)
            {
                cardGameObject = Instantiate(merchantCardPrefab,panel.transform);
                GameObject coin = cardGameObject.transform.Find("Coin").gameObject;
                TextMeshProUGUI price = coin.GetComponentInChildren<TextMeshProUGUI>();
                GameObject energy = cardGameObject.transform.Find("Energy").gameObject;
                Text text = cardGameObject.GetComponentInChildren<Text>();
                
                int value = 50;
                price.text =  $"-{value}";
                price.color =  Color.red;
                energy.GetComponentInChildren<TextMeshProUGUI>().text =  $"{card.EnergyCost}";

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
                cardGameObject = Instantiate(cardPrefab,panel.transform);
                cardGameObject.transform.Find("Energy").gameObject.GetComponentInChildren<TextMeshProUGUI>().text =  $"{card.EnergyCost}";

            }

            cardGameObject.GetComponent<Image>().sprite = card.Artwork;
            /*if (card.CardType != "Manoeuvre")
            {
                cardGameObject.GetComponentInChildren<Text>().text= $"{card.NutritionPoints}";
            }*/
            //Debug.Log(card.NutritionPoints );
            if (card.NutritionPoints == 0)
            {
                cardGameObject.GetComponentInChildren<Text>().text= "";
            }
            else
            {
                cardGameObject.GetComponentInChildren<Text>().text= $"{card.NutritionPoints}";
            }
        }
    }
    
    public void GenerateDeckInBattle(List<Card> deck)
    {
        //Debug.Log("GenerateDeck!");

        foreach (Card card in deck)
        {
            //Debug.Log(card);
            GameObject cardGameObject =   Instantiate(cardPrefab,panel.transform);

            cardGameObject.GetComponent<Image>().sprite = card.Artwork;
            cardGameObject.transform.Find("Energy").gameObject.GetComponentInChildren<TextMeshProUGUI>().text =  $"{card.EnergyCost}";
            /*if (card.CardType != "Manoeuvre")
            {
                cardGameObject.GetComponentInChildren<Text>().text= $"{card.NutritionPoints}";
            }*/
            //Debug.Log(card.NutritionPoints );
            if (card.NutritionPoints == 0)
            {
                cardGameObject.GetComponentInChildren<Text>().text= "";
            }
            else
            {
                cardGameObject.GetComponentInChildren<Text>().text= $"{card.NutritionPoints}";
            }
        }
    }

    private void GetDeckType(List<Object> deck)
    {
        if (deck.GetType() == typeof(List<CardBaseInfo>))
        {
            Debug.Log(deck.GetType() );
        }
        else
        {
            Debug.Log(deck.GetType() );
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
        EventManager.IsInEvent = false;

        if (MerchantWindowControl.IsInMerchant || SenseiWindowControl.IsInSensei )
        {
            EventManager.IsInEvent = true;
        }

    }
    
    private void OnDestroy()
    {
        //Debug.Log("Deck destroyed!");
    }

}
