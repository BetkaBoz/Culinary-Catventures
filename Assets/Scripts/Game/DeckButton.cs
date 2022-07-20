using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject deckPrefab;

    private Player player;
    private UILayer uiLayer;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        uiLayer = GameObject.FindGameObjectWithTag("UILayer").GetComponent<UILayer>();
        button = GetComponent<Button>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ShowPlayerDeck()
    {
        Debug.Log("ShowPlayerDeck!");
        GameObject deckGameObject = Instantiate(deckPrefab,uiLayer.transform);

        deckGameObject.GetComponent<Deck>().GenerateDeck(player.Deck);
    }
}
