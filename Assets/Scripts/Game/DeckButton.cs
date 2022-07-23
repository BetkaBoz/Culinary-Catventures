using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Deck deckPrefab;

    private Player player;
    private UILayer uiLayer;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        uiLayer = GameObject.FindGameObjectWithTag("UILayer").GetComponent<UILayer>();
        button = GetComponent<Button>();
    }

    public void ShowPlayerDeckInIsland()
    {
        Debug.Log("Show Deck!");
        Deck deckGameObject = Instantiate(deckPrefab,uiLayer.transform);

        deckGameObject.GenerateDeck(player.Deck);
        Adjust();


    }
    //TODO PRIDAT LISTY PODLA KTORYCH SA MA GENEROVAT DECK
    public void ShowPlayerDeckInBattle()
    {
      
        Debug.Log("Show Deck!");
        Deck deckGameObject = Instantiate(deckPrefab,uiLayer.transform);

        deckGameObject.GenerateDeck(player.Deck);
        Adjust();


    }
    
    public void ShowPlayerDiscardInBattle()
    {
        Debug.Log("Show Discard!");
        Deck deckGameObject = Instantiate(deckPrefab,uiLayer.transform);

        deckGameObject.GenerateDeck(player.Deck);
        Adjust();

    }

    private void Adjust()
    {
        Time.timeScale = 0;
        EventManager.IsInEvent = true;

    }
    
}
