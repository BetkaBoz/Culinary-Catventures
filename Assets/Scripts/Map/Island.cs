using UnityEngine;
using UnityEngine.SceneManagement;


public class Island : MonoBehaviour
{
    private enum IslandTypes 
    { Start,Finish,Flower,Town,City,Box,Crater,Globe,Night } 
    
    private enum OptionalPlaces
    { Market,Hotel,Event,Dealer,School }
    
    [SerializeField] private IslandTypes IslandType { get; set; }
    [SerializeField] private int sector;


    public string GetIslandType()
    {
        return this.IslandType.ToString();
    }
    
    public void SetStartType( )
    {
        this.IslandType = IslandTypes.Start;
    }
    public void SetFinishType( )
    {
        this.IslandType = IslandTypes.Finish;
    }
    
    public void SetRandomIslandType( )
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
    
    private void OnMouseDown()
    {
        Debug.Log("Clicked");
        SceneManager.LoadScene("Battle");
    }
}
