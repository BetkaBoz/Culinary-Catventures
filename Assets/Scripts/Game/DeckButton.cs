using TMPro;
using UnityEngine;

public class DeckButton : MonoBehaviour
{
    //[SerializeField] private Button button;
    [SerializeField] private Deck deckPrefab;
    [SerializeField] private GameObject spawnLocation;
    [SerializeField] private Player player;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject cardLayer;
    //private Player player;


    private void Awake()
    {
        if (gameObject.name == "DeckButtonIsland"|| gameObject.name == "SellButton")
    {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            enabled = false;
        }
    }
    private void Update()
    {
        UpdateNumber();
    }

    public void ShowPlayerDeckInIsland()
    {
        //Debug.Log("Show Deck!");
        //Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        //UILayer uiLayer = GameObject.FindGameObjectWithTag("UILayer").GetComponent<UILayer>();
        //GameObject spawn = GameObject.FindGameObjectWithTag("Deck");

        Deck deckGameObject = Instantiate(deckPrefab, spawnLocation.transform);

        deckGameObject.GenerateDeck(player.Deck);
        deckGameObject.ChangeName("DECK");
        
        Adjust();
    }
    public void ShowPlayerDeckInMerchant()
    {
        //Debug.Log("Show Deck!");
        //Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        //GameObject spawn = GameObject.FindGameObjectWithTag("Deck");

        Deck deckGameObject = Instantiate(deckPrefab, spawnLocation.transform);

        deckGameObject.GenerateDeckInMerchant(player.Deck);
        deckGameObject.ChangeName("DECK");
        Adjust();
    }
    public void ShowPlayerDeckInSensei()
    {
        //Debug.Log("Show Deck!");
        //Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        //GameObject spawn = GameObject.FindGameObjectWithTag("Deck");

        Deck deckGameObject = Instantiate(deckPrefab, spawnLocation.transform);

        deckGameObject.GenerateDeckInSensei(player.Deck);
        deckGameObject.ChangeName("DECK");
        Adjust();
    }
    
    /*public void MoveInHierarchy(int delta, GameObject deck) {
        int index = deck.transform.GetSiblingIndex();
        transform.SetSiblingIndex (index + delta);
    }*/
    //TODO PRIDAT LISTY PODLA KTORYCH SA MA GENEROVAT DECK
    public void ShowPlayerDeckInBattle()
    {
        //Debug.Log("Show Deck!");
        //GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        //GameObject cardLayer = GameObject.Find("Card Layer");
        Deck deckGameObject = Instantiate(deckPrefab, cardLayer.transform);

        deckGameObject.GenerateDeckInBattle(gameManager.deck);
        deckGameObject.ChangeName("DECK");

        Adjust();
    }

    public void ShowPlayerDiscardInBattle()
    {
        //Debug.Log("Show Discard!");
        //GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        //GameObject cardLayer = GameObject.Find("Card Layer");
        Deck deckGameObject = Instantiate(deckPrefab, cardLayer.transform);

        deckGameObject.GenerateDeckInBattle(gameManager.discardPile);
        deckGameObject.ChangeName("DISCARD");

        Adjust();
    }

    private void Adjust()
    {
        //Time.timeScale = 1;
        EventManager.IsInEvent = true;
    }

    private void UpdateNumber()
    {
        //GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        if (gameObject.name == "DeckButton")
        {
            TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();

            text.text = $"{gameManager.deck.Count}";

        }
        else if (gameObject.name == "DiscardButton")
        {
            TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
            text.text = $"{gameManager.discardPile.Count}";
        }
    }
}
