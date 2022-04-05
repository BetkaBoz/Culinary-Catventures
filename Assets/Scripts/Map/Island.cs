using UnityEngine;


public class Island : MonoBehaviour
{
    private enum IslandTypes 
    { Start,Finish,Flower,Town,City,Box,Crater,Globe,Night } 
    
    private enum OptionalPlaces
    { Market,Hotel,Event,Dealer,School }
    
    [SerializeField] private IslandTypes IslandType { get; set; }
    [SerializeField] private int Sector;

    private bool isVisiting = false;
    private Island previousIsland = null;
    private Island nextIsland = null;


    public void switchIsVisiting()
    {
        isVisiting = !isVisiting;
        
        Color currentColor = this.GetComponent<SpriteRenderer>().color;
        if (isVisiting)
        {
            currentColor.a = 0;
        }
        else
        {
            currentColor.a = 255;
        }
        this.GetComponent<SpriteRenderer>().color = currentColor;
    }

    public bool IsVisiting
    {
        get => isVisiting;
    }

    public Island PreviousIsland
    {
        get => previousIsland;
        set => previousIsland = value;
    }

    public Island NextIsland
    {
        get => nextIsland;
        set => nextIsland = value;
    }

    public string getIslandType()
    {
        return this.IslandType.ToString();
    }
    
    public void setStartType( )
    {
        this.IslandType = IslandTypes.Start;
    }
    public void setFinishType( )
    {
        this.IslandType = IslandTypes.Finish;
    }
    
    public void setRandomIslandType( )
    {
        IslandType = (IslandTypes) Random.Range(2, 8);
        // Debug.Log(IslandType);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
