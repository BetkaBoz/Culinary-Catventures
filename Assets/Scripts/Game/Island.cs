using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;


public class Island : MonoBehaviour
{
    private enum IslandTypes 
    { Start,Finish,Flower,Town,City,Box,Crater,Globe,Night } 
    
    private enum OptionalPlaces
    { Market,Hotel,Event,Dealer,School }
    
    [SerializeField] private IslandTypes IslandType { get; set; }
    [SerializeField] private int Sector;


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
        Debug.Log(IslandType);
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
