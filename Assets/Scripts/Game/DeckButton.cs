using TMPro;
using UnityEngine;

public class DeckButton : MonoBehaviour
{
    //[SerializeField] private Button button;
    [SerializeField] private Deck deckPrefab;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject cardLayer;
    //private Player player;


    private void Awake()
    {
        //GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if (gameObject.name == "DeckButtonIsland")
        {
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
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        UILayer uiLayer = GameObject.FindGameObjectWithTag("UILayer").GetComponent<UILayer>();

        Deck deckGameObject = Instantiate(deckPrefab, uiLayer.transform);

        deckGameObject.GenerateDeck(player.Deck);
        deckGameObject.ChangeName("DECK");

        Adjust();
    }
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
