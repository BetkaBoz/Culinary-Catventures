using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject panel;


    //List<CardBaseInfo> playerDeck;
    // Start is called before the first frame update
    private void Awake()
    {
        

        //List<CardBaseInfo> playerDeck =player.Deck;
    }
    void Start()
    {
        //GenerateDeck();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateDeck(List<CardBaseInfo> deck )
    {
        Debug.Log("GenerateDeck!");

        //GameObject panel = gameObject.GetComponentInChildren<GridLayoutGroup>().gameObject;
        if (panel == null)
        {
            return;
        }
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
                Debug.Log("Tu SOM" );

                cardGameObject.GetComponentInChildren<Text>().text= "";
            }
            else
            {
                cardGameObject.GetComponentInChildren<Text>().text= $"{card.NutritionPoints}";

            }
        }
    }
/*
    public void ShowDeck(List<CardBaseInfo> deck)
    {
        Instantiate(deckPrefab,uiLayer.transform);
    }
    public void ShowPlayerDeck()
    {
        Debug.Log("ShowPlayerDeck!");

        Instantiate(deckPrefab,uiLayer.transform);
    }*/
    public void Destroy()
    {
        Destroy(gameObject);
    }
    
    private void OnDestroy()
    {
        Debug.Log("Deck destroyed!");
    }

}
