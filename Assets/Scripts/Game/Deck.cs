using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

public class Deck : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject panel;


    public void GenerateDeck(List<CardBaseInfo> deck)
    {
        Debug.Log("GenerateDeck!");

        foreach (CardBaseInfo card in deck)
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
    
    public void GenerateDeckInBattle(List<Card> deck)
    {
        Debug.Log("GenerateDeck!");

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
    
    public void Destroy()
    {
        Destroy(gameObject);
        Time.timeScale = 1;
        EventManager.IsInEvent = false;

    }
    
    private void OnDestroy()
    {
        Debug.Log("Deck destroyed!");
    }

}
